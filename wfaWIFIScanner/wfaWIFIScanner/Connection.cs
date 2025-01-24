using MyLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;

namespace wfaWIFIScanner
{
    public partial class Connection : Form
    {
        private readonly WifiModel _wifiModel;

        public Connection(string ssid, WifiModel wifiModel)
        {
            InitializeComponent();
            SSID_textBox.Text = ssid; // Устанавливаем выбранный SSID
            _wifiModel = wifiModel;  // Передаем экземпляр WifiModel
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            string ssid = SSID_textBox.Text;
            string password = Password_textBox.Text;

            // Подключаемся к сети
            string result = _wifiModel.ConnectToNetwork(ssid, password);

            // Показываем результат пользователю
            //MessageBox.Show(result, "Статус подключения", MessageBoxButton.OK, MessageBoxImage.Information);

            // Закрываем окно после попытки подключения
            Close();
        }
    }
}
