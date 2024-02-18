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

        private async void btnHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Global.Instance.GetEarthquakeData(Global.Filter.Hour);
        }

        private async void btnDay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Global.Instance.GetEarthquakeData(Global.Filter.Day);
        }

        private async void btnWeek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Global.Instance.GetEarthquakeData(Global.Filter.Week);
        }

        private async void btnMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Global.Instance.GetEarthquakeData(Global.Filter.Month);
        }
    }
}
