using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class ButtonPanel : UserControl
    {
        public ButtonPanel()
        {
            InitializeComponent();
            DataContext = Global.Instance;
        }

        private void btnHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Global.Instance.GetEarthquakeData(Global.Filter.Hour);
        }

        private void btnDay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Global.Instance.GetEarthquakeData(Global.Filter.Day);
        }

        private void btnWeek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Global.Instance.GetEarthquakeData(Global.Filter.Week);
        }

        private void btnMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Global.Instance.GetEarthquakeData(Global.Filter.Month);
        }
    }
}
