using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class Loader : UserControl
    {
        public Loader()
        {
            InitializeComponent();
            DataContext = Global.Instance;
        }
    }
}
