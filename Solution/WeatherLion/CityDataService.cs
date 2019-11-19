using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          CityDataService
///   Description:    This class is used as a background service for 
///                   retrieving data about a local from a webservice
///                   to local storage.
///   Author:         Paul O. Patterson     Date: May 13, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A model for storing data regarding a specific world location.
    /// </summary>
    public class CityDataService
    {
        private Thread thread;

        private string m_response;
        private string m_url;
        private string m_service;
        private static string strJSON { get; set; }
        private static string cityName { get; set; }
        private static string countryName { get; set; }
        private static string countryCode { get; set; }
        private static string regionName { get; set; }
        private static string regionCode { get; set; }
        private static string Latitude { get; set; }
        private static string Longitude { get; set; }       

        delegate void AccessWidget(PreferencesForm f);
        public static PreferencesForm frmPreferences = null;

        public static string[] matchList;

        private const string TAG = "CityDataService";

        private const String QUERY_COMMAND = "name_equals";
	    private String dataURL = null;
        private String response;

        public CityDataService(string url, string service)
        {
            m_url = url;
            m_service = service;
        }// end of default constructor

        public CityDataService(string url, string service, PreferencesForm activeForm)
        {
            m_url = url;
            m_service = service;
            frmPreferences = activeForm;
        }// end of three-argument constructor

        public string GetResponse()
        {
            return m_response;
        }

        public void SetResponse(string response)
        {
            m_response = response;
        }

        public string GetService()
        {
            return m_service;
        }

        public void SetService(string service)
        {
            m_service = service;
        }

        public string GetUrl()
        {
            return m_url;
        }

        public void SetUrl(string url)
        {
            m_url = url;
        }        

        /// <summary>
        /// Retrieve the data from the weather web service.
        /// </summary>
        public void GetCityData()
        {
            string previousSearchesPath = $@"{AppDomain.CurrentDomain.BaseDirectory}res\storage\previous_searches\";
            string previousCitySearchFile = null;

            if (dataURL != null)
            {
                if (dataURL.Contains("geonames"))
                {
                    SetService("geo");
                }// end of if block'
                else if (dataURL.Contains("api.here"))
                {
                    SetService("here");
                }// end of else block
            }// end of if block

            switch (GetService())
            {
                case "geo":
                    string cityName = null;
                    string currentLocation = null;

                    if (UtilityMethod.HasInternetConnection())
                    {
                        // the means that we only have GPS coordinates
                        if (dataURL.Contains("findNearbyPlaceNameJSON"))
                        {
                            response = HttpHelper.DownloadUrl(dataURL);

                            CityData currentCityData = null;
                            List<dynamic> cityDataList = 
                                JsonConvert.DeserializeObject<List<dynamic>>(response);

                            if (cityDataList != null)
                            {
                                cityName = cityDataList[0]["name"].ToString();
                                string countryName = cityDataList[0]["countryName"].ToString();
                                string countryCode = cityDataList[0]["countryCode"].ToString();
                                string localCityName = cityDataList[0]["toponymName"].ToString();
                                string regionCode = cityDataList[0]["adminCode1"].ToString();
                                string regionName = cityDataList[0]["countryCode"].ToString().Equals("US") ?
                                                UtilityMethod.usStatesByCode[regionCode].ToString() :
                                                null;
                                float latitude = float.Parse(cityDataList[0]["lat"].ToString());
                                float longitude = float.Parse(cityDataList[0]["lng"].ToString());

                                currentCityData = new CityData
                                {
                                    cityName = cityName,
                                    countryName = countryName,
                                    countryCode = countryCode,
                                    regionCode = regionCode,
                                    regionName = regionName,
                                    latitude = latitude,
                                    longitude = longitude
                                };

                                if (regionName != null)
                                {
                                    currentLocation = cityName + ", " + regionName + ", "
                                            + countryName;
                                }// end of if block
                                else
                                {
                                    currentLocation = cityName + ", " + countryName;
                                }// end of else block
                            }// end of if block
                            else
                            {
                                // this means that the user entered a city manually
                                currentLocation = response;
                            }// end of else block                               

                            cityName = currentLocation;
                        }// end of if block                                          
                        else // the URL contains the city name which can be extracted
                        {
                            int start = dataURL.IndexOf(QUERY_COMMAND) + QUERY_COMMAND.Length + 1;
                            int end = dataURL.IndexOf("&");

                            try
                            {
                                cityName = Uri.UnescapeDataString(GetUrl().Substring(start, end).ToLower());
                                cityName = cityName.ReplaceAll("\\W", " ");
                            }// end of try block
                            catch (UriFormatException e)
                            {
                                UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                                        TAG + "::handleWeatherData");
                            }// end of else block

                        }// end of else block

                        // just the city name is required and nothing else
                        if (cityName != null && cityName.Contains(","))
                        {
                            cityName = cityName.Substring(0, cityName.IndexOf(",")).ToLower();
                        }// end of if block // end of if block
                                                
                        StringBuilder fileData = new StringBuilder();

                        if (cityName != null)
                        {
                            previousCitySearchFile = $@"{previousSearchesPath}gn_sd_{cityName.ReplaceAll(" ", "_")}.json";
                        }// end of if block

                        if (File.Exists(previousCitySearchFile))
                        {
                            // use the file from the local storage
                            strJSON =
                                   File.ReadAllText($"{previousSearchesPath}gn_sd_{cityName.ReplaceAll(" ", "_")}.json");
                        }// end of if block
                        else
                        {
                            SaveGeoNamesSearchResults(previousCitySearchFile, cityName);
                        }// end of else block
                    }// end of if block

                    break;
                case "here":
                    // I prefer to use the GeoNames search results as the Here results only returns a single city.
                    // I might just add if in the future though.
                    break;
                default:
                    break;
            }// end of switch block          
            
            ProcessCityData();
        }// end of method GetCityData

        private void SaveGeoNamesSearchResults(string previousCitySearchFile, string cityName)
        {
            string searchURL;

            if (UtilityMethod.HasInternetConnection())
            {
                // now that we have the name of the city, we need some search results from GeoNames
                try
                {
                    int maxRows = 100;

                    // All spaces must be replaced with the + symbols for the HERE Maps web service
                    if (cityName.Contains(" "))
                    {
                        cityName = cityName.Replace(" ", "+");
                    }// end of if block

                    searchURL = string.Format(
                        "http://api.geonames.org/searchJSON?{0}={1}&maxRows={2}&username={3}",
                            QUERY_COMMAND,
                            cityName.ToLower(),
                            maxRows,
                            WidgetUpdateService.geoNameAccount);

                    response = HttpHelper.DownloadUrl(searchURL); // get the search results from the GeoNames web service
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                            TAG + "::handleWeatherData [line: " +
                                    UtilityMethod.GetExceptionLineNumber(e) + "]");

                    response = null;
                }// end of catch block

                // attempt to store the search results locally if this search was not performed before
                if (!File.Exists(previousCitySearchFile))
                {
                    if (response != null)
                    {
                        try
                        {
                            if (JSONHelper.SaveToJSONFile(response, previousCitySearchFile))
                            {
                                UtilityMethod.LogMessage(UtilityMethod.LogLevel.INFO,
                                    "JSON search data stored locally for " + cityName + ".",
                                        TAG + "::SaveGeoNamesSearchResults");
                            }// end of if block
                        }// end of try block
                        catch (Exception e)
                        {
                            UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                                TAG + "::handleWeatherData [line: " +
                                    UtilityMethod.GetExceptionLineNumber(e) + "]");
                        }// end of catch block
                    }// end of if block
                }// end of if block
            }// end of if block
        }// end of method saveGeoNamesSearchResults

        /// <summary>
        /// Process the data received from the web service.
        /// </summary>
        public void ProcessCityData()
        {
            switch (GetService())
            {
                case "geo":
                    GetGeoNamesSuggestions();
                    break;
                case "here":
                    GetHereSuggestions();
                    break;                
                default:
                    break;
            }// end of switch block
        }// end of method ProcessCityData

        /// <summary>
        /// Allows this <see cref="Thread"/> to access the widget controls running on another <see cref="Thread"/>.
        /// </summary>
        /// <param name="frmPreferences">The <see cref="Form"/> which it's controls require access from this <see cref="Thread"/>.</param>
        public static void AccessFormControlProperty(PreferencesForm frmPreferences)
        {
            frmPreferences.cboLocation.Focus();
            frmPreferences.cboLocation.Items.AddRange(matchList);

            frmPreferences.btnSearch.Image = null;
            frmPreferences.btnSearch.Text = "Search";
            frmPreferences.cboLocation.DropDownStyle = ComboBoxStyle.DropDown;
            frmPreferences.cboLocation.DroppedDown = true;
        }// end of method AccessFormControlsOnOtherThread

        /// <summary>
        /// Retrieves data from the Geo Names Web Service
        /// </summary>
        private void GetGeoNamesSuggestions()
        {
            List<string> matches = new List<string>();
            string localCityName = null;
            string response = null;

            cityName = null;
            countryName = null;
            countryCode = null;
            regionCode = null;
            regionName = null;

            if (strJSON != null)
            {
                if (UtilityMethod.IsValidJson(strJSON))
                {
                    GeoNamesGeoLocation.cityGeographicalData = JsonConvert.DeserializeObject<GeoNamesGeoLocation>(strJSON);
                    int matchCount = GeoNamesGeoLocation.cityGeographicalData.totalResultsCount;

                    if (matchCount == 1)
                    {
                        GeoNamesGeoLocation.GeoNames place = GeoNamesGeoLocation.cityGeographicalData.geonames[0];

                        cityName = place.name;
                        countryCode = place.countryCode;
                        countryName = place.countryName;
                        localCityName = place.toponymName;
                        regionCode = place.adminCode1;
                        regionName = place.countryCode.Equals("US") ?
                                     UtilityMethod.usStatesByCode[regionCode].ToString() :
                                     null;
                        Latitude = place.lat;
                        Longitude = place.lng;

                        if (regionName != null)
                        {
                            response = $"{cityName}, {regionName}, {countryName}";
                        }// end of if block
                        else
                        {
                            response = $"{cityName}, {countryName}";
                        }// end of else block
                    }// end of if block
                    else
                    {
                        foreach (GeoNamesGeoLocation.GeoNames place in GeoNamesGeoLocation.cityGeographicalData.geonames)
                        {
                            StringBuilder match = new StringBuilder();

                            // We only need the cities with the same name
                            if (!place.name.Equals(PreferencesForm.searchCity.ToString(),
                                StringComparison.OrdinalIgnoreCase)) continue;

                            match.Append(place.name);

                            // Geo Names may not return adminCodes1 for some results 
                            if (place.adminCodes1 != null)
                            {
                                if (place.adminCodes1.iso != null &&
                                        !UtilityMethod.IsNumeric(place.adminCodes1.iso))
                                {
                                    string region = place.adminCodes1.iso ?? null;

                                    match.Append(", " + region);
                                }// end of outer if block
                            }// end of if block

                            match.Append(", " + place.countryName);

                            // Always verify that the adminName1 and countryName does not indicate a city already added 
                            if (!matches.ToString().Contains($"{place.adminName1}, {place.countryName}"))
                            {
                                // Redundancy check
                                if (!matches.Contains(match.ToString()))
                                {
                                    matches.Add(match.ToString());
                                }// end of if block
                            }// end of if block
                        }// end of for each loop 
                    }// end of else block                                         
                }// end of if block 
            }// end of if block
            else
            {
                UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, "The web service returned invalid JSON data",
                    "CityDataService::GetGeoNamesSuggestions");
            }// end of else block

            // ensure that a service did not request this data
            if (frmPreferences != null)
            {
                if (matches.Count > 0)
                {
                    matchList = matches.ToArray();
                    PreferencesForm.cityNames = matchList;
                }// end of if block
                else
                {
                    matches.Clear();
                    matchList = new string[] { "No match found..." };
                    PreferencesForm.cityNames = matchList;
                }// end of else block

                // since it is known that invoke will be required, the if statement is not
                // necessary but, it is good for practice.
                if (frmPreferences.InvokeRequired)
                {
                    AccessWidget wid = new AccessWidget(AccessFormControlProperty);
                    frmPreferences.Invoke(wid, new object[] { frmPreferences });
                }// end of if block      
            }// end of if block           
        }// end of method GetGeoNamesSuggestions        

        /// <summary>
        /// Retrieves data from the Here Maps Web Service
        /// </summary>
        private void GetHereSuggestions()
        {
            string response = null;
            List<string> matches = new List<string>();

            if (strJSON != null)
            {
                if (UtilityMethod.IsValidJson(strJSON))
                {
                    HereGeoLocation.cityGeographicalData = JsonConvert.DeserializeObject<HereGeoLocation>(strJSON);
                    List<HereGeoLocation.Response.View.Result> places = HereGeoLocation.cityGeographicalData.response.view.result;
                    int matchCount = places.Count;

                    if (matchCount == 1)
                    {
                        HereGeoLocation.Response.View.Result place = places[0];
                        HereGeoLocation.Response.View.Result.Location location = place.location;
                        HereGeoLocation.Response.View.Result.Location.Address address = location.address;
                        HereGeoLocation.Response.View.Result.Location.Address.AdditionalData country = address.additionalData[0];
                        HereGeoLocation.Response.View.Result.Location.DisplayPosition displayPosition = location.displayPosition;
                        string label = place.location.address.label;

                        cityName = label.Substring(0, label.IndexOf(","));
                        countryCode = UtilityMethod.ToProperCase(country.value);
                        countryName = (string)UtilityMethod.worldCountryCodes[countryName];
                        regionName = null;
                        regionCode = null;
                        Latitude = displayPosition.latitude.ToString();
                        Longitude = displayPosition.longitude.ToString();

                        response = label;
                        matches.Add(response);
                    }// end of if block
                    else
                    {
                        foreach (HereGeoLocation.Response.View.Result place in places)
                        {
                            StringBuilder match = new StringBuilder();

                            // We only need the cities with the same name
                            if (!place.location.address.label.Equals(PreferencesForm.searchCity.ToString(),
                                StringComparison.OrdinalIgnoreCase)) continue;

                            string m = place.location.address.label;

                            if (!m.Contains(m.ToString()))
                            {
                                matches.Add(m.ToString());
                            }// end of if block
                        }// end of for each loop 
                    }// end of the else block
                }// end of if block
            }// end of if block
        }// end of method GetHereSuggestions

        /// <summary>
        /// Executes the <see cref="Thread"/>.
        /// </summary>
        public void Run()
        {
            thread = new Thread(GetCityData);
            thread.Start();
        }// end of method Run

    }// end of method CityDataService
}// end of namespace WeatherLion
