using System.Windows;
using System.Windows.Threading;

namespace earthquake_finder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Global.Instance;

            FetchInitialEarthquakeData();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(RefreshEarthquakeData);
            timer.Interval = new TimeSpan(0, 5, 0); // hours, minutes, second
            timer.Start();
        }

        private async void FetchInitialEarthquakeData()
        {
            await Global.Instance.GetEarthquakeData(Global.Filter.Hour);
        }

        private async void RefreshEarthquakeData (object sender, EventArgs e)
        {
            await Global.Instance.GetEarthquakeData(Global.Instance.CurrentFilter);
        }
    }
}