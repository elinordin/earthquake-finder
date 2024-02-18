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
    public class MapManager
    {
        public static Map CreateMapContent()
        {
            var map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer()); // Base layer
            map.Layers.Add(CreatePointsLayer());
            map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
            return map;
        }

        public static ILayer CreatePointsLayer()
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

            return new SymbolStyle
            {
                SymbolScale = 0.5,
                SymbolOffset = new Offset(0, 0),
                Fill = new Brush(GetColorFromMagnitude(earthquake.Magnitude)),
                Outline = new Pen
                {
                    Color = darkGrey,
                    Width = 5
                }
            };
        }

        private static Color GetColorFromMagnitude (float? magnitude)
        {
            var lightBlue = new Color(121, 164, 180);
            var lightYellow = new Color(242, 217, 131);
            var lightOrange = new Color(255, 182, 39);
            var orange = new Color(255, 122, 0);

            Color colorFill;

            if (magnitude < 4.5)
            {
                colorFill = lightBlue; // small
            }
            else if (magnitude < 5.5)
            {
                colorFill = lightYellow; // medium
            }
            else if (magnitude < 6.5)
            {
                colorFill = lightOrange; // larger
            }
            else
            {
                colorFill = orange; // large
            }

            return colorFill;
        }
    }

    public partial class EarthquakeMap : UserControl
    {
        private readonly Map mapContent;
        public EarthquakeMap()
        {
            InitializeComponent();
            DataContext = Global.Instance;

            mapContent = MapManager.CreateMapContent();
            mapsuiMapControl.Map = mapContent;

            Global.Instance.PropertyChanged += HandleGlobalPropertyChange;
        }

        private void HandleGlobalPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var pointsLayer = mapContent.Layers.FirstOrDefault(layer => layer.Name == "Points");

            if(pointsLayer != null)
            {
                mapContent.Layers.Remove(pointsLayer);
                mapContent.Layers.Add(MapManager.CreatePointsLayer());
            }      
        }
    }
}
