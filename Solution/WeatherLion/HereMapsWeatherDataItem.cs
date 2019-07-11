using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          HereMapsWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Here Maps Weather Service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Here Maps weather data object.
    /// </summary>
    public class HereMapsWeatherDataItem
    {
        public static HereMapsWeatherDataItem.ForecastData hereFxWeatherData { get; set; }
        public static HereMapsWeatherDataItem.WeatherData hereWxWeatherData { get; set; }
        public static HereMapsWeatherDataItem.AstronomyData hereAxData { get; set; }

        public class WeatherData
        {
            public Observations observations { get; set; }

            public class Observations
            {
                public List<Location> location { get; set; }              

                public class Location
                {
                    public List<Observation> observation { get; set; }
                    public string country { get; set; }
                    public string state { get; set; }
                    public string city { get; set; }
                    public float latitude { get; set; }
                    public float longitude { get; set; }
                    public float distance { get; set; }
                    public int timezone { get; set; }

                    public class Observation
                    {
                        public string daylight { get; set; }
                        public string description { get; set; }
                        public string skyInfo { get; set; }
                        public string skyDescription { get; set; }
                        public string temperature { get; set; }
                        public string temperatureDesc { get; set; }
                        public string comfort { get; set; }
                        public string highTemperature { get; set; }
                        public string lowTemperature { get; set; }
                        public string humidity { get; set; }
                        public string dewPoint { get; set; }
                        public string precipitation1H { get; set; }
                        public string precipitation3H { get; set; }
                        public string precipitation6H { get; set; }
                        public string precipitation12H { get; set; }
                        public string precipitation24H { get; set; }
                        public string precipitationDesc { get; set; }
                        public string airInfo { get; set; }
                        public string airDescription { get; set; }
                        public string windSpeed { get; set; }
                        public string windDirection { get; set; }
                        public string windDesc { get; set; }
                        public string windDescShort { get; set; }
                        public string barometerPressure { get; set; }
                        public string barometerTrend { get; set; }
                        public string visibility { get; set; }
                        public string snowCover { get; set; }
                        public string icon { get; set; }
                        public string iconName { get; set; }
                        public string iconLink { get; set; }
                        public string ageMinutes { get; set; }
                        public string activeAlerts { get; set; }
                        public string country { get; set; }
                        public string state { get; set; }
                        public string city { get; set; }
                        public float latitude { get; set; }
                        public float longitude { get; set; }
                        public string distance { get; set; }
                        public string elevation { get; set; }
                        public string utcTime { get; set; }                                             
                    }// end of class Observation
                }// end of class Location
            }// end of class Observations
        }// end of class WeatherData

        public class ForecastData
        {
            public DailyForecasts dailyForecasts { get; set; }           

            public class DailyForecasts
            {
                public ForecastLocation forecastLocation { get; set; }                

                public class ForecastLocation
                {
                    public List<Forecast> forecast { get; set; }                   

                    public class Forecast
                    {
                        public string daylight { get; set; }
                        public string description { get; set; }
                        public string skyInfo { get; set; }
                        public string skyDescription { get; set; }
                        public string temperatureDesc { get; set; }
                        public string comfort { get; set; }
                        public string highTemperature { get; set; }
                        public string lowTemperature { get; set; }
                        public string humidity { get; set; }
                        public string dewPoint { get; set; }
                        public string precipitationProbability { get; set; }
                        public string precipitationDesc { get; set; }
                        public string rainFall { get; set; }
                        public string snowFall { get; set; }
                        public string airInfo { get; set; }
                        public string airDescription { get; set; }
                        public string windSpeed { get; set; }
                        public string windDirection { get; set; }
                        public string windDesc { get; set; }
                        public string windDescShort { get; set; }
                        public string beaufortScale { get; set; }
                        public string beaufortDescription { get; set; }
                        public string uvIndex { get; set; }
                        public string uvDesc { get; set; }
                        public string barometerPressure { get; set; }
                        public string icon { get; set; }
                        public string iconName { get; set; }
                        public string iconLink { get; set; }
                        public string dayOfWeek { get; set; }
                        public string weekday { get; set; }
                        public string utcTime { get; set; }                     
                    }// end of class Forecast
                }// end of class ForecastLocation
            }// end of class DailyForecasts
        }// end of class ForecastData

        public class AstronomyData
        {
            public Astronomic astronomy { get; set; }           

            public class Astronomic
            {
                public List<Astronomy> astronomy { get; set; }
                public string country { get; set; }
                public string state { get; set; }
                public string city { get; set; }
                public float latitude { get; set; }
                public float longitude { get; set; }
                public int timezone { get; set; }
              
                public class Astronomy
                {
                    public string sunrise { get; set; }
                    public string sunset { get; set; }
                    public string moonrise { get; set; }
                    public string moonset { get; set; }
                    public string moonPhase { get; set; }
                    public string moonPhaseDesc { get; set; }
                    public string iconName { get; set; }
                    public string city { get; set; }
                    public string latitude { get; set; }
                    public string longitude { get; set; }
                    public string utcTime { get; set; }                  
                }// end of class AstronomyData
            }// end of class Astronomy
        }// end of class AstronomyData

        public static bool DeserializeHereWxJSON(string strJSON, ref HereMapsWeatherDataItem.WeatherData hereWxWeatherData)
        {
            hereWxWeatherData = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.WeatherData>(strJSON);

            if (hereWxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeHereWxJSON

        public static bool DeserializeHereAxJSON(string strJSON, ref HereMapsWeatherDataItem.AstronomyData hereAxWeatherData)
        {
            hereAxWeatherData = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.AstronomyData>(strJSON);

            if (hereWxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeHereAxJSON

        public static bool DeserializeHereFxJSON(string strJSON, ref HereMapsWeatherDataItem.ForecastData hereFxWeatherData)
        {
            hereFxWeatherData = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.ForecastData>(strJSON);

            if (hereFxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeHereFxJSON
    }// end of class HereMapsWeatherDataItem
}// end of namespace WeatherLion
