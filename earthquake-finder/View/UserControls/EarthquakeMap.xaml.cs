using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Limiting;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
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
            var pointsLayer = mapsuiMapControl.Map.Layers.FirstOrDefault(layer => layer.Name == "Points");

            if(pointsLayer != null)
            {
                mapsuiMapControl.Map.Layers.Remove(pointsLayer);
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
                Style = DefaultStyle(),
            };
        }

        private static List<IFeature> CreatePointFeatures()
        {
            var features = new List<IFeature>();

            foreach (Earthquake earthquake in Global.Instance.Earthquakes)
            {
                var point = SphericalMercator.FromLonLat(earthquake.Longitude, earthquake.Latitude).ToMPoint();

                var pointFeature = new PointFeature(point)
                { 
                    Styles = new List<IStyle>([CreatePointStyle(earthquake)])
                };

                features.Add(pointFeature);
            }

            return features;
        }

        private static SymbolStyle DefaultStyle()
        {
            return new SymbolStyle
            {
                SymbolScale = 0.5, // Custom styles are made on top of this one, so without setting it smaller there will be a white padding around the point
            };
        }

        private static SymbolStyle CreatePointStyle(Earthquake earthquake)
        {
            var darkGrey = new Color(55, 55, 55);
            var lightBlue = new Color(121, 164, 180);
            var lightYellow = new Color(242, 217, 131);
            var lightOrange = new Color(255, 182, 39);
            var orange = new Color(255, 122, 0);

            Color vectorFill;

            if (earthquake.Magnitude < 4.5) 
            {
                vectorFill = lightBlue; // small
            } else if (earthquake.Magnitude < 5.5) 
            {
                vectorFill = lightYellow; // medium
            } else if(earthquake.Magnitude < 6.5) 
            {
                vectorFill = lightOrange; // larger
            } else
            {
                vectorFill = orange; // large
            }

            return new SymbolStyle
            {
                SymbolScale = 0.5,
                SymbolOffset = new Offset(0, 0),
                Fill = new Brush(vectorFill),
                Outline = new Pen
                {
                    Color = darkGrey,
                    Width = 5
                }
            };
        }
    }
}
