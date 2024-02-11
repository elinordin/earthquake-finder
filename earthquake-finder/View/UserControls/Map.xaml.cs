using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();

            mapsuiMapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        }

    }
}
