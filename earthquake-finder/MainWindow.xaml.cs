using System.Windows;

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
        }
    }
}