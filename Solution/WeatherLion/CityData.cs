using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          CityData
///   Description:    This class is used as a model object for storing 
///                   details about a particular place or city.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A obejct representing geographical information about a place.
    /// </summary>
    public class CityData
    {
        public static CityData currentCityData;
        private const string TAG = "CityData";

        public string cityName { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string regionName { get; set; }
        public string regionCode { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }

        //no-argument constructor
        public CityData()
        {
        }// default constructor

        public CityData(string name, string country, float latitude, float longitude)
        {
            cityName = name;
            countryName = country;
            this.latitude = latitude;
            this.longitude = longitude;
        }// end of four-argument constructor

        public CityData(string name, string country, string countryCode, string regionCode, float latitude, float longitude)
        {
            cityName = name;
            countryName = country;
            this.countryCode = countryCode;
            this.regionCode = regionCode;
            this.latitude = latitude;
            this.longitude = longitude;
        }// end on six-argument constructor

        public CityData(string name, string country, string countryCode, string region,
                 string regionCode, float latitude, float longitude)
        {
            cityName = name;
            countryName = country;
            this.countryCode = countryCode;
            regionName = region;
            this.regionCode = regionCode;
            this.latitude = latitude;
            this.longitude = longitude;
        }// end on seven-argument constructor

        public static bool DeserializeCityJSON(string cityJSON, ref CityData cityData)
        {
            cityData = JsonConvert.DeserializeObject<CityData>(cityJSON);

            if (cityData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block            
        }// end of method DeserializeCityJSON       
    }// end of class CityData
}// end of namespace WeatherLion
