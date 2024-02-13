using System.Windows;

namespace earthquake_finder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // sets the initial data to past hour
            Earthquake.GetEarthquakeData(Earthquake.Filter.Hour);

            InitializeComponent();
        }
    }
}