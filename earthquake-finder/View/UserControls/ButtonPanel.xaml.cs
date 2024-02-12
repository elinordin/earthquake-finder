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
            Earthquake.GetEarthquakeData("all_hour.geojson");
            Earthquake.CurrentFilter = Earthquake.Filter.Hour;
        }

        private void btnDay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData("all_day.geojson");
            Earthquake.CurrentFilter = Earthquake.Filter.Day;
        }

        private void btnWeek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData("all_week.geojson");
            Earthquake.CurrentFilter = Earthquake.Filter.Week;
        }

        private void btnMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Earthquake.GetEarthquakeData("all_month.geojson");
            Earthquake.CurrentFilter = Earthquake.Filter.Month;
        }
    }
}
