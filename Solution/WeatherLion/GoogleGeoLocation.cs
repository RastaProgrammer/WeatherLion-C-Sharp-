using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          GoogleGeoLocation
///   Description:    This class serves as an object model for data
///                   received from the Google web service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// This class serves as an object model for data received from the Google web service.
    /// </summary>
    public class GoogleGeoLocation
    {
        public static GoogleGeoLocation cityGeographicalData;

        public List<Result> results { get; set; }
        public string status { get; set; }
        
        public class Result
        {
            private List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }            

            public class AddressComponent
            {
                public string long_name { get; set; }
                public string short_name { get; set; }
                public List<string> types { get; set; }
            }// end of class AddressComponent

            public class Geometry
            {
                public Bound bounds { get; set; }
                public Location location { get; set; }
                private string location_type { get; set; }
                private Viewport viewport { get; set; }              

                public class Bound
                {
                    public class NorthEast
                    {
                        public float lat { get; set; }
                        public float lng { get; set; }
                    }// end of class North East

                    public class SouthEast
                    {
                        public float lat { get; set; }
                        public float lng { get; set; }                       
                    }// end of class South East
                }// end of class Bound

                public class Location
                {
                    public float lat { get; set; }
                    public float lng { get; set; }
                }// end of class Location

                public class Viewport
                {
                    public class NorthEast
                    {
                        public float lat { get; set; }
                        public float lng { get; set; }                      
                    }// end of class North East

                    public class SouthEast
                    {
                        public float lat { get; set; }
                        public float lng { get; set; }
                    }// end of class South East
                }// end of class Viewport
            }// end of class Geometry
        }// end of class Result

        public static bool DeserializeGoogleGeoLocationJSON(string strJSON, ref GoogleGeoLocation cityGeographicalData)
        {
            cityGeographicalData = JsonConvert.DeserializeObject<GoogleGeoLocation>(strJSON);

            if (cityGeographicalData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeDarkSkyJSON
    }// end of class GoogleGeoLocation
}// end of namespace WeatherLion
