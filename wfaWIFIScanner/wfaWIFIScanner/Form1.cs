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
            timer.Interval = 1000; // ������ � ���������� � 1 �������
            timer.Tick += Timer_Tick; // ���������� �������, ������� ����� ���������� ������ ���
        }

        private void StartTimer()
        {
            if (is_scan)
            {
                secondsRemaining = 5; // ������������� 5 ������
                UpdateTimerDisplay();
                timer.Start(); // ��������� ������
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (is_scan)
            {
                secondsRemaining--; // ��������� ���������� ���������� ������
                UpdateTimerDisplay();

                if (secondsRemaining <= 0)
                {
                    timer.Stop(); // ������������� ������, ����� ������ ����������

                    // ��������� ������� (DataGrid)
                    if (ViewModel.WifiNetworks != null)
                    {
                        ViewModel.WifiNetworks.Clear();
                    }

                    // ���������� BestNetworkLabel � ���������� ��� ������� ������
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

                    // ������ ����� ����� ��������� �������� �������
                    BeginInvoke(new Action(async () =>
                    {
                        await Task.Delay(2000); // ���� 2 �������
                        StartTimer(); // ��������� ������ �����
                    }));
                }
            }
        }


        // ���������� ����������� ������� � UI
        private void UpdateTimerDisplay()
        {
            if (TimerLabel.InvokeRequired)
            {
                // ���������� Invoke ��� ���������� ���� � ������ ����������������� ����������
                TimerLabel.Invoke(new Action(() =>
                {
                    TimerLabel.Text = $"�������� �������: {secondsRemaining} ������";
                }));
            }
            else
            {
                // ��������� �����, ���� ����� ���� �� ������ ����������������� ����������
                TimerLabel.Text = $"�������� �������: {secondsRemaining} ������";
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

            // ������� ���� ������
            dbInsert.Clear();

            // ��������� ������
            foreach (var network in ViewModel.WifiNetworks)
            {
                try
                {
                    dbInsert.InsertNetworks(network.SSID, network.SignalQuality);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"������ ��� ���������� ������: {ex.Message}");
                }
            }
        }

        private void ShowData_Click(object sender, EventArgs e)
        {
            var dbInsert = new MyLibrary.DBInsert();

            try
            {
                string data = dbInsert.GetAllNetworks();
                MessageBox.Show(data, "����������� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("�������� ���� ��� ����������� �������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Network_DataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            // ���������, ��� ���� ��� �� �� ���������
            var hitTestInfo = Network_DataGridView.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex >= 0) // ��������, ��� ��� ������, � �� ���������
            {
                // �������� ��������� ������
                var selectedRow = Network_DataGridView.Rows[hitTestInfo.RowIndex];

                // ��������� ������ ���� WifiNetwork �� ������
                if (selectedRow.DataBoundItem is WifiNetwork selectedNetwork)
                {
                    _selectedNetwork = selectedNetwork;
                }
                else
                {
                    MessageBox.Show("��������� ������� �� ������������� ���� WifiNetwork.");
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
            // ���������, ���� ���� ������
            if (ViewModel.WifiNetworks == null || ViewModel.WifiNetworks.Count == 0)
            {
                MessageBox.Show("��� ������ ��� �����������.");
                return;
            }

            // ������������� �������� ������
            Network_DataGridView.DataSource = null; // ���������� �������� ������
            Network_DataGridView.DataSource = ViewModel.WifiNetworks;
        }

        private void TimerLabel_Click(object sender, EventArgs e)
        {

        }
    }
}