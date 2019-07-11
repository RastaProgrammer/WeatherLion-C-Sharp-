using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          GeoNamesGeoLocation
///   Description:    This class serves as an object model for data
///                   received from the GeoNames web service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// This class serves as an object model for data received from the GeoNames web service.
    /// </summary>
    public class GeoNamesGeoLocation
    {
        public static GeoNamesGeoLocation cityGeographicalData = null;

        public int totalResultsCount { get; set; }
        public List<GeoNames> geonames { get; set; }       

        public class GeoNames
        {
            public string adminCode1 { get; set; }
            public string lng { get; set; }
            public long geonameId { get; set; }
            public string toponymName { get; set; }
            public string countryId { get; set; }
            public string fcl { get; set; }
            public long population { get; set; }
            public string countryCode { get; set; }
            public string name { get; set; }
            public string fclName { get; set; }
            public AdminCodes1 adminCodes1 { get; set; }
            public string countryName { get; set; }
            public string fcodeName { get; set; }
            public string adminName1 { get; set; }
            public string lat { get; set; }
            public string fcode { get; set; }         
  
            public class AdminCodes1
            {
                [JsonProperty("ISO3166_2")]
                public string iso { get; set; }
            }// end of class AdminCodes1
        }// end of class GeoNames
        
        public static bool DeserializeGeonamesJSON(string strJSON, ref GeoNamesGeoLocation cityGeographicalData)
        {
            cityGeographicalData = JsonConvert.DeserializeObject<GeoNamesGeoLocation>(strJSON);

            if (cityGeographicalData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeGeonamesJSON
    }// end of class GeoNamesGeoLocation
}// end of namespace WeatherLion
