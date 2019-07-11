using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WeatherUndergroundWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Weather Underground Weather Service.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Weather Underground weather data object.
    /// </summary>
    [Obsolete("WeatherUndergroundWeatherDataItem is no longer useable as the free service has been discontinued.", true)]
    public class WeatherUndergroundWeatherDataItem
    {
        public Response response { get; set; }
        public Location location { get; set; }
        public CurrentObservation current_observation { get; set; }
        public Forecast forecast { get; set; }
        public SunPhase sun_phase { get; set; }

        public class Response
        {
            public float version { get; set; }
            public string termsofService { get; set; }
            public Feature features { get; set; }

            public class Feature
            {
                public int geolookup { get; set; }
                public int conditions { get; set; }
                public int forecast { get; set; }
            }// end of class Feature             
        }// end of class Response

        public class Location
        {
            public string type { get; set; }
            public string country { get; set; }
            public string country_iso3166 { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string tz_short { get; set; }
            public string tz_long { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public int zip { get; set; }
            public int magic { get; set; }
            public int wmo { get; set; }
            public string l { get; set; }
            public string requesturl { get; set; }
            public string wuiurl { get; set; }

            public class NearbyWeatherStations
            {
                public class Airport
                {
                    List<Station> station { get; set; }

                    public class Station
                    {
                        public string city { get; set; }
                        public string state { get; set; }
                        public string country { get; set; }
                        public string icao { get; set; }
                        public double lat { get; set; }
                        public double lon { get; set; }
                    }// end of class Station
                }// end of class Airport

                public class PWS
                {
                    List<Station> station { get; set; }

                    public class Station
                    {
                        public string city { get; set; }
                        public string state { get; set; }
                        public string country { get; set; }
                        public string id { get; set; }
                        public double lat { get; set; }
                        public double lon { get; set; }
                        public int distance_km { get; set; }
                        public int distance_mi { get; set; }
                    }// end of class Station
                }// end of class PWS
            }// end of method NearbyWeatherStations
        }// end of class Location

        public class CurrentObservation
        {
            public class Image
            {
                public string url { get; set; }
                public string title { get; set; }
                public string link { get; set; }
            }// end of class Image

            public class DisplayLocation
            {
                public string full { get; set; }
                public string city { get; set; }
                public string state { get; set; }
                public string state_name { get; set; }
                public string country { get; set; }
                public string country_iso3166 { get; set; }
                public int zip { get; set; }
                public int magic { get; set; }
                public int wmo { get; set; }
                public double latitude { get; set; }
                public double longitude { get; set; }
                public double elevation { get; set; }
            }// end of method DisplayLocation

            public class ObservationLocation
            {
                public string full { get; set; }
                public string city { get; set; }
                public string state { get; set; }
                public string country { get; set; }
                public string country_iso3166 { get; set; }
                public double latitude { get; set; }
                public double longitude { get; set; }
                public double elevation { get; set; }
            }// end of method ObservationLocation

            public class Estimated
            {

            }// end of method Estimated

            public string station_id { get; set; }
            public string observation_time { get; set; }
            public string observation_time_rfc822 { get; set; }
            public string observation_epoch { get; set; }
            public string local_time_rfc822 { get; set; }
            public string local_epoch { get; set; }
            public string local_tz_short { get; set; }
            public string local_tz_long { get; set; }
            public string local_tz_offset { get; set; }
            public string weather { get; set; }
            public string temperature_string { get; set; }
            public string temp_f { get; set; }
            public string temp_c { get; set; }
            public string relative_humidity { get; set; }
            public string wind_string { get; set; }
            public string wind_dir { get; set; }
            public string wind_degrees { get; set; }
            public float wind_mph { get; set; }
            public float wind_gust_mph { get; set; }
            public float wind_kph { get; set; }
            public float wind_gust_kph { get; set; }
            public int pressure_mb { get; set; }
            public float pressure_in { get; set; }
            public string pressure_trend { get; set; }
            public string dewpoint_string { get; set; }
            public int dewpoint_f { get; set; }
            public int dewpoint_c { get; set; }
            public string heat_index_string { get; set; }
            public string heat_index_f { get; set; }
            public string heat_index_c { get; set; }
            public string windchill_string { get; set; }
            public string windchill_f { get; set; }
            public string windchill_c { get; set; }
            public string feelslike_string { get; set; }
            public float feelslike_f { get; set; }
            public float feelslike_c { get; set; }
            public string visibility_mi { get; set; }
            public string visibility_km { get; set; }
            public string solarradiation { get; set; }
            public string UV { get; set; }
            public string precip_1hr_in { get; set; }
            public string precip_1hr_metric { get; set; }
            public string precip_today_string { get; set; }
            public string precip_today_in { get; set; }
            public string precip_today_metric { get; set; }
            public string icon { get; set; }
            public string icon_url { get; set; }
            public string forecast_url { get; set; }
            public string history_url { get; set; }
            public string ob_url { get; set; }
            public string nowcast { get; set; }
        }// end of class CurrentObservation 

        public class Forecast
        {
            public TxtForecast txt_forecast { get; set; }
            public SimpleForecast simpleforecast { get; set; }

            public class TxtForecast
            {
                public string date { get; set; }
                public List<ForeCastDay> forecastday { get; set; }

                public class ForeCastDay
                {
                    public int period { get; set; }
                    public string icon { get; set; }
                    public string icon_url { get; set; }
                    public string title { get; set; }
                    public string fcttext { get; set; }
                    public string fcttext_metric { get; set; }
                    public int pop { get; set; }
                }// end of class ForeCastDay
            }// end of class TxtForecast

            public class SimpleForecast
            {
                public List<ForeCastDay> forecastday { get; set; }

                public class ForeCastDay
                {
                    public ForcastDate date { get; set; }
                    public High high { get; set; }
                    public Low low { get; set; }

                    public class ForcastDate
                    {
                        public string epoch { get; set; }
                        public string pretty { get; set; }
                        public int day { get; set; }
                        public int month { get; set; }
                        public int year { get; set; }
                        public int yday { get; set; }
                        public int hour { get; set; }
                        public int min { get; set; }
                        public int sec { get; set; }
                        public int isdst { get; set; }
                        public string monthname { get; set; }
                        public string monthname_short { get; set; }
                        public string weekday_short { get; set; }
                        public string weekday { get; set; }
                        public string ampm { get; set; }
                        public string tz_short { get; set; }
                        public string tz_long { get; set; }
                    }// end of class ForcastDate

                    public int period { get; set; }

                    public class High
                    {
                        public int fahrenheit { get; set; }
                        public int celsius { get; set; }
                    }// end of class High

                    public class Low
                    {
                        public int fahrenheit { get; set; }
                        public int celsius { get; set; }
                    }// end of class Low

                    public string conditions { get; set; }
                    public string icon { get; set; }
                    public string icon_url { get; set; }
                    public string skyicon { get; set; }
                    public int pop { get; set; }
                    public QpfAllDay qpf_allday { get; set; }
                    public QpfDay qpf_day { get; set; }
                    public QpfNight qpf_night { get; set; }
                    public SnowAllDay snow_allday { get; set; }
                    public SnowDay snow_day { get; set; }
                    public SnowNight snow_night { get; set; }
                    public MaxWind maxwind { get; set; }
                    public AveWind avewind { get; set; }
                    public int avehumidity { get; set; }
                    public int maxhumidity { get; set; }
                    public int minhumidity { get; set; }

                    public class QpfAllDay
                    {
                        public string inch { get; set; }
                        public string mm { get; set; }
                    }// end of class QpfAllDay

                    public class QpfDay
                    {
                        public string inch { get; set; }
                        public string mm { get; set; }
                    }// end of class QpfDay

                    public class QpfNight
                    {
                        public string inch { get; set; }
                        public string mm { get; set; }
                    }// end of class QpfNight

                    public class SnowAllDay
                    {
                        public string inch { get; set; }
                        public string cm { get; set; }
                    }// end of class SnowAllDay

                    public class SnowDay
                    {
                        public string inch { get; set; }
                        public string cm { get; set; }
                    }// end of class SnowDay

                    public class SnowNight
                    {
                        public string inch { get; set; }
                        public string cm { get; set; }
                    }// end of class SnowNight

                    public class MaxWind
                    {
                        public int mph { get; set; }
                        public int kph { get; set; }
                        public string dir { get; set; }
                        public int degrees { get; set; }
                    }// end of class MaxWind

                    public class AveWind
                    {
                        public int mph { get; set; }
                        public int kph { get; set; }
                        public string dir { get; set; }
                        public int degrees { get; set; }
                    }// end of class AveWind
                }// end of method ForeCastDay              
            }// end of class SimpleForecast                          
        }// end of class Forecast

        public class SunPhase
        {
            public SunRise sunrise { get; set; }
            public SunSet sunset { get; set; }

            public class SunRise
            {
                public int hour { get; set; }
                public int minute { get; set; }
            }// end of class SunRise

            public class SunSet
            {
                public int hour { get; set; }
                public int minute { get; set; }
            }// end of class SunSet
        }// end of class SunPhase

        /// <summary>
        /// Converts JSON data to a class object.
        /// </summary>
        /// <param name="strJSON">The JSON data received from the weather service provided.</param>
        /// <param name="wuWeatherData">The reference to the <code>WeatherUndergroundWeatherDataItem</code> object.</param>
        /// <returns>True/False based on a successful conversion.</returns>
        public static bool DeserializeWuJSON(string strJSON, ref WeatherUndergroundWeatherDataItem wuWeatherData)
        {
            wuWeatherData = JsonConvert.DeserializeObject<WeatherUndergroundWeatherDataItem>(strJSON);

            if (wuWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeWuJSON
    }// end of class WeatherUndergroundWeatherDataItem
}// end of namespace WeatherLion
