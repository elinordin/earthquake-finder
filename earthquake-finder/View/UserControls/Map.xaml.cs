using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();

            var mapControl = new Mapsui.UI.Wpf.MapControl();
            mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());

            Content = mapControl;
        }
    }
}
