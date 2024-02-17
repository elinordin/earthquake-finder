using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Limiting;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using NetTopologySuite.Geometries;
using System.ComponentModel;
using System.Windows.Controls;

namespace earthquake_finder.View.UserControls
{
    public partial class EarthquakeMap : UserControl
    {
        public EarthquakeMap()
        {
            InitializeComponent();
            DataContext = Global.Instance;
            CreateMapAsync();

            Global.Instance.PropertyChanged += HandleGlobalPropertyChange;
        }

        private void HandleGlobalPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var polygonsLayer = mapsuiMapControl.Map.Layers.FirstOrDefault(layer => layer.Name == "Polygons");

            if(polygonsLayer != null)
            {
                mapsuiMapControl.Map.Layers.Remove(polygonsLayer);
            }

            mapsuiMapControl.Map.Layers.Add(CreatePointsLayer());
        }

        private Task<Map> CreateMapAsync()
        {
            var map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer()); // Base layer
            map.Layers.Add(CreatePointsLayer());
            mapsuiMapControl.Map = map;
            mapsuiMapControl.Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
            return Task.FromResult(map);
        }

        private static ILayer CreatePointsLayer()
        {
            return new MemoryLayer
            {
                Name = "Points",
                Features = CreatePointFeatures(),
                Style = CreatePointStyle()
            };
        }

        private static List<IFeature> CreatePointFeatures()
        {
            var features = new List<IFeature>();

            foreach (Earthquake earthquake in Global.Instance.Earthquakes)
            {
                var pointFeature = new PointFeature(SphericalMercator.FromLonLat(earthquake.Longitude, earthquake.Latitude).ToMPoint());
                features.Add(pointFeature);
            }

            return features;
        }

        private static IStyle CreatePointStyle()
        {
            var lightBlue = new Color(80, 83, 118);
            var orange = new Color(255, 182, 39);

            return new SymbolStyle
            {
                SymbolScale = 0.5,
                SymbolOffset = new Offset(0, 0),
                Fill = new Brush(orange), 
                Outline = new Pen
                {
                    Color = lightBlue,
                    Width = 5
                }
            };
        }
    }
}
