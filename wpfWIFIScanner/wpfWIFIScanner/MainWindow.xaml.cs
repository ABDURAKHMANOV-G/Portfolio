using SimpleWifi;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyLibrary;
using System.Collections.ObjectModel;
using SQLite;
using System.Data.SQLite;
using System.Windows.Threading;
using OxyPlot.Wpf;

namespace wpfWIFIScanner
{
    public partial class MainWindow : Window
    {
        public WifiModel ViewModel { get; set; }
        private DispatcherTimer timer;
        private double secondsRemaining = 5;
        public ObservableCollection<WifiNetwork> WifiNetworks { get; set; } = new ObservableCollection<WifiNetwork>();
        public string BestNetworkInfo { get; set; }
        public bool is_scan = false;
        public int speed;
        private WifiNetwork _selectedNetwork;


        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new WifiModel();
            this.DataContext = ViewModel;

            // Инициализируем таймер
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);  // Таймер с интервалом в 1 секунду
            timer.Tick += Timer_Tick;  // Обработчик события, которое будет вызываться каждый тик


        }

        private void StartTimer()
        {
            if (is_scan)
            {
                secondsRemaining = 5; // Устанавливаем 10 секунд
                UpdateTimerDisplay();
                timer.Start(); // Запускаем таймер
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (is_scan)
            {
                secondsRemaining--; // Уменьшаем количество оставшихся секунд
                UpdateTimerDisplay();

                if (secondsRemaining <= 0)
                {
                    timer.Stop(); // Останавливаем таймер, когда отсчет завершился

                    // Обновляем таблицу (DataGrid)
                    // Обновление DataGrid, например, перезагрузим данные
                    if (ViewModel.WifiNetworks != null)
                    {
                        ViewModel.WifiNetworks.Clear();
                    }
                    BestNetworkLabel.Content = ViewModel.ScanNetworks();

                    // Делаем паузу перед следующим запуском таймера
                    Dispatcher.InvokeAsync(async () =>
                    {
                        await Task.Delay(2000); // Ждем 2 секунды
                        StartTimer(); // Запускаем таймер снова
                    });
                }
            }
        }

                
        // Обновление отображения таймера в UI (например, в TextBlock)
        private void UpdateTimerDisplay()
        {
            // Например, обновляем текст в Label или TextBlock, показывая оставшееся время
            TimerLabel.Content = $"Осталось времени: {secondsRemaining} секунд";
        }

        private void Btn_Scan_Click(object sender, RoutedEventArgs e)
        {
            StartScan_Btn.Visibility = Visibility.Hidden;
            StopScan_Btn.Visibility = Visibility.Visible;
            is_scan = true;
            if (is_scan)
            {
                // Запускаем таймер
                secondsRemaining = 5; // Начинаем отсчет с 10 секунд
                timer.Start();
                UpdateTimerDisplay();

                // Например, запустим сканирование сетей Wi-Fi или любые другие действия, связанные с этим
                BestNetworkLabel.Content = ViewModel.ScanNetworks();
            }
            else
            {
                timer.Stop();
            }
           
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранную сеть
            try
            {
                _selectedNetwork = (WifiNetwork)DataGridView_networks.SelectedItem;
            }
            catch
            {
                MessageBox.Show("Выберите сеть!");
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Получаем доступ к коллекции WifiNetworks
            var wifiNetworks = ViewModel.WifiNetworks;

            var dbInsert = new MyLibrary.DBInsert(); // Создаем экземпляр класса DBInsert

            // Очистка Базы данных
            dbInsert.Clear();

            // Проходим по всем сетям и сохраняем их в базе данных
            foreach (var network in wifiNetworks)
            {
                try
                {
                    dbInsert.InsertNetworks(network.SSID, network.SignalQuality);  // Вставляем данные в базу данных
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
                }
            }
        }

        private void Btn_ShowData_Click(object sender, RoutedEventArgs e)
        {
            var dbInsert = new MyLibrary.DBInsert(); // Создаем экземпляр класса DBInsert

            try
            {
                // Получаем данные из базы
                string data = dbInsert.GetAllNetworks();
                // Создаем новое окно
                DataWindow dataWindow = new DataWindow();

                // Передаем данные в TextBlock
                dataWindow.TextBlock_Data.Text = data;

                // Отображаем окно
                dataWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

        }

        private void Btn_StopScan_Click(object sender, RoutedEventArgs e)
        {
            StopScan_Btn.Visibility = Visibility.Hidden;
            StartScan_Btn.Visibility = Visibility.Visible;
            is_scan = false;
        }

        private void Btn_ShowGraphic_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNetwork != null)
            {
                int.TryParse(GraphicSpeed.Text, out int speed);
                if (GraphicSpeed.Text == null || speed <= 0)
                {
                    speed = 2;
                }

                // Открываем новое окно графика для выбранной сети
                var graphWindow = new Graphics(_selectedNetwork, speed);
                graphWindow.Show(); // Делаем окно немодальным
                graphWindow.StartTimer(); // Запускаем график
            }
            else
            {
                MessageBox.Show("Выберите сеть, для которой хотите отобразить график.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NetworksListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridView_networks.SelectedItem is WifiNetwork selectedNetwork)
            {
                var connectWindow = new ConnectWindow(selectedNetwork.SSID, ViewModel);
                connectWindow.ShowDialog();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}