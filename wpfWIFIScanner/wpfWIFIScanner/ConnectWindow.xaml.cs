using MyLibrary;
using System.Windows;

namespace wpfWIFIScanner
{
    public partial class ConnectWindow : Window
    {
        private readonly WifiModel _wifiModel;

        public ConnectWindow(string ssid, WifiModel wifiModel)
        {
            InitializeComponent();
            SSIDTextBox.Text = ssid; // Устанавливаем выбранный SSID
            _wifiModel = wifiModel;  // Передаем экземпляр WifiModel
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string ssid = SSIDTextBox.Text;
            string password = PasswordBox.Password;

            // Подключаемся к сети
            string result = _wifiModel.ConnectToNetwork(ssid, password);

            // Показываем результат пользователю
            MessageBox.Show(result, "Статус подключения", MessageBoxButton.OK, MessageBoxImage.Information);

            // Закрываем окно после попытки подключения
            Close();
        }
    }
}
