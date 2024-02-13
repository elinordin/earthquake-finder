using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;

namespace earthquake_finder
{
    class Earthquake
    {
        // The current filter
        public enum Filter
        {
            Hour,
            Day,
            Week,
            Month
        }

        private static Filter currentFilter;

        public static Filter CurrentFilter
        {
            get { return currentFilter; }
            set
            {
                currentFilter = value;
            }
        }

        // The current list of earthquakes
        private static List<Earthquake> earthquakes = new List<Earthquake>();

        public static List<Earthquake> Earthquakes
        {
            get { return earthquakes; }
            set
            {
                earthquakes = value;
            }
        }

        public Earthquake(long time, double longitude, double latitude, float magnitude)
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

        public static async void GetEarthquakeData(Filter filter)
        {
            string filterQuery = getFilterQuery(filter);
            string apiUrl = $"https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/{filterQuery}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(apiUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                var parsedData = JObject.Parse(data)["features"];

                                List<Earthquake> localEarthquakes = new List<Earthquake>();

                                foreach (var item in parsedData)
                                {
                                    long time = (long)item["properties"]["time"];
                                    double longitude = (double)item["geometry"]["coordinates"][0];
                                    double latitude = (double)item["geometry"]["coordinates"][1];
                                    float magnitude = (float)item["properties"]["mag"];

                                    Earthquake earthquake = new Earthquake(time, longitude, latitude, magnitude);

                                    localEarthquakes.Add(earthquake);
                                }

                                Earthquakes = localEarthquakes;
                                CurrentFilter = filter;
                            }
                            else
                            {
                                Trace.WriteLine("Empty response");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Error calling api");
                Trace.WriteLine(exception);
            }
        }

        private static string getFilterQuery(Filter filter)
        {
            switch (filter)
            {
                case Filter.Hour:
                    return "all_hour.geojson";
                case Filter.Day:
                    return "all_day.geojson";
                case Filter.Week:
                    return "all_week.geojson";
                case Filter.Month:
                    return "all_month.geojson";
                default:
                    return "";
            }
        }
    }
}
