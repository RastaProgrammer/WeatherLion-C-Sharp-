using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          DarkSkyWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Dark Sky Weather Service.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Dark Sky weather data object.
    /// </summary>
    public class DarkSkyWeatherDataItem
    {
        private static readonly string[] compassSectors = { "N", "NNE", "NE", "ENE", "E", "ESE",
                                "SE", "SSE", "S", "SSW", "SW", "WSW",
                                "W", "WNW", "NW", "NNW" };
        public static string searchLocation;
        public static string searchCountryName;

        public float latitude { get; set; }
        public float longitude { get; set; }
        public string timezone { get; set; }
        public int offset { get; set; }
        public Currently currently { get; set; }
        public Minutely minutely { get; set; }
        public Hourly hourly { get; set; }
        public Daily daily { get; set; }
        public List<Alert> alerts { get; set; }
        public Flag flags { get; set; }

        public class Currently
        {
            public long time { get; set; }
            public string summary { get; set; }
            public string icon { get; set; }
            public int nearestStormDistance { get; set; }
            public float precipIntensity { get; set; }
            public float precipIntensityError { get; set; }
            public float precipProbability { get; set; }
            public string precipType { get; set; }
            public float temperature { get; set; }
            public float apparentTemperature { get; set; }
            public float dewPoint { get; set; }
            public float humidity { get; set; }
            public float windSpeed { get; set; }
            public float windGust { get; set; }
            public int windBearing { get; set; }
            public float visibility { get; set; }
            public float cloudCover { get; set; }
            public float pressure { get; set; }
            public float ozone { get; set; }
            public int uvIndex { get; set; }

        }// end of class Currently

        public class Minutely
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public List<Data> data { get; set; }

            public class Data
            {
                public long time { get; set; }
                public float precipIntensity { get; set; }
                public float precipIntensityError { get; set; }
                public string precipType { get; set; }
            }// end of class Data
        }// end of class Minutely

        public class Hourly
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public List<Data> data { get; set; }

            public class Data
            {
                public string time { get; set; }
                public string summary { get; set; }
                public string icon { get; set; }
                public float precipIntensity { get; set; }
                public float precipProbability { get; set; }
                public string precipType { get; set; }
                public float temperature { get; set; }
                public float apparentTemperature { get; set; }
                public float dewPoint { get; set; }
                public float humidity { get; set; }
                public float windSpeed { get; set; }
                public float windGust { get; set; }
                public int windBearing { get; set; }
                public float visibility { get; set; }
                public float cloudCover { get; set; }
                public float pressure { get; set; }
                public float ozone { get; set; }
                public int uvIndex { get; set; }
            }// end of class Data
        }// end of class Hourly

        public class Daily
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public List<Data> data { get; set; }

            public class Data
            {
                public long time { get; set; }
                public string summary { get; set; }
                public string icon { get; set; }
                public long sunriseTime { get; set; }
                public long sunsetTime { get; set; }
                public float moonPhase { get; set; }
                public float precipIntensity { get; set; }
                public float precipIntensityMax { get; set; }
                public long precipIntensityMaxTime { get; set; }
                public float precipProbability { get; set; }
                public string precipType { get; set; }
                public float temperatureMin { get; set; }
                public long temperatureMinTime { get; set; }
                public float temperatureMax { get; set; }
                public long temperatureMaxTime { get; set; }
                public float apparentTemperatureMin { get; set; }
                public long apparentTemperatureMinTime { get; set; }
                public float apparentTemperatureMax { get; set; }
                public long apparentTemperatureMaxTime { get; set; }
                public float dewPoint { get; set; }
                public float humidity { get; set; }
                public float windSpeed { get; set; }
                public int windBearing { get; set; }
                public float visibility { get; set; }
                public float cloudCover { get; set; }
                public float pressure { get; set; }
                public float ozone { get; set; }
                public int uvIndex { get; set; }
                public long uvIndexTime { get; set; }
            }// end of class Data
        }// end of class Daily

        public class Alert
        {
            public string title { get; set; }
            public long time { get; set; }
            public long expires { get; set; }
            public string description { get; set; }
            public string uri { get; set; }
        }// end of class Alert

        public class Flag
        {
            public List<string> sources { get; set; }
            public List<string> isd_stations { get; set; }
            public string units { get; set; }

        }// end of class Flag      

        public static bool DeserializeDarkSkyJSON(string strJSON, ref DarkSkyWeatherDataItem darkSkyWeatherData)
        {
            darkSkyWeatherData = JsonConvert.DeserializeObject<DarkSkyWeatherDataItem>(strJSON);

            if (darkSkyWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeDarkSkyJSON
    }// end of class DarkSkyWeatherDataItem
}// end of namespace WeatherLion
