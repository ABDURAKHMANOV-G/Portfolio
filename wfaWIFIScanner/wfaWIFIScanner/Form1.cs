using SimpleWifi;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLibrary;
using System.Windows.Controls;

namespace wfaWIFIScanner
{
    public partial class Form1 : Form
    {
        public WifiModel ViewModel { get; set; }
        private System.Windows.Forms.Timer timer;
        private double secondsRemaining = 5;
        //public ObservableCollection<WifiNetwork> WifiNetworks { get; set; } = new ObservableCollection<WifiNetwork>();
        public string BestNetworkInfo { get; set; }
        public bool is_scan = false;
        private WifiNetwork _selectedNetwork;
        private string cellValue;


        public Form1()
        {
            InitializeComponent();
            ViewModel = new WifiModel();
            InitTimer();
        }

        private void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Таймер с интервалом в 1 секунду
            timer.Tick += Timer_Tick; // Обработчик события, которое будет вызываться каждый тик
        }

        private void StartTimer()
        {
            if (is_scan)
            {
                secondsRemaining = 5; // Устанавливаем 5 секунд
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
                    if (ViewModel.WifiNetworks != null)
                    {
                        ViewModel.WifiNetworks.Clear();
                    }

                    // Обновление BestNetworkLabel в безопасном для потоков режиме
                    if (BestNetworkLabel.InvokeRequired)
                    {
                        BestNetworkLabel.Invoke(new Action(() =>
                        {
                            BestNetworkLabel.Text = ViewModel.ScanNetworks();
                        }));
                        UpdateDataGrid();
                    }
                    else
                    {
                        BestNetworkLabel.Text = ViewModel.ScanNetworks();
                        UpdateDataGrid();
                    }

                    // Делаем паузу перед следующим запуском таймера
                    BeginInvoke(new Action(async () =>
                    {
                        await Task.Delay(2000); // Ждем 2 секунды
                        StartTimer(); // Запускаем таймер снова
                    }));
                }
            }
        }


        // Обновление отображения таймера в UI
        private void UpdateTimerDisplay()
        {
            if (TimerLabel.InvokeRequired)
            {
                // Используем Invoke для выполнения кода в потоке пользовательского интерфейса
                TimerLabel.Invoke(new Action(() =>
                {
                    TimerLabel.Text = $"Осталось времени: {secondsRemaining} секунд";
                }));
            }
            else
            {
                // Обновляем текст, если вызов идет из потока пользовательского интерфейса
                TimerLabel.Text = $"Осталось времени: {secondsRemaining} секунд";
            }
        }


        private void Scan_Click(object sender, EventArgs e)
        {
            is_scan = true;
            StartTimer();
            BestNetworkLabel.Text = ViewModel.ScanNetworks();
            UpdateDataGrid();
        }

        private void SaveData_Click(object sender, EventArgs e)
        {
            var dbInsert = new MyLibrary.DBInsert();

            // Очистка базы данных
            dbInsert.Clear();

            // Сохраняем данные
            foreach (var network in ViewModel.WifiNetworks)
            {
                try
                {
                    dbInsert.InsertNetworks(network.SSID, network.SignalQuality);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
                }
            }
        }

        private void ShowData_Click(object sender, EventArgs e)
        {
            var dbInsert = new MyLibrary.DBInsert();

            try
            {
                string data = dbInsert.GetAllNetworks();
                MessageBox.Show(data, "Сохраненные сети", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowGraphic_Click(object sender, EventArgs e)
        {
            if (_selectedNetwork != null)
            {
                int.TryParse(GraphicUpdateSpeed.Text, out int speed);
                if (GraphicUpdateSpeed == null || speed <= 0)
                {
                    var graphForm = new GraphForm(_selectedNetwork, 2);
                    graphForm.Show();
                    graphForm.StartTimer();
                }
                else
                {
                    var graphForm = new GraphForm(_selectedNetwork, speed);
                    graphForm.Show();
                    graphForm.StartTimer();
                }
                
                
            }
            else
            {
                MessageBox.Show("Выберите сеть для отображения графика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Network_DataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            // Проверяем, что клик был не на заголовке
            var hitTestInfo = Network_DataGridView.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex >= 0) // Убедимся, что это строка, а не заголовок
            {
                // Получаем выбранную строку
                var selectedRow = Network_DataGridView.Rows[hitTestInfo.RowIndex];

                // Извлекаем объект типа WifiNetwork из строки
                if (selectedRow.DataBoundItem is WifiNetwork selectedNetwork)
                {
                    _selectedNetwork = selectedNetwork;
                }
                else
                {
                    MessageBox.Show("Выбранный элемент не соответствует типу WifiNetwork.");
                }
            }
        }

        private void Network_DataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var connection = new Connection(_selectedNetwork.SSID, ViewModel);
            connection.ShowDialog();
        }


        private void UpdateDataGrid()
        {
            // Проверяем, если есть данные
            if (ViewModel.WifiNetworks == null || ViewModel.WifiNetworks.Count == 0)
            {
                MessageBox.Show("Нет данных для отображения.");
                return;
            }

            // Устанавливаем привязку данных
            Network_DataGridView.DataSource = null; // Сбрасываем источник данных
            Network_DataGridView.DataSource = ViewModel.WifiNetworks;
        }

        private void TimerLabel_Click(object sender, EventArgs e)
        {

        }
    }
}