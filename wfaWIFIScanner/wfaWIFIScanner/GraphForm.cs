using System;
using System.Windows.Forms;
using MyLibrary;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace wfaWIFIScanner
{
    public partial class GraphForm : Form
    {
        private PlotModel plotModel;
        private LineSeries networkQualitySeries;
        private System.Windows.Forms.Timer timer; // Используем стандартный Timer для Windows Forms
        private double time;
        private WifiNetwork selectedNetwork;
        private WifiModel wifiModel;
        private int graphSpeed;

        public GraphForm(WifiNetwork wifiNetwork, int speed)
        {
            InitializeComponent();
            selectedNetwork = wifiNetwork;
            graphSpeed = speed;
            wifiModel = new WifiModel(); // Создаём экземпляр WifiModel
            InitializePlot();
            InitializeTimer();
        }

        private void InitializePlot()
        {
            plotModel = new PlotModel
            {
                Title = $"Signal Quality Over Time: {selectedNetwork.SSID}",
                Background = OxyColors.WhiteSmoke,
                TextColor = OxyColors.DarkBlue,
                PlotAreaBorderColor = OxyColors.Black
            };

            networkQualitySeries = new LineSeries
            {
                Title = $"Signal Quality of {selectedNetwork.SSID}",
                MarkerType = MarkerType.Circle,
                LineStyle = LineStyle.Solid,
                Color = OxyColor.FromRgb(105, 133, 247)
            };

            plotModel.Series.Add(networkQualitySeries);

            // Создаём и добавляем PlotView в форму
            var plotView = new PlotView
            {
                Dock = DockStyle.Fill,
                Model = plotModel
            };
            Controls.Add(plotView);
        }

        private void InitializeTimer()
        {
            if (graphSpeed > 0)
            {
                timer = new System.Windows.Forms.Timer
                {
                    Interval = graphSpeed * 1000  // интервал в миллисекундах
                };
                timer.Tick += Timer_Tick;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Сканируем сети и обновляем качество сигнала для выбранной сети
            wifiModel.ScanNetworks();

            // Находим обновлённую информацию о выбранной сети
            var updatedNetwork = wifiModel.WifiNetworks.FirstOrDefault(n => n.SSID == selectedNetwork.SSID);

            if (updatedNetwork != null)
            {
                selectedNetwork.SignalQuality = updatedNetwork.SignalQuality; // Обновляем качество сигнала

                // Добавляем точку на график
                double signalQuality = Convert.ToDouble(selectedNetwork.SignalQuality.Replace("%", ""));
                UpdateGraph(time, signalQuality);
                time++;
            }
        }

        public void StartTimer()
        {
            time = 0;
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void UpdateGraph(double time, double signalQuality)
        {
            networkQualitySeries.Points.Add(new DataPoint(time, signalQuality));

            if (networkQualitySeries.Points.Count > 100)
            {
                networkQualitySeries.Points.RemoveAt(0);
            }

            plotModel.InvalidatePlot(true);
        }

        public void RefreshGraph()
        {
            Invalidate();
        }
    }
}
