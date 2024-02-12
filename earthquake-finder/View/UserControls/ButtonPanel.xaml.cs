using System.Diagnostics;
using System.Windows.Controls;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace earthquake_finder.View.UserControls
{
    public partial class ButtonPanel : UserControl
    {
        public ButtonPanel()
        {
            InitializeComponent();
        }


        private void btnHour_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetEarthquakeData("all_hour.geojson");
        }

        private void btnDay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetEarthquakeData("all_day.geojson");
        }

        private void btnWeek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetEarthquakeData("all_week.geojson");
        }

        private void btnMonth_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetEarthquakeData("all_month.geojson");
        }

        private static async void GetEarthquakeData(string filter)
        {
            string apiUrl = $"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/{filter}";

            try {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(apiUrl))
                {
                    using(HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                var parsedData = JObject.Parse(data)["features"];

                                List<Earthquake> earthquakes = new List<Earthquake>();

                                foreach (var item in parsedData)
                                {
                                    long time = (long)item["properties"]["time"];
                                    double longitude = (double)item["geometry"]["coordinates"][0];
                                    double latitude = (double)item["geometry"]["coordinates"][1];
                                    float magnitude = (float)item["properties"]["mag"];

                                    Earthquake earthquake = new Earthquake(time, longitude, latitude, magnitude);

                                    earthquakes.Add(earthquake);
                                }

                                Trace.WriteLine(earthquakes.Count);
                            }
                            else
                            {
                                Trace.WriteLine("Empty response");
                            }
                        }
                }
            }
            } catch(Exception exception)
            {
                Trace.WriteLine("Error calling api");
                Trace.WriteLine(exception);
            }
        }

        private class Earthquake
        {
            public Earthquake(long time , double longitude, double latitude, float magnitude)
            {
                Magnitude = magnitude;
                Longitude = longitude;
                Latitude = latitude;
                Time = time;
            }
            public long Time { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public float Magnitude { get; set; }
        }
    }
}
