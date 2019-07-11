using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          OpenWeatherMapWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Open Weather Map Weather Service.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// An Open Weather Map weather data object.
    /// </summary>
    public class OpenWeatherMapWeatherDataItem
    {
        public static string searchLocation;
        public static string searchCountryName;

        public WeatherData wx { get; set; }
        public ForecastData fx { get; set; }

        public class WeatherData
        {
            #region Current Weather  Data

            public Coordinate coord { get; set; }
            public List<Weather> weather { get; set; }
            public string _base { get; set; }
            public Main main { get; set; }
            public Wind wind { get; set; }
            public Cloud clouds { get; set; }
            public long dt { get; set; }
            public System sys { get; set; }
            public long id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }

            public class Coordinate
            {
                public float lon { get; set; }
                public float lat { get; set; }
            }// end of class 

            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }// end of class       

            public class Main
            {
                public float temp { get; set; }
                public float pressure { get; set; }
                public float humidity { get; set; }
                public float temp_min { get; set; }
                public float temp_max { get; set; }
                public float sea_level { get; set; }
                public float grnd_level { get; set; }
            }// end of class

            public class Wind
            {
                public float speed { get; set; }
                public float deg { get; set; }
            }// end of class

            public class Cloud
            {
                public int all { get; set; }
            }// end of class

            public class System
            {
                public float message { get; set; }
                public string country { get; set; }
                public long sunrise { get; set; }
                public long sunset { get; set; }
            }// end of class

            #endregion
        }// end of inner class WeatherData

        public class ForecastData
        {
            #region 5 day Forecast Weather  Data

            public City city { get; set; }
            public int cod { get; set; }
            public string message { get; set; }
            public int cnt { get; set; }
            public List<Data> list { get; set; }

            public class City
            {
                public long id { get; set; }
                public string name { get; set; }
                public Coordinate coord { get; set; }
                public string country { get; set; }
                public string population { get; set; }

                public class Coordinate
                {
                    public float lon { get; set; }
                    public float lat { get; set; }
                }// end of class 
            }// end of class City

            public class Data
            {
                public long dt { get; set; }
                public Temperature temp { get; set; }
                public List<Weather> weather { get; set; }
                public float speed { get; set; }
                public int deg { get; set; }
                public int clouds { get; set; }
                public float rain { get; set; }

                public class Temperature
                {
                    public float day { get; set; }
                    public float min { get; set; }
                    public float max { get; set; }
                    public float night { get; set; }
                    public float eve { get; set; }
                    public float morn { get; set; }
                }// end of class Temperature

                public class Weather
                {
                    public int id { get; set; }
                    public string main { get; set; }
                    public string description { get; set; }
                    public string icon { get; set; }
                }// end of class                
            }// end of class Data
            #endregion
        }// end of inner class ForecastData

        public static bool DeserializeOpenWeatherMapWxJSON(string strJSON, ref WeatherData owmWxWeatherData)
        {
            owmWxWeatherData = JsonConvert.DeserializeObject<WeatherData>(strJSON);

            if (owmWxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeOpenWeatherMapWxJSON

        public static bool DeserializeOpenWeatherMapFxJSON(string strJSON, ref ForecastData owmFxWeatherData)
        {
            owmFxWeatherData = JsonConvert.DeserializeObject<ForecastData>(strJSON);

            if (owmFxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeOpenWeatherMapFxJSON
    }// end of class OpenWeatherMapWeatherDataItem
}// end of namespace WeatherLion
