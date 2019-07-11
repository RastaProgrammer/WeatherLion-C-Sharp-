using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          YahooWeatherYdnDataItem
///   Description:    This class is a model class for data received
///                   from the Yahoo Weather Service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Yahoo! Weather weather data object.
    /// </summary>
    public class YahooWeatherYdnDataItem
    {
        public static YahooWeatherYdnDataItem yahooWeatherData;

        public Location location;
        public CurrentObservation current_observation;
        public List<Forecast> forecasts;

        public class Location
        {
            public int woeid { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public double lat { get; set; }
            [JsonProperty("long")]
            public double lon { get; set; }
            public string timezone_id { get; set; }
        }// end of class Location

        public class CurrentObservation
        {
            public Wind wind { get; set; }
            public Atmosphere atmosphere { get; set; }
            public Astronomy astronomy { get; set; }
            public Condition condition { get; set; }
            public long pubDate;           

            public class Wind
            {
                public double chill { get; set; }
                public int direction { get; set; }
                public double speed { get; set; }                
            }// end of class Wind

            public class Atmosphere
            {
                public double humidity { get; set; }
                public double visibility { get; set; }
                public double pressure { get; set; }                
            }// end of class  Atmosphere

            public class Astronomy
            {
                public string sunrise { get; set; }
                public string sunset { get; set; }
            }// end of class Astronomy

            public class Condition
            {
                public string text { get; set; }
                public int code { get; set; }
                public double temperature { get; set; }
            }// end of class Condition
        }// end of classCurrentObservation

        public class Forecast
        {
            public string day { get; set; }
            public long date { get; set; }
            public double low { get; set; }
            public double high { get; set; }
            public string text { get; set; }
            public int code { get; set; }            
        }// end of class Forecast

        public static bool DeserializeYahooJSON(string strJSON, ref YahooWeatherYdnDataItem yahooWeatherData)
        {
            yahooWeatherData = JsonConvert.DeserializeObject<YahooWeatherYdnDataItem>(strJSON);

            if (yahooWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeYahooJSON
    }// end of class YahooWeatherYdnDataItem
}// end of namespace WeatherLion
