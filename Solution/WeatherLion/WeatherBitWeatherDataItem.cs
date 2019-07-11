using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WeatherBitWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Weather Bit Weather Service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A WeatherBit weather data object.
    /// </summary>
    public class WeatherBitWeatherDataItem
    {
        public static SixteenDayForecastData wbFxWeatherData { get; set; }
        public static WeatherData wbWxWeatherData { get; set; }

        public WeatherData wx { get; set; }
        public FiveDayForecastData fx { get; set; }

        public WeatherBitWeatherDataItem()
        {
        }        

        public class WeatherData
        {
            public List<Data> data { get; set; }
            public int count { get; set; }           

            public class Data
            {
                public string wind_cdir { get; set; }
                public int rh { get; set; }
                public string pod { get; set; }
                public string lon { get; set; }
                public double pres { get; set; }
                public string timezone { get; set; }
                public string ob_time { get; set; }
                public string country_code { get; set; }
                public int clouds { get; set; }
                public double vis { get; set; }
                public double wind_spd { get; set; }
                public string wind_cdir_full { get; set; }
                public double appTemp { get; set; }
                public string state_code { get; set; }
                public long ts { get; set; }
                public double h_angle { get; set; }
                public double dewpt { get; set; }
                public Weather weather { get; set; }
                public double uv { get; set; }
                public string station { get; set; }
                public int windDir { get; set; }
                public int elevAngle { get; set; }
                public string datetime { get; set; }
                public string precip { get; set; }
                public double dhi { get; set; }
                public string cityName { get; set; }
                public string sunrise { get; set; }
                public string sunset { get; set; }
                public double temp { get; set; }
                public string lat { get; set; }
                public double slp { get; set; }
            }// end of class Data

            public class Weather
            {
                public string icon { get; set; }
                public string code { get; set; }
                public string description { get; set; }
            }// end of class Weather
        }// end of class WeatherData

        public class FiveDayForecastData
        {
            public List<Data> data { get; set; }
            public string city_name { get; set; }
            public string lon { get; set; }
            public string timezone { get; set; }
            public string lat { get; set; }
            public string country_code { get; set; }
            public string state_code { get; set; }
            
            public class Data
            {
                public string wind_cdir { get; set; }
                public double rh { get; set; }
                public double wind_spd { get; set; }
                public int pop { get; set; }
                public string wind_cdir_full { get; set; }
                public double app_temp { get; set; }
                public int snow6h { get; set; }
                public string pod { get; set; }
                public double dewpt { get; set; }
                public int snow { get; set; }
                public double uv { get; set; }
                public int ts { get; set; }
                public int wind_dir { get; set; }
                public Weather weather { get; set; }
                public int snow_depth { get; set; }
                public double dhi { get; set; }
                public string precip6h { get; set; }
                public string precip { get; set; }
                public double pres { get; set; }
                public string datetime { get; set; }
                public double temp { get; set; }
                public double slp { get; set; }
                public int clouds { get; set; }
                public double vis { get; set; }

                public class Weather
                {
                    public string icon { get; set; }
                    public string code { get; set; }
                    public string description { get; set; }
                }// end of class Weather
            }// end of class Data
        }// end of inner class FiveDayForecastData

        public class SixteenDayForecastData
        {
            public List<Data> data { get; set; }
            public string city_name { get; set; }
            public string lon { get; set; }
            public string timezone { get; set; }
            public string lat { get; set; }
            public string country_code { get; set; }
            public string state_code { get; set; }

            public class Data
            {
                public int moonrise_ts { get; set; }
                public string wind_cdir { get; set; }
                public int rh { get; set; }
                public double pres { get; set; }
                public int sunset_ts { get; set; }
                public double ozone { get; set; }
                public double moon_phase { get; set; }
                public double wind_gust_spd { get; set; }
                public int snow_depth { get; set; }
                public int clouds { get; set; }
                public int ts { get; set; }
                public int sunrise_ts { get; set; }
                public double app_min_temp { get; set; }
                public double wind_spd { get; set; }
                public int pop { get; set; }
                public string wind_cdir_full { get; set; }
                public double slp { get; set; }
                public double app_max_temp { get; set; }
                public double vis { get; set; }
                public double dewpt { get; set; }
                public int snow { get; set; }
                public double uv { get; set; }
                public string valid_date { get; set; }
                public int wind_dir { get; set; }
                //public double max_dhi { get; set; }
                public int clouds_hi { get; set; }
                public double precip { get; set; }
                public Weather weather { get; set; }
                public double max_temp { get; set; }
                public int moonset_ts { get; set; }
                public string datetime { get; set; }
                public double temp { get; set; }
                public double min_temp { get; set; }
                public int clouds_mid { get; set; }
                public int clouds_low { get; set; }              

                public class Weather
                {
                    public string icon { get; set; }
                    public string code { get; set; }
                    public string description { get; set; }
                }// end of class Weather
            }// end of class Data
        }// end of inner class SixteenDayForecastData

        public static bool DeserializeWeatherBitWeatherFxJSON(string strJSON, ref SixteenDayForecastData wbFxWeatherData)
        {
            wbFxWeatherData = JsonConvert.DeserializeObject<SixteenDayForecastData>(strJSON);

            if (wbFxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeOpenWeatherMapWxJSON

        public static bool DeserializeWeatherBitWxJSON(string strJSON, ref WeatherData wbWxWeatherData)
        {
            wbWxWeatherData = JsonConvert.DeserializeObject<WeatherData>(strJSON);

            if (wbWxWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeOpenWeatherMapFxJSON
   }// end of class WeatherBitWeatherDataItem
}// end of namespace WeatherLion
