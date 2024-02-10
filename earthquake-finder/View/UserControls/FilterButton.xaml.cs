using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class FilterButton : UserControl, INotifyPropertyChanged
    {
        public FilterButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string label;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Label
        {
            get { return label; }
            set { 
                label = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
