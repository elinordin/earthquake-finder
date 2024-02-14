using Mapsui;
using Mapsui.Layers;
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
            SetMapBounderies();

            Global.Instance.PropertyChanged += HandleGlobalPropertyChange;
        }

        private void HandleGlobalPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var polygonsLayer = mapsuiMapControl.Map.Layers.FirstOrDefault(layer => layer.Name == "Polygons");

            if(polygonsLayer != null)
            {
                mapsuiMapControl.Map.Layers.Remove(polygonsLayer);
            }

            mapsuiMapControl.Map.Layers.Add(CreateLayer());
        }

        private void SetMapBounderies()
        {
            // These settings gets rid of the white border, however the pan bounds now restrict being able to zoom outside of EU (bug that needs fixing)
            var defaultMin = 0.14929107086948307;
            mapsuiMapControl.Map.Navigator.OverrideZoomBounds = new MMinMax(defaultMin, 26000.0);

            var leftAndRight = 100000.0;
            var topAndBottom = 11070000.0;

            mapsuiMapControl.Map.Navigator.OverridePanBounds = new MRect(-leftAndRight, -topAndBottom, leftAndRight, topAndBottom);
        }
        
        private Task<Map> CreateMapAsync()
        {
            var map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer()); // Base layer
            map.Layers.Add(CreateLayer());
            mapsuiMapControl.Map = map;
            return Task.FromResult(map);
        }

        private static ILayer CreateLayer()
        {
            return new Layer("Polygons")
            {
                DataSource = new MemoryProvider(CreatePolygon().ToFeatures()),
                Style = new VectorStyle
                {
                    Fill = new Brush(new Color(66, 69, 99, 150)),
                    Outline = new Pen
                    {
                        Color = new Color(255, 182, 39, 150),
                        Width = 2,
                        PenStyle = PenStyle.Solid,
                        PenStrokeCap = PenStrokeCap.Round
                    }
                }
            };
        }

        private static List<Polygon> CreatePolygon()
        {
            var result = new List<Polygon>();

            foreach(Earthquake earthquake in Global.Instance.Earthquakes) {
                var center = SphericalMercator.FromLonLat(earthquake.Longitude, earthquake.Latitude);
                var offset = 200000;

                var square = new Polygon(
                    new LinearRing(new[] {
                        new Coordinate(center.x + offset, center.y + offset), // top right
                        new Coordinate(center.x - offset, center.y + offset), // top left
                        new Coordinate(center.x - offset, center.y - offset), // bottom left
                        new Coordinate(center.x + offset, center.y - offset), // bottom right
                        new Coordinate(center.x + offset, center.y + offset) // top right
                    })
                );

                result.Add(square);
            }

            return result;
        }
    }
}
