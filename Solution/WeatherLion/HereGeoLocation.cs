using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          HereGeoLocation
///   Description:    This class serves as an object model for data
///                   received from the Here Maps web service.
///   Author:         Paul O. Patterson     Date: May 21, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// This class serves as an object model for data received from the Here Maps web service.
    /// </summary>
    public class HereGeoLocation
    {
        public static HereGeoLocation cityGeographicalData;
        public Response response { get; set; }   

        //no-argument constructor
        public HereGeoLocation()
        {
        }// default constructor

        public class Response
        {
            public MetaInfo meta { get; set; }
            public View view { get; set; }           

            public class MetaInfo
            {
                public string timestamp { get; set; }               
            }// end of class MetaInfo

            public class View
            {
                public string _type { get; set; }
                public int viewID { get; set; }
                public List<Result> result { get; set; }             

                public class Result
                {
                    public int relevance { get; set; }
                    public string matchLevel { get; set; }
                    public MatchQuality matchQuality { get; set; }
                    public Location location { get; set; }                 

                    public class MatchQuality
                    {
                        public int state { get; set; }
                        public int district { get; set; }
                    }// end of class MatchQuality 

                    public class Location
                    {
                        public string locationID { get; set; }
                        public string locationType { get; set; }
                        public DisplayPosition displayPosition { get; set; }
                        public NavigationPosition navigationPosition { get; set; }
                        public MapView mapView { get; set; }
                        public Address address { get; set; }

                        public class DisplayPosition
                        {
                            public float latitude { get; set; }
                            public float longitude { get; set; }
                        }// end of class DisplayPosition

                        public class NavigationPosition
                        {
                            public float latitude { get; set; }
                            public float longitude { get; set; }
                        }// end of class NavigationPosition

                        public class MapView
                        {
                            public class TopLeft
                            {
                                public float latitude { get; set; }
                                public float longitude { get; set; }
                              }// end of class TopLeft

                            public class BottomLeft
                            {
                                public double latitude { get; set; }
                                public double longitude { get; set; }
                            }// end of class BottomLeft
                        }// end of function MapView

                        public class Address
                        {
                            public string label { get; set; }
                            public string country { get; set; }
                            public string state { get; set; }
                            public string county { get; set; }
                            public string city { get; set; }
                            public string district { get; set; }
                            public string postalCode { get; set; }
                            public List<AdditionalData> additionalData = null;

                            public class AdditionalData
                            {
                                public string key { get; set; }
                                public string value { get; set; }
                            }// end of class AdditionalData
                        }// end of class Address
                    }// end of class Location
                }// end of class Result

            }// end of class View

        }// end of class Response

        public static bool DeserializeHereGeoLocationJSON(string strJSON, ref HereGeoLocation cityGeographicalData)
        {
            cityGeographicalData = JsonConvert.DeserializeObject<HereGeoLocation>(strJSON);

            if (cityGeographicalData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block
        }// end of method DeserializeHereGeoLocationJSON
    }// end of class HereGeoLocation
}// end of namespace WeatherLion
