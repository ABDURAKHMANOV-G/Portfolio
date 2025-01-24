using SimpleWifi;
using SimpleWifi.Win32.Interop;
using SimpleWifi.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;



namespace MyLibrary
{
    public class WifiModel
    {
        // Коллекция для хранения списка сетей
        public ObservableCollection<WifiNetwork> WifiNetworks { get; set; }
        
        private WifiNetwork _bestNetwork;
        public WifiNetwork BestNetwork
        {
            get => _bestNetwork;
            set
            {
                _bestNetwork = value;
                OnPropertyChanged(nameof(BestNetwork)); // Уведомляем об изменении
                OnPropertyChanged(nameof(BestNetworkInfo)); // Уведомляем об изменении объединённой строки
            }
        }
        private WlanClient _wifiScanClient;
        private Wifi _wifiConnectedClient;

        public WifiModel()
        {
            // Инициализируем клиента WiFi
            _wifiScanClient = new WlanClient();
            WifiNetworks = new ObservableCollection<WifiNetwork>();
            _wifiConnectedClient = new Wifi();
        }

        //Метод для получения списка доступных сетей
        public string ScanNetworks()
        {
            //Очищаем текущие данные и добавляем новые
            WifiNetworks.Clear();


            //Првоерка подключения к сети
            if (_wifiConnectedClient.ConnectionStatus == WifiStatus.Disconnected)
            {
                return "Сканирование доступно только при подключении к сети";
            }

            var _wifiNetworks = _wifiScanClient.Interfaces[0].GetAvailableNetworkList(0).ToList();
            var seenSSIDs = new HashSet<string>(); // Хранилище для уникальных SSID
            var netList = _wifiNetworks.Select(x =>
            {

                var network = new WifiNetwork
                {
                    SSID = GetStringForSSID(x.dot11Ssid),
                    SignalQuality = x.wlanSignalQuality.ToString() + "%"
                };

                return network;
            })
             .Where(n =>
             {
                 // Проверяем уникальность SSID
                 if (seenSSIDs.Contains(n.SSID))
                 {
                     return false; // Пропускаем дубликат
                 }
                 seenSSIDs.Add(n.SSID);
                 return true; // Добавляем уникальную сеть
             })
            .OrderByDescending(x => int.Parse(x.SignalQuality.Replace("%", ""))) // Сортировка по качеству сигнала
            .ToList();

            foreach (var network in netList)
            {
                WifiNetworks.Add(network);
            }

            //Лучшая сеть
            BestNetwork = netList.FirstOrDefault();
            return $"{BestNetwork.SSID}: {BestNetwork.SignalQuality}";
        }


        public string BestNetworkInfo =>
            BestNetwork != null ? $"{BestNetwork.SSID} - {BestNetwork.SignalQuality}" : "Нет доступных сетей";

        private string GetStringForSSID(Dot11Ssid ssid)
        {
            try
            {
                string ssidString = Encoding.UTF8.GetString(ssid.SSID, 0, ssid.SSID.Length).Trim('\0');
                return string.IsNullOrWhiteSpace(ssidString) ? "Скрытая сеть" : ssidString;
            }
            catch
            {
                return "Unknown SSID";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


         // Метод подключения к выбранной сети
        public string ConnectToNetwork(string ssid, string password = null)
        {
            try
            {
                // Получаем сеть по SSID
                var accessPoint = _wifiConnectedClient.GetAccessPoints().FirstOrDefault(ap => ap.Name == ssid);

                if (accessPoint == null)
                    return "Сеть не найдена";

                // Если сеть защищена, проверяем наличие пароля
                if (accessPoint.IsSecure && string.IsNullOrEmpty(password))
                    return "Необходим пароль для подключения к этой сети";

                // Создаем объект для авторизации
                var authRequest = new AuthRequest(accessPoint)
                {
                    Password = password
                };

                // Пытаемся подключиться
                if (accessPoint.Connect(authRequest))
                {
                    return $"Подключение к {ssid} успешно выполнено!";
                }
                else
                {
                    return $"Не удалось подключиться к {ssid}. Проверьте пароль.";
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка подключения: {ex.Message}";
            }
        }
    }
}
