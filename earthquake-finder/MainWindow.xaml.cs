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

            // sets the initial data to past hour
            Global.Instance.GetEarthquakeData(Global.Filter.Hour);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(RefreshEarthquakeData);
            timer.Interval = new TimeSpan(0, 5, 0); // hours, minutes, second
            timer.Start();
        }

        private void RefreshEarthquakeData (object sender, EventArgs e)
        {
            Global.Instance.GetEarthquakeData(Global.Instance.CurrentFilter);
        }
    }
}