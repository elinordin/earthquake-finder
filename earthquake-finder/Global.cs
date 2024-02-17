using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace earthquake_finder
{
    class Earthquake
    {
        public Earthquake(long time, double longitude, double latitude, float? magnitude )

        {
            Magnitude = magnitude;
            Longitude = longitude;
            Latitude = latitude;
            Time = time;
        }
        public long Time { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public float? Magnitude { get; set; }
    }

    class Global : INotifyPropertyChanged
    {
        private static Global instance;

        public static Global Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Global();
                }
                return instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public enum Filter
        {
            Hour,
            Day,
            Week,
            Month
        }

        private Filter currentFilter;

        public Filter CurrentFilter
        {
            get { return currentFilter; }
            set
            {
                currentFilter = value;
                OnPropertyChanged();
            }
        }

        private List<Earthquake> earthquakes = new List<Earthquake>();

        public List<Earthquake> Earthquakes
        {
            get { return earthquakes; }
            set
            {
                earthquakes = value;
                OnPropertyChanged();
            }
        }

        private string updatedAt;

        public string UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                updatedAt = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public async void GetEarthquakeData(Filter filter)
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
                                    float? magnitude;

                                    object mag = item["properties"]["mag"];
                                    

                                    if (float.TryParse(mag.ToString(), out float result))
                                    {
                                        magnitude = result;
                                    }
                                    else
                                    {
                                        magnitude = null;
                                    }
 



                                    Earthquake earthquake = new Earthquake(time, longitude, latitude, magnitude);

                                    localEarthquakes.Add(earthquake);
                                }

                                Earthquakes = localEarthquakes;
                                CurrentFilter = filter;
                                UpdatedAt = DateTime.Now.ToString();
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

        private string getFilterQuery(Filter filter)
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
