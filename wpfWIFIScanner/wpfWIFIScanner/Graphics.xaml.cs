using System;
using System.Windows;
using System.Windows.Threading;
using MyLibrary;
using OxyPlot;
using OxyPlot.Series;

namespace wpfWIFIScanner
{
    public partial class Graphics : Window
    {
        private PlotModel plotModel;
        private LineSeries networkQualitySeries;
        private DispatcherTimer timer;
        private double time;
        private WifiNetwork selectedNetwork;
        private WifiModel wifiModel;
        private int GraphSpeed;

        public Graphics(WifiNetwork wifiNetwork, int speed)
        {
            InitializeComponent();
            selectedNetwork = wifiNetwork;
            GraphSpeed = speed;
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
            plotView.Model = plotModel;
        }

        private void InitializeTimer()
        {
            if (GraphSpeed != null || GraphSpeed > 0)
            {
                timer = new DispatcherTimer
                {

                    Interval = TimeSpan.FromSeconds(GraphSpeed)
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
            plotView.InvalidateVisual();
        }
    }
}
