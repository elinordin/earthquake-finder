using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class ButtonPanel : UserControl
    {
        public ButtonPanel()
        {
            InitializeComponent();
        }

        private void btnHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData(Earthquake.Filter.Hour);
        }

        private void btnDay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData(Earthquake.Filter.Day);
        }

        private void btnWeek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData(Earthquake.Filter.Week);
        }

        private void btnMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData(Earthquake.Filter.Month);
        }
    }
}
