using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WidgetUpdateService
///   Description:    This class is responsible updating the widget
///                   with data provided by weather web services.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    ///  This class is responsible updating the widget with data provided by weather web services.
    /// </summary>
    public class WidgetUpdateService
    {
        #region Field Declarations

        private Thread thread;
        private const string TAG = "WidgetUpdateService";

        private static readonly string WEATHER_IMAGE_PATH_PREFIX =
            $@"{AppDomain.CurrentDomain.BaseDirectory}res\assets\img\weather_images\";

        // weather providers
        private static DarkSkyWeatherDataItem darkSky;
        private static HereMapsWeatherDataItem.WeatherData hereWeatherWx;
        private static HereMapsWeatherDataItem.ForecastData hereWeatherFx;
        private static HereMapsWeatherDataItem.AstronomyData hereWeatherAx;
        private static OpenWeatherMapWeatherDataItem.WeatherData openWeatherWx;
        private static OpenWeatherMapWeatherDataItem.ForecastData openWeatherFx;
        private static WeatherBitWeatherDataItem.WeatherData weatherBitWx;
        private static WeatherBitWeatherDataItem.SixteenDayForecastData weatherBitFx;
        //private static WeatherUndergroundDataItem underground;
        // private static YahooWeatherDataItem yahoo; // Deprecated code that Yahoo! replaced in 2019	
        private static YahooWeatherYdnDataItem yahoo19;
        private static YrWeatherDataItem yrWeatherData;

        private StringBuilder wxUrl = new StringBuilder();
        private StringBuilder fxUrl = new StringBuilder();
        private StringBuilder axUrl = new StringBuilder();
        private List<string> strJSON;

        private const string CELSIUS = "\u00B0C";
        private const string DEGREES = "\u00B0";
        private const string FAHRENHEIT = "\u00B0F";
        private const string dayDateFormat = "{0:ddd d}";

        private static StringBuilder currentCity = new StringBuilder();
        private static StringBuilder currentCountry = new StringBuilder();
        private static StringBuilder currentTemp = new StringBuilder();
        private static StringBuilder currentFeelsLikeTemp = new StringBuilder();
        private static StringBuilder currentWindSpeed = new StringBuilder();
        private static StringBuilder currentWindDirection = new StringBuilder();
        private static StringBuilder currentHumidity = new StringBuilder();
        private static StringBuilder currentLocation = new StringBuilder();
        public static StringBuilder currentCondition = new StringBuilder();
        private static StringBuilder currentHigh = new StringBuilder();
        private static StringBuilder currentLow = new StringBuilder();
        private static List<FiveDayForecast> currentFiveDayForecast =
            new List<FiveDayForecast>();
        private static int[,] hl;

        private static bool unitChange;

        private WeatherDataXMLService wXML;
        private Dictionary<string, float[,]> dailyReading;

        private string tempUnits;

        private static XmlDocument weatherXML;
        private static XmlNode rootNode;
        private static XmlNode xmlAtmosphere;
        private static XmlNode xmlCurrent;
        private static XmlNode xmlWind;
        private static XmlNode xmlForecast;
        private static XmlNodeList xmlForecastList = null;

        private static readonly Hashtable hereMapsWeatherProductKeys = new Hashtable()
        {
            {"conditions", "observation"},
            {"forecast", "forecast_7days_simple"},
            {"astronomy", "forecast_astronomy"}
        };

        private static readonly ArrayList dWords = new ArrayList()
        {
            {"during"},
            {"in"},
            {"is"},
            {"overnight"},
            {"starting"},
            {"throughout"},
            {"until"},
            {"with"}
        };

        private static StringBuilder ts = new StringBuilder();

        // public variables
        public static string darkSkyApiKey = null;
        public static string hereAppId = null;
        public static string hereAppCode = null;
        public static string yahooConsumerKey = null;
        public static string yahooConsumerSecret = null;
        public static string yahooAppId = null;
        public static string openWeatherMapApiKey = null;
        public static string weatherBitApiKey = null;
        public static string weatherUndergroundApiKey = null;
        public static string geoNameAccount = null;

        public static StringBuilder sunriseTime = new StringBuilder();
        public static StringBuilder sunsetTime = new StringBuilder();

        public static Hashtable yahooWxAuth = new Hashtable()
        {
            {"cURL", "https://weather-ydn-yql.media.yahoo.com/forecastrss"},
            {"cAppID", yahooAppId},
            {"cConsumerKey", yahooConsumerKey},
            {"cConsumerSecret", yahooConsumerSecret},
            {"cOAuthVersion", "1.0"},
            {"cOAuthSignMethod", "HMAC-SHA1"},
            {"cUnitID", "u=f"},
            {"cFormat", "json"}
        };

        public static long currentRefreshInterval;
        public static string previousWeatherDataXml = $"{WeatherLionMain.DATA_DIRECTORY_PATH}{WeatherLionMain.WEATHER_DATA_XML}";

        public static WidgetForm frmWeatherWidget = null;    
        public static WidgetForm frmWeatherWidgetOtherThread = null;
        private delegate void CheckAstronomyDelegate();

        #endregion

        /// <summary>
        /// Access the <see cref="WidgetForm.CheckAstronomy"/> method running on the widget UI <see cref="Thread"/>.
        /// </summary>
        private void RunAstronomyCheck()
        {
            if (frmWeatherWidget.InvokeRequired)
            {
                var d = new CheckAstronomyDelegate(RunAstronomyCheck);
                frmWeatherWidget.Invoke(d, new object[] {});
            }// end of if block
            else
            {
                frmWeatherWidget.CheckAstronomy();
            }// end of else block
        }// end of method RunAstronomyCheck

        public WidgetUpdateService(bool unitChanged, WidgetForm activeForm)
        {
            unitChange = unitChanged;
            frmWeatherWidget = activeForm;            

            // store the current provider in case of failure
            frmWeatherWidget.previousWeatherProvider.Clear();
            frmWeatherWidget.previousWeatherProvider.Append(WeatherLionMain.storedPreferences.StoredPreferences.Provider);
        }// end of default constructor        

        /// <summary>
        /// Attempts to retrieve weather information from the weather providers' web services
        /// </summary>
        /// <returns>A list containing weather information in JSON format</returns>
        public void GetWeatherData()
        {
            tempUnits = WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? CELSIUS : FAHRENHEIT;
            currentCity.Clear();
            currentCity.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);
            string json = null;
            float lat;
            float lng;
            strJSON = new List<string>();
            string wxDataPrivider = null;

            if (WeatherLionMain.noAccessToStoredProvider)
            {
                wxDataPrivider = LionSecurityManager.webAccessGranted[0];
            }// end of if block
            else
            {
                wxDataPrivider = WeatherLionMain.storedPreferences.StoredPreferences.Provider;
            }// end of else block

            if (!unitChange)
            {
                // Check the Internet connection availability
                if (UtilityMethod.HasInternetConnection())
                {
                    wxUrl.Clear();
                    fxUrl.Clear();
                    axUrl.Clear();

                    switch (wxDataPrivider)
                    {
                        case WeatherLionMain.DARK_SKY:
                            json =
                                UtilityMethod.RetrieveGeoNamesGeoLocationUsingAddress(currentCity.ToString());

                            CityData.currentCityData = UtilityMethod.CreateGeoNamesCityData(json)[0];
                            lat = CityData.currentCityData.latitude;
                            lng = CityData.currentCityData.longitude;

                            wxUrl.Clear();
                            wxUrl.Append("https://api.darksky.net/forecast/" +
                                        darkSkyApiKey + "/" + lat + "," + lng);

                            break;
                        case WeatherLionMain.HERE_MAPS:
                            json =
                                UtilityMethod.RetrieveHereGeoLocationUsingAddress(currentCity.ToString());

                            CityData.currentCityData = UtilityMethod.CreateHereCityData(json);

                            wxUrl.Clear();
                            wxUrl.Append(
                                    "https://weather.api.here.com/weather/1.0/report.json?" +
                                    "app_id=" + hereAppId +
                                    "&app_code=" + hereAppCode +
                                    "&product=" + hereMapsWeatherProductKeys["conditions"] +
                                    "&name=" + UtilityMethod.EscapeUriString(currentCity.ToString()) +
                                    "&metric=false");

                            fxUrl.Clear();
                            fxUrl.Append(
                                    "https://weather.api.here.com/weather/1.0/report.json?" +
                                    "app_id=" + hereAppId +
                                    "&app_code=" + hereAppCode +
                                    "&product=" + hereMapsWeatherProductKeys["forecast"] +
                                    "&name=" + UtilityMethod.EscapeUriString(currentCity.ToString()) +
                                    "&metric=false");

                            axUrl.Clear();
                            axUrl.Append(
                                    "https://weather.api.here.com/weather/1.0/report.json?" +
                                    "app_id=" + hereAppId +
                                    "&app_code=" + hereAppCode +
                                    "&product=" + hereMapsWeatherProductKeys["astronomy"] +
                                    "&name=" + UtilityMethod.EscapeUriString(currentCity.ToString()) +
                                    "&metric=false");
                            break;
                        case WeatherLionMain.OPEN_WEATHER:
                            json =
                                UtilityMethod.RetrieveGeoNamesGeoLocationUsingAddress(currentCity.ToString());

                            CityData.currentCityData = UtilityMethod.CreateGeoNamesCityData(json)[0];
                            lat = CityData.currentCityData.latitude;
                            lng = CityData.currentCityData.longitude;

                            wxUrl.Clear();
                            wxUrl.Append("https://api.openweathermap.org/data/2.5/weather?" +
                                    "lat=" + lat + "&lon=" + lng + "&appid=" + openWeatherMapApiKey +
                                        "&units=imperial");

                            fxUrl.Clear();
                            fxUrl.Append(
                                    "https://api.openweathermap.org/data/2.5/forecast/daily?" +
                                        "lat=" + lat + "&lon=" + lng + "&appid=" + openWeatherMapApiKey +
                                        "&units=imperial");

                            break;                        
                        case WeatherLionMain.WEATHER_BIT:
                            json =
                                UtilityMethod.RetrieveGeoNamesGeoLocationUsingAddress(currentCity.ToString());

                            CityData.currentCityData = UtilityMethod.CreateGeoNamesCityData(json)[0];
                            lat = CityData.currentCityData.latitude;
                            lng = CityData.currentCityData.longitude;

                            wxUrl.Clear();
                            wxUrl.Append(
                                    "https://api.weatherbit.io/v2.0/current?city=" +
                                        UtilityMethod.EscapeUriString(currentCity.ToString()) +
                                            "&units=I&key=" + weatherBitApiKey);

                            // Sixteen day forecast will be used as it contains more relevant data
                            fxUrl.Clear();
                            fxUrl.Append(
                                    "https://api.weatherbit.io/v2.0/forecast/daily?city=" +
                                        UtilityMethod.EscapeUriString(currentCity.ToString()) +
                                            "&units=I&key=" + weatherBitApiKey);
                            break;
                        case WeatherLionMain.YAHOO_WEATHER:
                            try
                            {
                                strJSON.Add(GetYahooWeatherData(WeatherLionMain.storedPreferences.StoredPreferences.Location.ToLower()));
                            }// end of try block
                            catch (Exception e)
                            {
                                UtilityMethod.LogMessage("severe", e.Message,
                                  $"{nameof(WeatherLion)}::GetWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                                strJSON = null;
                            }// end of catch block

                            break;
                        case WeatherLionMain.YR_WEATHER:
                            json =
                                UtilityMethod.RetrieveGeoNamesGeoLocationUsingAddress(currentCity.ToString());

                            CityData.currentCityData = UtilityMethod.CreateGeoNamesCityData(json)[0];

                            string cityName =
                                    CityData.currentCityData.cityName.Contains(" ") ?
                                            CityData.currentCityData.cityName.Replace(" ", "_") :
                                                CityData.currentCityData.cityName;
                            string countryName =
                                    CityData.currentCityData.countryName.Contains(" ") ?
                                            CityData.currentCityData.countryName.Replace(" ", "_") :
                                                CityData.currentCityData.countryName;
                            string regionName = cityName.Equals("Kingston", StringComparison.OrdinalIgnoreCase) ? "Kingston" :
                                    CityData.currentCityData.regionName.Contains(" ") ?
                                        CityData.currentCityData.regionName.Replace(" ", "_") :
                                            CityData.currentCityData.regionName;   // Yr data mistakes Kingston as being in St. Andrew

                            wxUrl.Clear();
                            wxUrl.Append($"https://www.yr.no/place/{countryName}/{regionName}/{cityName}/forecast.xml");
                            break;                        
                    }// end of switch block
                }// end of if block	

                if (!wxDataPrivider.Equals(value: WeatherLionMain.YAHOO_WEATHER))
                {
                    if (wxUrl.Length != 0 && fxUrl.Length != 0 && axUrl.Length != 0)
                    {
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(wxUrl.ToString()));
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(fxUrl.ToString()));
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(axUrl.ToString()));
                    }// end of if block
                    else if (wxUrl.Length != 0 && fxUrl.Length != 0 && axUrl.Length == 0)
                    {
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(wxUrl.ToString()));
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(fxUrl.ToString()));
                    }// end of if block
                    else if (wxUrl.Length != 0 && fxUrl.Length == 0 && axUrl.Length == 0)
                    {
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(wxUrl.ToString()));
                    }// end of else if block
                    else if (wxUrl.Length == 0 && fxUrl.Length != 0 && axUrl.Length == 0)
                    {
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(fxUrl.ToString()));
                    }// end of else if block
                    else if (wxUrl.Length == 0 && fxUrl.Length == 0 && axUrl.Length != 0)
                    {
                        strJSON.Add(UtilityMethod.RetrieveWeatherData(axUrl.ToString()));
                    }// end of else if block
                }// end of if block                
            }// end of if block

            // process the data received
            ProcessWeatherData();
        }// end of method GetWeatherData

        /// <summary>
        /// <para>
        /// Yahoo! Developers Network 2019 documentation
        /// </para>
        /// URL: <seealso cref="https://developer.yahoo.com/weather/documentation.html?guccounter=1#oauth-csharp"/>
        /// </summary>
        /// <param name="wxCity">The city whose weather is being queried</param>
        /// <returns>A <see cref="string"/> representation of JSON data</returns>
        public static string GetYahooWeatherData(string wxCity)
        {
            string cityEncoded = wxCity.Replace(" ", "%2B").Replace(",", "%2C"); // add URL Encoding for two characters 
            TimeSpan lTS = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string timestamp = Convert.ToInt64(lTS.TotalSeconds).ToString();
            string nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            string lCKey = string.Concat(yahooConsumerSecret, "&");

            List<string> parameters = new List<string>
            {
                $"oauth_consumer_key={yahooConsumerKey}",
                $"oauth_nonce={nonce}",
                $"oauth_signature_method={(string)yahooWxAuth["cOAuthSignMethod"]}",
                $"oauth_timestamp={timestamp}",
                $"oauth_version={(string)yahooWxAuth["cOAuthVersion"]}",
                // Make sure value is encoded
                $"location={cityEncoded}",
                (string)yahooWxAuth["cUnitID"],
                $"format={(string)yahooWxAuth["cFormat"]}"
            };

            parameters.Sort();

            StringBuilder parametersList = new StringBuilder();

            for (int i = 0; i < parameters.Count; i++)
            {
                parametersList.Append(((i > 0) ? "&" : "") + parameters[i]);
            }// end of for loop

            string lSign = string.Concat(
             "GET&", Uri.EscapeDataString((string)yahooWxAuth["cURL"]), "&", 
             Uri.EscapeDataString(parametersList.ToString())
            );

            using (var lHasher = new HMACSHA1(Encoding.ASCII.GetBytes(lCKey)))
            {
                lSign = Convert.ToBase64String(
                    lHasher.ComputeHash(Encoding.ASCII.GetBytes(lSign))
                );
            }  // end using

            string oAuth = "OAuth " +
                   "oauth_consumer_key=\"" + yahooConsumerKey + "\", " +
                   "oauth_nonce=\"" + nonce + "\", " +
                   "oauth_timestamp=\"" + timestamp + "\", " +
                   "oauth_signature_method=\"" + (string)yahooWxAuth["cOAuthSignMethod"] + "\", " +
                   "oauth_signature=\"" + lSign + "\", " +
                   "oauth_version=\"" + (string)yahooWxAuth["cOAuthVersion"] + "\"";

            string lURL = $"{(string)yahooWxAuth["cURL"]}?location={cityEncoded}&{(string)yahooWxAuth["cUnitID"]}&format={(string)yahooWxAuth["cFormat"]}";
            var lClt = new WebClient();

            lClt.Headers.Set("Content-Type", $"application/{(string)yahooWxAuth["cFormat"]}");
            lClt.Headers.Add("X-Yahoo-App-Id", yahooAppId);
            lClt.Headers.Add("Authorization", oAuth);

            byte[] lDataBuffer = lClt.DownloadData(lURL);
            string lOut = Encoding.ASCII.GetString(lDataBuffer);

            return lOut;
        }// end of method GetYahooWeatherData

        /// <summary>
        /// Access controls on a <see cref="Form"/> running on another <see cref="Thread"/>.
        /// </summary>
        /// <param name="controlProperty">The <see cref="Form"/> <see cref="Control"/></param>
        /// <param name="propertyValue">The property of the <see cref="Control"/></param>
        private void UpdateWidgetControl(Control formControl, string controlProperty, string propertyValue)
        {
            // a null controlProperty means that the caller intends to access a field or call a method
            if (controlProperty != null)
            {
                PropertyInfo piInstance = formControl.GetType().GetProperty(controlProperty);

                if (piInstance.PropertyType == typeof(Boolean))
                {
                    formControl.Invoke((MethodInvoker)delegate
                    {
                        formControl.GetType().GetProperty(controlProperty).SetValue(formControl, bool.Parse(propertyValue));
                    });
                }// end of if block
                else if (piInstance.PropertyType == typeof(Color))
                {
                    formControl.Invoke((MethodInvoker)delegate
                    {
                        formControl.GetType().GetProperty(controlProperty).SetValue(formControl,
                            ColorTranslator.FromHtml($"#{propertyValue}"));
                    });
                }// end of if block
                else if (piInstance.PropertyType == typeof(Image))
                {
                    // the Current Conditions Imgae needs resizing
                    if (formControl.Name.Equals("picCurrentConditions"))
                    {
                        Image wxImage = Image.FromFile(propertyValue);

                        if (propertyValue.Contains("google now") ||
                            propertyValue.Contains("miui") ||
                            propertyValue.Contains("weezle"))
                        {
                            formControl.GetType().GetProperty(controlProperty).SetValue(formControl,
                                UtilityMethod.ResizeImage(wxImage, 120, 120));
                        }// end of if block
                        else
                        {
                            formControl.GetType().GetProperty(controlProperty).SetValue(formControl,
                                UtilityMethod.ResizeImage(wxImage, 140, 140));
                        }// end of else block                        
                    }// end of if block
                    else if (formControl.Name.Contains("picDay"))
                    {
                        Image wxImage = Image.FromFile(propertyValue);
                        formControl.GetType().GetProperty(controlProperty).SetValue(formControl,
                            UtilityMethod.ResizeImage(wxImage, 40, 40));
                    }// end of else if block
                    else
                    {
                        formControl.Invoke((MethodInvoker)delegate
                        {
                            formControl.GetType().GetProperty(controlProperty).SetValue(formControl, Image.FromFile(propertyValue));
                        });
                    }// end of else block                   
                }// end of if block
                else if (piInstance.PropertyType == typeof(String))
                {
                    formControl.Invoke((MethodInvoker)delegate
                    {
                        formControl.GetType().GetProperty(controlProperty).SetValue(formControl, propertyValue);
                    });
                }// end of if block 
            }// end of if block             
            else
            {
                // if the property value contains and equal sign then the user wants to access a field
                if (propertyValue.Contains("="))
                {
                    string[] func = propertyValue.Split('=');
                    
                    // attempt a recursive call to update the field
                    UpdateWidgetControl(formControl, func[0], func[1]);
                }// end of if block                              
            }// end of else block
        }// end of method UpdateWidgetControl

        /// <summary>
        /// Process the data recieved from the web service
        /// </summary>
        private void ProcessWeatherData()
        {
            if (!unitChange)
            {
                if (strJSON == null)
                {
                    UtilityMethod.LogMessage("severe", "No weather data received from the provider.",
                        "WidgetUpdateService::done");
                    return;
                }// end of if block                 

                // check that the ArrayList is not empty and the the first element is not null 
                if (strJSON != null && strJSON[0] != null)
                {
                    UpdateWidgetControl(frmWeatherWidget.picOffline, "Visible", "false"); // we are connected to the Internet if JSON data is returned
                    string provider = WeatherLionMain.storedPreferences.StoredPreferences.Provider;

                    try
                    {
                        switch (provider)
                        {
                            case WeatherLionMain.DARK_SKY:
                                darkSky = JsonConvert.DeserializeObject<DarkSkyWeatherDataItem>(strJSON[0]);
                                LoadDarkSkyWeather();

                                break;
                            case WeatherLionMain.HERE_MAPS:
                                hereWeatherWx = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.WeatherData>(strJSON[0]);
                                hereWeatherFx = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.ForecastData>(strJSON[1]);
                                hereWeatherAx = JsonConvert.DeserializeObject<HereMapsWeatherDataItem.AstronomyData>(strJSON[2]);
                                LoadHereMapsWeather();

                                break;
                            case WeatherLionMain.OPEN_WEATHER:
                                openWeatherWx = JsonConvert.DeserializeObject<OpenWeatherMapWeatherDataItem.WeatherData>(strJSON[0]);
                                openWeatherFx = JsonConvert.DeserializeObject<OpenWeatherMapWeatherDataItem.ForecastData>(strJSON[1]);
                                LoadOpenWeather();

                                break;
                            case WeatherLionMain.WEATHER_BIT:
                                weatherBitWx = JsonConvert.DeserializeObject<WeatherBitWeatherDataItem.WeatherData>(strJSON[0]);
                                weatherBitFx = JsonConvert.DeserializeObject<WeatherBitWeatherDataItem.SixteenDayForecastData>(strJSON[1]);
                                LoadWeatherBitWeather();

                                break;
                            case WeatherLionMain.YAHOO_WEATHER:

                                // Temporary fix for Yahoo! Weather bad JSON data
                                //								if( !strJSON.ToString().startsWith( "{" )  ) 
                                //								{
                                //									String temp = strJSON.ToString();
                                //									strJSON.clear();
                                //									strJSON.add( temp.Substring(temp.IndexOf( "{" ), temp.lastIndexOf( "}" ) + 1 ) );
                                //								}// end of if block

                                // Yahoo is constantly messing around with their API
                                string jsonWeatherObj = null;

                                // Check if a JSON was returned from the web service
                                foreach (string wxD in strJSON)
                                {
                                    if (UtilityMethod.IsValidJson(wxD))
                                    {
                                        jsonWeatherObj = wxD;
                                    }// end of if block		        				
                                }// end of for each loop

                                yahoo19 = JsonConvert.DeserializeObject<YahooWeatherYdnDataItem>(strJSON[0]);
                                LoadYahooYdnWeather();

                                break;
                            case WeatherLionMain.YR_WEATHER:
                                yrWeatherData = new YrWeatherDataItem();
                                YrWeatherDataItem.DeserializeYrXML(strJSON[0], ref yrWeatherData);
                                yrWeatherData = YrWeatherDataItem.yrWeatherDataItem;
                                LoadYrWeather();

                                break;
                            default:
                                break;
                        }// end of switch block

                        UtilityMethod.lastUpdated = DateTime.Now;

                        string timeUpdated = $"{UtilityMethod.lastUpdated.ToString("ddd h:mm tt")}";
                        currentLocation.Clear();
                        currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

                        // Update the current location and update time stamp
                        UpdateWidgetControl(frmWeatherWidget.btnLocation, "Text",
                            currentLocation.ToString().Substring(0, currentLocation.ToString().IndexOf(",")) +
                               ", " + timeUpdated);

                        // Update the weather provider label
                        UpdateWidgetControl(frmWeatherWidget.btnWeatherProvider, "Text",
                            WeatherLionMain.storedPreferences.StoredPreferences.Provider);
                        UpdateWidgetControl(frmWeatherWidget.btnWeatherProvider, "Image",
                           $"res/assets/img/icons/{WeatherLionMain.storedPreferences.StoredPreferences.Provider.ToLower()}.png");                       

                        if (UtilityMethod.refreshRequested)
                        {
                            UtilityMethod.refreshRequested = false;
                        }// end of if block

                        if (!frmWeatherWidget.Visible)
                        {
                            UpdateWidgetControl(frmWeatherWidget, "Visible", "true");
                        }// end of if block

                        WeatherLionMain.weatherLoadedFromProvider = true;
                        frmWeatherWidget.usingPreviousData = false;
                    }// end of try block
                    catch (Exception e)
                    {
                        UtilityMethod.LogMessage("severe", e.Message,
                           $"WidgetUpdateService::ProcessWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");

                        frmWeatherWidget.dataLoadedSuccessfully = false;

                        // Undo changes made
                        WeatherLionMain.storedPreferences.StoredPreferences.Provider =
                            frmWeatherWidget.previousWeatherProvider.ToString();

                        Preference.SaveProgramConfiguration("prefs", "Provider",
                            frmWeatherWidget.previousWeatherProvider.ToString());

                        if (WeatherLionMain.preferences.Visible)
                        {
                            WeatherLionMain.preferences.cboWeatherProviders.Invoke(
                                (MethodInvoker)delegate
                                {
                                    WeatherLionMain.preferences.cboWeatherProviders.SelectedItem = 
                                        frmWeatherWidget.previousWeatherProvider.ToString();
                                });
                        }// end of if block
                    }// end of catch block					 

                }// end of inner if block
                else // no json data was returned so check for Internet connectivity
                {
                    // Check the Internet connection availability
                    if (!UtilityMethod.HasInternetConnection())
                    {
                        string previousWeatherData = $"{WeatherLionMain.DATA_DIRECTORY_PATH}{WeatherLionMain.WEATHER_DATA_XML}";

                        // check for previous weather data stored locally
                        //if (File.Exists(previousWeatherData))
                        //{
                        //    LoadPreviousWeatherData();
                        //    WeatherLionMain.weatherLoadedFromProvider = false;

                        //    if (!frmWeatherWidget.picOffline.Visible)
                        //    {
                        //        UtilityMethod.ShowMessage("No internet connection was detected so "
                        //            + "previous weather\ndata will be used until connection to the internet is restored.",
                        //            frmWeatherWidget, $"{WeatherLionMain.PROGRAM_NAME} - Internet Connection Error");

                        //        UpdateWidgetControl(frmWeatherWidget.picOffline, "Visible", "true"); // display the offline icon on the widget
                        //    }// end of if block
                        //}// end of if block
                    }// end of if block	
                    else // we are connected to the Internet so that means the issue lies with the weather source
                    {
                        int scheduleTime = 5000; // Sleep for five seconds

                        // wait for five seconds and try the provider once more
                        while (frmWeatherWidget.running && !frmWeatherWidget.reAttempted)
                        {
                            Thread.Sleep(scheduleTime);

                            UtilityMethod.LogMessage("info", "Waiting to retry service provider...",
                                    "WidgetUpdateService::ProcessWeatherData");

                            UtilityMethod.LogMessage("info", "Retrying service provider...",
                                    "WidgetUpdateService::ProcessWeatherData");

                            frmWeatherWidget.reAttempted = true;

                            // run the weather service
                            WidgetUpdateService ws = new WidgetUpdateService(false, frmWeatherWidget);
                            ws.Run();

                        }// end of while block

                        if (frmWeatherWidget.usingPreviousData && frmWeatherWidget.reAttempted)
                        {
                            UtilityMethod.LogMessage("severe", $"Data service responded with: {strJSON.ToString()}",
                                   "WidgetUpdateService::ProcessWeatherData");

                            // return to the previous data service
                            WeatherLionMain.storedPreferences.StoredPreferences.Provider = frmWeatherWidget.previousWeatherProvider.ToString();

                            UtilityMethod.ShowMessage(WeatherLionMain.storedPreferences.StoredPreferences.Provider
                                + " seems to be non-responsive at the moment.\nRight click the widget and select"
                                + " another provider from the\npreferences dialog box.", frmWeatherWidget,
                                $"{WeatherLionMain.PROGRAM_NAME} - Weather Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            WeatherLionMain.preferences.ShowDialog(frmWeatherWidget);
                        }// end of else block	
                    }// end of else block
                }// end of inner else block
            }// end of if block
            else
            {
                UpdateTemps(true);
            }// end of else block

            // Update tooltip on all controls
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picRefresh, "Refresh Weather Data");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picOffline, "No Internet Connection");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblCurrentTemperature, "Current Temperature");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblFeelsLike, "Feels Like Temperature");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh, "Highest Temperature Today");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow, "Lowest Temperature Today");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblWeatherCondition, "Current Conditions");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnWindReading, "Current Wind Reading");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnHumidity, "Current Humidity");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnLocation, "Current Location");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnSunrise, "Sunrise Time");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnSunset, "Sunset Time");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.lblClock, "Current Time");
            UtilityMethod.AddControlToolTip(frmWeatherWidget.btnWeatherProvider, "Current Weather Provider");           
        }// end of method ProcessWeatherData

        /// <summary>
        /// Obtain the weather data from the Yr Norway weather service.
        /// </summary>
        /// <param name="wxLocation">The location to be queried.</param>

        /// <summary>
        /// Load the weather data from dark sky's weather service
        /// </summary>
        private void LoadDarkSkyWeather()
        {
            currentCity.Clear();
            currentCity.Append(CityData.currentCityData.cityName);

            currentCountry.Clear();
            currentCountry.Append(CityData.currentCityData.countryName);

            currentCondition.Clear(); // reset
            StringBuilder cc = new StringBuilder(UtilityMethod.ReplaceAll(darkSky.currently.icon, "-", " ").Trim());
            string[] omitWords = { "day", "night" };

            // Dark Sky attaches words to their icon files
            foreach (string word in omitWords)
            {
                if (cc.ToString().ToLower().Contains(word))
                {
                    cc.Replace(word, "");
                }// end of if block
            }// end of for each loop

            currentCondition.Append(UtilityMethod.ToProperCase(cc.ToString().Trim()));

            currentWindDirection.Clear();
            currentWindDirection.Append(UtilityMethod.CompassDirection(darkSky.currently.windBearing));

            currentHumidity.Clear();
            currentHumidity.Append($"{Math.Round(darkSky.currently.humidity * 100)}");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            sunriseTime.Clear();
            sunriseTime.Append(string.Format("{0:h:mm tt}",
                      UtilityMethod.GetDateTime(darkSky.daily.data[0].sunriseTime)).ToUpper());

            sunsetTime.Clear();
            sunsetTime.Append(string.Format("{0:h:mm tt}",
                UtilityMethod.GetDateTime(darkSky.daily.data[0].sunsetTime)).ToUpper());

            UpdateTemps(true); // call update temps here
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
                UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                $"{currentWindDirection} {currentWindSpeed} " +
                $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                       $"WidgetLionWidget::LoadDarkSkyWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
               

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            // Five Day Forecast
            int i = 1;
            currentFiveDayForecast.Clear(); // ensure that this list is clean

            foreach (DarkSkyWeatherDataItem.Daily.Data wxForecast in darkSky.daily.data)
            {
                DateTime fxDate = UtilityMethod.GetDateTime(wxForecast.time);
                StringBuilder fCondition = new StringBuilder(UtilityMethod.ReplaceAll(wxForecast.icon, "-", " ").Trim());

                // Dark Sky attaches words to their icon files
                foreach (string word in omitWords)
                {
                    if (fCondition.ToString().ToLower().Contains(word))
                    {
                        fCondition.Replace(word, "");
                    }// end of if block
                }// end of for each loop

                // Removes any spaces from the end
                while (fCondition.ToString()[fCondition.Length - 1] == ' ')
                {
                    fCondition.Remove(fCondition.Length - 1, 1);
                }// end of while loop                

                // this code may be remove in the future due to the use of the icon string
                // as opposed to the summary.
                foreach (string word in dWords)
                {
                    if (Regex.Match(fCondition.ToString(), $@"\b{word}\b", RegexOptions.IgnoreCase).Success)
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();

                        int index = ts.ToString().IndexOf($" {word} ") == -1
                                    ? ts.ToString().IndexOf($" {word}")
                                    : ts.ToString().IndexOf($" {word} ");

                        fCondition.Append(ts.ToString().Substring(0, index).Trim());
                    }// end of if block                            
                }// end of for each block                

                if (fCondition.ToString().ToLower().Contains("and"))
                {
                    string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" },
                        StringSplitOptions.None);

                    fCondition.Clear();
                    fCondition.Append(conditions[0].Trim());
                }// end of if block

                ts.Clear();
                ts.Append(fCondition.ToString());
                fCondition.Clear();
                fCondition.Append(UtilityMethod.ToProperCase(ts.ToString()));

                UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Day", true)[0],
                    "Text", string.Format("{0:ddd d}", fxDate));

                // Load current forecast condition weather image
                if (fCondition.ToString().ToLower().Contains("(day)"))
                {
                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(ts.ToString().Replace("(day)", "").Trim());
                }// end of if block
                else if (fCondition.ToString().ToLower().Contains("(night)"))
                {
                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(ts.ToString().Replace("(night)", "").Trim());
                }// end of if block

                StringBuilder fConditionIcon = new StringBuilder();

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            fConditionIcon.Clear();
                            fConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        fConditionIcon.Clear();
                        fConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    fConditionIcon.Clear();
                    fConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                fConditionIcon.Clear();
                fConditionIcon.Append(
                    UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null
                    ? "na.png" : (string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);

                UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                    "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                // Set image tooltip to forecast condition string
                UtilityMethod.AddControlToolTip((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                    fCondition.ToString().ToProperCase());

                currentFiveDayForecast.Add(
                        new FiveDayForecast(fxDate, hl[i - 1, 0].ToString(),
                            hl[i - 1, 1].ToString(), fCondition.ToString()));

                if (i == 5)
                {
                    break;
                }// end of if block

                i++; // increment sentinel
            }// end of for each loop

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            wXML = new WeatherDataXMLService(WeatherLionMain.DARK_SKY, DateTime.Now,
                   currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                   currentTemp.ToString().Substring(0, currentTemp.ToString().IndexOf(DEGREES)).Trim(),
                   currentFeelsLikeTemp.ToString(), currentHigh.ToString(), currentLow.ToString(),
                   currentWindSpeed.ToString(), currentWindDirection.ToString(), currentHumidity.ToString(),
                   sunriseTime.ToString(), sunsetTime.ToString(), currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadDarkSkyWeather

        /// <summary>
        /// Load the weather data from here maps' weather service
        /// </summary>
        private void LoadHereMapsWeather()
        {
            HereMapsWeatherDataItem.WeatherData.Observations.Location.Observation obs = hereWeatherWx.observations.location[0]
                    .observation[0];
            HereMapsWeatherDataItem.AstronomyData.Astronomic.Astronomy ast = hereWeatherAx.astronomy.astronomy[0];

            currentCity.Clear();
            currentCity.Append(CityData.currentCityData.cityName);

            currentCountry.Clear();
            currentCountry.Append(CityData.currentCityData.countryName);

            currentCondition.Clear();
            currentCondition.Append(obs.iconName.Contains("_") ?
                    UtilityMethod.ToProperCase(UtilityMethod.ReplaceAll(obs.iconName, "_", " ")) :
                        UtilityMethod.ToProperCase(UtilityMethod.ReplaceAll(obs.iconName, "_", " ")));

            currentWindDirection.Clear();
            currentWindDirection.Append(obs.windDescShort);

            currentWindSpeed.Clear();
            currentWindSpeed.Append(Math.Round(float.Parse(obs.windSpeed)));

            currentHumidity.Clear();
            currentHumidity.Append($"{Math.Round(float.Parse(obs.humidity))}");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            sunriseTime.Clear();
            sunriseTime.Append(ast.sunrise.ToUpper());

            sunsetTime.Clear();
            sunsetTime.Append(ast.sunset.ToUpper());
            List<HereMapsWeatherDataItem.ForecastData.DailyForecasts.ForecastLocation.Forecast> fdf =
                    hereWeatherFx.dailyForecasts.forecastLocation.forecast;

            UpdateTemps(true); // call update temps here
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
               UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                $"{currentWindDirection} {currentWindSpeed} " +
                $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetLionWidget::LoadHereMapsWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            // Five Day Forecast
            int i = 1;
            DateTime lastDate = new DateTime();
            currentFiveDayForecast.Clear(); // ensure that this list is clean
            StringBuilder ts = new StringBuilder();

            // loop through the forecast data. only 5 days are needed
            foreach (HereMapsWeatherDataItem.ForecastData.DailyForecasts.ForecastLocation.Forecast wxForecast in fdf)
            {
                DateTime? fxDate = null;

                try
                {
                    fxDate = DateTime.Parse(wxForecast.utcTime.Substring(0, 10));
                }// end of try block
                catch (Exception pe)
                {
                    UtilityMethod.LogMessage("severe", pe.Message,
                         $"WidgetLionWidget::LoadHereMapsWeather [line: {UtilityMethod.GetExceptionLineNumber(pe)}]");
                }// end of catch block                              

                if (!string.Format("{0:MMM dd, yyyy}", fxDate).Equals(string.Format("{0:MMM dd, yyyy}", lastDate)))
                {
                    lastDate = (DateTime)fxDate;

                    StringBuilder fCondition = new StringBuilder(wxForecast.iconName.Contains("_") ?
                        UtilityMethod.ToProperCase(UtilityMethod.ReplaceAll(wxForecast.iconName, "_", " ")) :
                            UtilityMethod.ToProperCase(UtilityMethod.ReplaceAll(wxForecast.iconName, "_", " ")));
                    string fDay = string.Format("{0:ddd d}", fxDate);

                    foreach (string word in dWords)
                    {
                        if (Regex.Match(fCondition.ToString(), $@"\b{word}\b", RegexOptions.IgnoreCase).Success)
                        {
                            ts.Clear();
                            ts.Append(fCondition.ToString());
                            fCondition.Clear();

                            int index = ts.ToString().IndexOf($" {word} ") == -1
                                        ? ts.ToString().IndexOf($" {word}")
                                        : ts.ToString().IndexOf($" {word} ");

                            fCondition.Append(ts.ToString().Substring(0, index).Trim());
                        }// end of if block                            
                    }// end of for each block                

                    if (fCondition.ToString().Contains("is "))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Substring(ts.ToString().IndexOf("is ") + 3,
                            ts.ToString().Length).Trim());
                    }// end of if block

                    if (fCondition.ToString().ToLower().Contains("and"))
                    {
                        string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);

                        fCondition.Clear();
                        fCondition.Append(conditions[0].Trim());
                    }// end of if block

                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(UtilityMethod.ToProperCase(ts.ToString()));

                    UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Day", true)[0],
                        "Text", string.Format("{0:ddd d}", fDay));

                    // Load current forecast condition weather image
                    if (fCondition.ToString().ToLower().Contains("(day)"))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Replace("(day)", "").Trim());
                    }// end of if block
                    else if (fCondition.ToString().ToLower().Contains("(night)"))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Replace("(night)", "").Trim());
                    }// end of if block

                    StringBuilder fConditionIcon = new StringBuilder();

                    if (UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null)
                    {
                        // sometimes the JSON data received is incomplete so this has to be taken into account 
                        foreach (string key in UtilityMethod.weatherImages.Keys)
                        {
                            if (key.StartsWith(fCondition.ToString().ToLower()))
                            {
                                fConditionIcon.Clear();
                                fConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                                fCondition.Clear(); // reset
                                fCondition.Append(key);
                                break; // exit the loop
                            }// end of if block
                        }// end of for block

                        // if a match still could not be found, use the not available icon
                        if (fConditionIcon == null)
                        {
                            fConditionIcon.Clear();
                            fConditionIcon.Append("na.png");
                        }// end of if block	            	
                    }// end of if block 
                    else
                    {
                        fConditionIcon.Clear();
                        fConditionIcon.Append(UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);
                    }// end of if block

                    fConditionIcon.Clear();
                    fConditionIcon.Append(
                        UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null
                        ? "na.png" : (string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);

                    UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                        "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                    // Set image tooltip to forecast condition string
                    UtilityMethod.AddControlToolTip((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                        fCondition.ToString().ToProperCase());

                    currentFiveDayForecast.Add(
                            new FiveDayForecast((DateTime)fxDate, hl[i - 1, 0].ToString(),
                                hl[i - 1, 1].ToString(), fCondition.ToString()));

                    if (i == 5)
                    {
                        break;
                    }// end of if block

                    i++;
                }// end of if block
            }// end of for each loop

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            wXML = new WeatherDataXMLService(WeatherLionMain.HERE_MAPS, DateTime.Now,
                    currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                    currentTemp.ToString().Substring(0, currentTemp.ToString().IndexOf(DEGREES)).Trim(),
                    currentFeelsLikeTemp.ToString(), currentHigh.ToString(), currentLow.ToString(),
                    currentWindSpeed.ToString(), currentWindDirection.ToString(), currentHumidity.ToString(),
                    sunriseTime.ToString(), sunsetTime.ToString(), currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadHereMapsWeather

        /// <summary>
        /// Load the weather data from open weather maps' weather service
        /// </summary>
        private void LoadOpenWeather()
        {
            currentCity.Clear();
            currentCity.Append(CityData.currentCityData.cityName);

            currentCountry.Clear();
            currentCountry.Append(CityData.currentCityData.countryName);

            currentCondition.Clear(); // reset
            currentCondition.Append(openWeatherWx.weather[0].description);

            currentWindDirection.Clear(); // reset
            currentWindDirection.Append(UtilityMethod.CompassDirection(openWeatherWx.wind.deg));

            currentHumidity.Clear();
            currentHumidity.Append($"{Math.Round(openWeatherWx.main.humidity)}");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            sunriseTime.Clear();
            sunriseTime.Append(string.Format("{0:h:mm tt}",
                    UtilityMethod.GetDateTime(openWeatherWx.sys.sunrise)).ToUpper());

            sunsetTime.Clear();
            sunsetTime.Append(string.Format("{0:h:mm tt}",
                    UtilityMethod.GetDateTime(openWeatherWx.sys.sunset)).ToUpper());

            List<OpenWeatherMapWeatherDataItem.ForecastData.Data> fdf = openWeatherFx.list;

            UpdateTemps(true); // call update temps here
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
                UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                $"{currentWindDirection} {currentWindSpeed} " +
                $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetLionWidget::LoadHereMapsWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            // Five Day Forecast
            int i = 1;
            DateTime lastDate = new DateTime();
            currentFiveDayForecast.Clear(); // ensure that this list is clean

            // loop through the forecast data. only 5 days are needed
            foreach (OpenWeatherMapWeatherDataItem.ForecastData.Data wxForecast in fdf)
            {
                DateTime fxDate = UtilityMethod.GetDateTime(wxForecast.dt);

                if (!string.Format("{0:MMM dd, yyyy}", fxDate).Equals(string.Format("{0:MMM dd, yyyy}", lastDate)))
                {
                    lastDate = UtilityMethod.GetDateTime(wxForecast.dt);
                    StringBuilder fCondition = new StringBuilder(wxForecast.weather[0].main);
                    StringBuilder ts = new StringBuilder();

                    string fDay = string.Format("{0:ddd d}", fxDate);

                    foreach (string word in dWords)
                    {
                        if (UtilityMethod.ContainsWholeWord(fCondition.ToString(), word))
                        {
                            ts.Clear();
                            ts.Append(fCondition.ToString());
                            fCondition.Clear();

                            int index = ts.ToString().IndexOf($" {word} ") == -1
                                        ? ts.ToString().IndexOf($" {word}")
                                        : ts.ToString().IndexOf($" {word} ");

                            fCondition.Append(ts.ToString().Substring(0, index).Trim());
                        }// end of if block                            
                    }// end of for each block              

                    if (fCondition.ToString().Contains("is "))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Substring(ts.ToString().IndexOf("is ") + 3,
                            fCondition.ToString().Length).Trim());
                    }// end of if block

                    if (fCondition.ToString().ToLower().Contains("and"))
                    {
                        string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);

                        fCondition.Clear();
                        fCondition.Append(conditions[0].Trim());
                    }// end of if block

                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(UtilityMethod.ToProperCase(ts.ToString()));

                    if (fCondition.ToString().Contains("is "))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Substring(ts.ToString().IndexOf("is ") + 3,
                            ts.ToString().Length).Trim());
                    }// end of if block

                    if (fCondition.ToString().ToLower().Contains("and"))
                    {
                        string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);

                        fCondition.Append(conditions[0].Trim());
                    }// end of if block

                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(UtilityMethod.ToProperCase(ts.ToString()));

                    // Load current forecast condition weather image
                    if (fCondition.ToString().ToLower().Contains("(day)"))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Replace("(day)", "").Trim());
                    }// end of if block
                    else if (fCondition.ToString().ToLower().Contains("(night)"))
                    {
                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(ts.ToString().Replace("(night)", "").Trim());
                    }// end of if block            

                    UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Day", true)[0],
                        "Text", string.Format("{0:ddd d}", fDay));

                    StringBuilder fConditionIcon = new StringBuilder();

                    if (UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null)
                    {
                        // sometimes the JSON data received is incomplete so this has to be taken into account 
                        foreach (string key in UtilityMethod.weatherImages.Keys)
                        {
                            if (key.StartsWith(fCondition.ToString().ToLower()))
                            {
                                fConditionIcon.Clear();
                                fConditionIcon.Append((string)UtilityMethod.weatherImages[key]);// use the closest match
                                break; // exit the loop
                            }// end of if block
                        }// end of for block

                        // if a match still could not be found, use the not available icon
                        if (fConditionIcon == null)
                        {
                            fConditionIcon.Clear();
                            fConditionIcon.Append("na.png");
                        }// end of if block	            	
                    }// end of if block 
                    else
                    {
                        fConditionIcon.Clear();
                        fConditionIcon.Append(UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);
                    }// end of if block

                    fConditionIcon.Clear();
                    fConditionIcon.Append(
                        UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null
                        ? "na.png" : (string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);

                    UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                        "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                    // Set image tooltip to forecast condition string
                    UtilityMethod.AddControlToolTip(((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0]),
                        fCondition.ToString().ToProperCase());

                    currentFiveDayForecast.Add(
                            new FiveDayForecast(fxDate, hl[i - 1, 0].ToString(),
                                hl[i - 1, 1].ToString(), fCondition.ToString()));

                    if (i == 5)
                    {
                        break;
                    }// end of if block

                    i++;
                }// end of if block
            }// end of for each loop

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            wXML = new WeatherDataXMLService(WeatherLionMain.OPEN_WEATHER, DateTime.Now,
                    currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                    currentTemp.ToString().Substring(0, currentTemp.ToString().IndexOf(DEGREES)).Trim(),
                    currentFeelsLikeTemp.ToString(), currentHigh.ToString(), currentLow.ToString(),
                    currentWindSpeed.ToString(), currentWindDirection.ToString(), currentHumidity.ToString(),
                    sunriseTime.ToString(), sunsetTime.ToString(), currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadOpenWeather

        /// <summary>
        /// Load the weather data from weather bit's weather service
        /// </summary>
        private void LoadWeatherBitWeather()
        {
            currentCity.Clear();
            currentCity.Append(CityData.currentCityData.cityName);

            currentCountry.Clear();
            currentCountry.Append(CityData.currentCityData.countryName);

            currentCondition.Clear(); // reset
            currentCondition.Append(UtilityMethod.ToProperCase(
                    weatherBitWx.data[0].weather.description));

            currentWindDirection.Clear();
            currentWindDirection.Append(weatherBitWx.data[0].wind_cdir);

            currentHumidity.Clear();
            currentHumidity.Append($"{weatherBitWx.data[0].rh}");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            // Weather seems to be in a time-zone that is four hours ahead of Eastern Standard Time
            // They do not supply that information though.
            int tzOffset = 5;

            sunriseTime.Clear();
            sunriseTime.Append(UtilityMethod.Get12HourTime(
                int.Parse(weatherBitWx.data[0].sunrise.Split(':')[0])
                    - tzOffset, int.Parse(
                        weatherBitWx.data[0].sunrise.Split(':')[1])));

            sunsetTime.Clear();
            sunsetTime.Append(UtilityMethod.Get12HourTime(
                 int.Parse(weatherBitWx.data[0].sunset.Split(':')[0])
                     - tzOffset, int.Parse(
                         weatherBitWx.data[0].sunset.Split(':')[1])));

            List<WeatherBitWeatherDataItem.SixteenDayForecastData.Data> fdf = weatherBitFx.data;

            // call update temps here
            UpdateTemps(true);
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
                UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                $"{currentWindDirection} {currentWindSpeed} " +
                $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetLionWidget::LoadHereMapsWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            // Five Day Forecast
            int hour = DateTime.Now.Hour;
            int currentHour = hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour);
            int i = 1;
            currentFiveDayForecast.Clear(); // ensure that this list is clean

            // loop through the 16 day forecast data. only 5 days are needed
            foreach (WeatherBitWeatherDataItem.SixteenDayForecastData.Data wxForecast in fdf)
            {
                DateTime? fxDate = null;
                string dt = wxForecast.datetime;

                try
                {
                    fxDate = DateTime.Parse(dt);
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                         $"{nameof(WeatherLion)}::LoadWeatherBitWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
               
                bool later = fxDate > DateTime.Now;

                if (later)
                {
                    string fDay = string.Format("{0:ddd d}", fxDate);
                    StringBuilder fCondition = new StringBuilder(wxForecast.weather.description);

                    if (later)
                    {
                        foreach (string word in dWords)
                        {
                            if (UtilityMethod.ContainsWholeWord(fCondition.ToString(), word))
                            {
                                ts.Clear();
                                ts.Append(fCondition.ToString());
                                fCondition.Append(ts.ToString().Substring(0, ts.ToString().IndexOf(word) - 1).Trim());
                            }// end of if block                            
                        }// end of for each block

                        if (fCondition.ToString().Contains("is "))
                        {
                            ts.Clear();
                            ts.Append(fCondition.ToString());
                            fCondition.Append(ts.ToString().Substring(ts.ToString().IndexOf("is ") + 3, ts.ToString().Length).Trim());
                        }// end of if block

                        if (fCondition.ToString().ToLower().Contains("and"))
                        {
                            string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);

                            fCondition.Clear();
                            fCondition.Append(conditions[0].Trim());
                        }// end of if block

                        ts.Clear();
                        ts.Append(fCondition.ToString());
                        fCondition.Clear();
                        fCondition.Append(UtilityMethod.ToProperCase(ts.ToString()));
                        ((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Day", true)[0]).Text = fDay;

                        // Load current forecast condition weather image
                        if (fCondition.ToString().ToLower().Contains("(day)"))
                        {
                            ts.Clear();
                            ts.Append(fCondition.ToString());
                            fCondition.Clear();
                            fCondition.Append(ts.ToString().Replace("(day)", "").Trim());
                        }// end of if block
                        else if (fCondition.ToString().ToLower().Contains("(night)"))
                        {
                            ts.Clear();
                            ts.Append(fCondition.ToString());
                            fCondition.Clear();
                            fCondition.Append(ts.ToString().Replace("(night)", "").Trim());
                        }// end of if block

                        StringBuilder fConditionIcon = new StringBuilder();

                        if (UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null)
                        {
                            // sometimes the JSON data received is incomplete so this has to be taken into account 
                            foreach (string key in UtilityMethod.weatherImages.Keys)
                            {
                                if (key.StartsWith(fCondition.ToString().ToLower()))
                                {
                                    fConditionIcon.Clear();
                                    fConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                                    break; // exit the loop
                                }// end of if block
                            }// end of for block

                            // if a match still could not be found, use the not available icon
                            if (fConditionIcon == null)
                            {
                                fConditionIcon.Clear();
                                fConditionIcon.Append("na.png");
                            }// end of if block	            	
                        }// end of if block 
                        else
                        {
                            fConditionIcon.Clear();
                            fConditionIcon.Append((string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);
                        }// end of if block

                        UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                            "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                        // Set image tooltip to forecast condition string
                        UtilityMethod.AddControlToolTip(((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0]),
                            fCondition.ToString().ToProperCase());

                        currentFiveDayForecast.Add(
                                new FiveDayForecast((DateTime)fxDate,
                                hl[i - 1, 0].ToString(), hl[i - 1, 1].ToString(), fCondition.ToString()
                            ));

                        if (i == 5)
                        {
                            break;
                        }// end of if block

                        i++; // increment sentinel

                    }// end of if block
                }// end of if block
            }// end of for each loop

            string ct = null;

            // sometimes weather bit includes symbols in their data
            if (currentTemp.ToString().Contains(DEGREES))
            {
                ct = currentTemp.ToString().Substring(0, currentTemp.ToString().IndexOf(DEGREES)).Trim();
            }// end of if block
            else
            {
                ct = currentTemp.ToString();
            }// end of else block

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            wXML = new WeatherDataXMLService(WeatherLionMain.WEATHER_BIT, DateTime.Now,
                    currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                    ct, currentFeelsLikeTemp.ToString(), currentHigh.ToString(),
                    currentLow.ToString(), currentWindSpeed.ToString(), currentWindDirection.ToString(),
                    currentHumidity.ToString(), sunriseTime.ToString(), sunsetTime.ToString(),
                    currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadWeatherBitWeather

        /// <summary>
        /// Load the weather data from yahoo's weather service as of 2019
        /// </summary>
        private void LoadYahooYdnWeather()
        {
            currentCity.Clear();
            currentCity.Append($"{yahoo19.location.city}, {yahoo19.location.region}");

            currentCountry.Clear(); // reset
            currentCountry.Append(yahoo19.location.country);

            currentCondition.Clear(); // reset
            currentCondition.Append(yahoo19.current_observation.condition.text);

            currentHumidity.Clear();
            currentHumidity.Append($"{Math.Round(yahoo19.current_observation.atmosphere.humidity)}");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            sunriseTime.Clear(); // reset
            sunriseTime.Append(yahoo19.current_observation.astronomy.sunrise.ToUpper());

            sunsetTime.Clear(); // reset
            sunsetTime.Append(yahoo19.current_observation.astronomy.sunset.ToUpper());

            UpdateTemps(true); // call update temps here
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
                UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                $"{currentWindDirection} {Math.Round(double.Parse(currentWindSpeed.ToString()))} " +
                $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");
            
            // Yahoo loves to omit a zero on the hour mark ex: 7:0 am
            if (sunriseTime.Length == 6)
            {
                string[] ft = sunriseTime.ToString().Split(':');
                sunriseTime.Append(ft[0] + ":0" + ft[1]);
            }// end of if block
            else if (sunsetTime.Length == 6)
            {
                string[] ft = sunsetTime.ToString().Split(':');
                sunsetTime.Append(ft[0] + ":0" + ft[1]);
            }// end if else if block            

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetLionWidget::LoadDarkSkyWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            List<YahooWeatherYdnDataItem.Forecast> fdf = yahoo19.forecasts;
            currentFiveDayForecast.Clear(); // ensure that this list is clean

            for (int i = 0; i <= fdf.Count; i++)
            {
                DateTime fDate = UtilityMethod.GetDateTime(fdf[i].date);

                UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i + 1}Day", true)[0],
                    "Text", string.Format(dayDateFormat, fDate));

                // Load current forecast condition weather image
                StringBuilder fCondition = new StringBuilder(UtilityMethod.yahooWeatherCodes[fdf[i].code]);

                if (fCondition.ToString().ToLower().Contains("(day)"))
                {
                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(ts.ToString().Replace("(day)", "").Trim());
                }// end of if block
                else if (fCondition.ToString().ToLower().Contains("(night)"))
                {
                    ts.Clear();
                    ts.Append(fCondition.ToString());
                    fCondition.Clear();
                    fCondition.Append(ts.ToString().Replace("(night)", "").Trim());
                }// end of if block

                if (fCondition.ToString().ToLower().Contains("and"))
                {
                    string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);
                                     
                    fCondition.Clear();
                    fCondition.Append(conditions[0].Trim());
                }// end of if block

                StringBuilder fConditionIcon = new StringBuilder();

                if (UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(fCondition.ToString().ToLower()))
                        {
                            fConditionIcon.Clear();
                            fConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match                           
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (fConditionIcon == null)
                    {
                        fConditionIcon.Clear();
                        fConditionIcon.Append("na.png");
                    }// end of if block	
                }// end of if block 
                else
                {
                    fConditionIcon.Clear();
                    fConditionIcon.Append(UtilityMethod.weatherImages[fCondition.ToString().ToLower()]);
                }// end of if block

                UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i + 1}Image", true)[0],
                    "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                // Set image tooltip to forecast condition string
                UtilityMethod.AddControlToolTip(((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i + 1}Image", true)[0]),
                    fCondition.ToString().ToProperCase());

                DateTime forecastDate = UtilityMethod.GetDateTime(fdf[i].date);

                currentFiveDayForecast.Add(
                    new FiveDayForecast(forecastDate, hl[i, 0].ToString(),
                            hl[i, 1].ToString(), fCondition.ToString()));
                if (i == 4)
                {
                    break;
                }// end of if block
            }// end of for loop

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            // precautionary actions
            if (currentHigh.Length == 0) currentHigh.Append(0);
            if (currentLow.Length == 0) currentLow.Append(0);

            wXML = new WeatherDataXMLService(WeatherLionMain.YAHOO_WEATHER, DateTime.Now,
                    currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                    currentTemp.ToString(), currentFeelsLikeTemp.ToString(), currentHigh.ToString(),
                    currentLow.ToString(),  currentWindSpeed.ToString(), currentWindDirection.ToString(),
                    currentHumidity.ToString(), sunriseTime.ToString(), sunsetTime.ToString(),
                    currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadYahooYdnWeather

        /// <summary>
        /// Load the weather data from Yr.no (Norwegian Metrological Institute)'s weather service
        /// </summary>
        private void LoadYrWeather()
        {
            currentCity.Clear();
            currentCity.Append(yrWeatherData.name);

            currentCountry.Clear();
            currentCountry.Append(yrWeatherData.country);

            currentCondition.Clear(); // reset
            currentCondition.Append(UtilityMethod.ToProperCase(yrWeatherData.forecast[0].symbolName));

            currentHumidity.Clear();
            currentHumidity.Append(currentHumidity.Length != 0 ? currentHumidity.ToString() : "0"); // use the humidity reading from previous providers

            // append a zero if there is no humidity
            if (currentHumidity.Length == 0) currentHumidity.Append("0");

            currentLocation.Clear();
            currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

            sunriseTime.Clear();
            sunriseTime.Append(string.Format("{0:h:mm tt}", yrWeatherData.sunrise));

            sunsetTime.Clear();
            sunsetTime.Append(string.Format("{0:h:mm tt}", yrWeatherData.sunset));

            // call update temps here
            UpdateTemps(true);
            RunAstronomyCheck();

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
                UtilityMethod.ToProperCase(currentCondition.ToString()));

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
               $"{currentWindDirection} {currentWindSpeed} " +
               $"{(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text",
                !currentHumidity.ToString().Contains("%") ? $"{currentHumidity}%" : $"{currentHumidity}");

            DateTime timeUpdated = yrWeatherData.lastupdate;
            string tu = currentLocation.ToString().Contains(",")
                    ? currentLocation.ToString().Substring(0, currentLocation.ToString().IndexOf(","))
                        + ", " + string.Format("{0:ddd h:mm tt}", timeUpdated)
                    : currentLocation + ", "
                        + string.Format("{0:ddd h:mm tt}", timeUpdated);

            UpdateWidgetControl(frmWeatherWidget.btnLocation, "Text", tu);

            // Some providers like Yahoo love to omit a zero on the hour mark example: 7:0 am
            if (sunriseTime.Length == 6)
            {
                string[] ft = sunriseTime.ToString().Split(':');
                sunriseTime.Append(ft[0] + ":0" + ft[1]);
            }// end of if block
            else if (sunsetTime.Length == 6)
            {
                string[] ft = sunsetTime.ToString().Split(':');
                sunsetTime.Append(ft[0] + ":0" + ft[1]);
            }// end if else if block

            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());

            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} " +
                    $"{UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetLionWidget::LoadDarkSkyWeather [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            StringBuilder currentConditionIcon = new StringBuilder();

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().Contains("(night)"))
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of if block
                else
                {
                    if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"]);
                    }// end of if block
                    else
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                    }// end of else block
                }// end of else block

                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 

                frmWeatherWidget.sunsetIconsInUse = true;
                frmWeatherWidget.sunriseIconsInUse = false;
            }// end of if block                
            else
            {
                if (UtilityMethod.weatherImages[currentCondition.ToString().ToLower()] == null)
                {
                    // sometimes the JSON data received is incomplete so this has to be taken into account 
                    foreach (string key in UtilityMethod.weatherImages.Keys)
                    {
                        if (key.StartsWith(currentCondition.ToString().ToLower()))
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                            currentCondition.Clear(); // reset
                            currentCondition.Append(key);
                            break; // exit the loop
                        }// end of if block
                    }// end of for block

                    // if a match still could not be found, use the not available icon
                    if (currentConditionIcon == null)
                    {
                        currentConditionIcon.Clear();
                        currentConditionIcon.Append("na.png");
                    }// end of if block
                }// end of if block 
                else
                {
                    currentConditionIcon.Clear();
                    currentConditionIcon.Append((string)UtilityMethod.weatherImages[currentCondition.ToString().ToLower()]);
                }// end of else block

                frmWeatherWidget.sunriseIconsInUse = true;
                frmWeatherWidget.sunsetIconsInUse = false;
            }// end of else block

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            List<YrWeatherDataItem.Forecast> fdf = yrWeatherData.forecast;
            //SimpleDateFormat df = new SimpleDateFormat("MMMM dd, yyyy");

            int i = 1;
            int x = 0;
            currentFiveDayForecast.Clear(); // ensure that this list is clean

            foreach (YrWeatherDataItem.Forecast wxDailyForecast in fdf)
            {
                x++;

                // the first time period is one that will be stored
                if (x == 1)
                {
                    DateTime forecastDate = wxDailyForecast.timeFrom;
                    UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Day", true)[0],
                        "Text", string.Format("{0:ddd d}", forecastDate));

                    // Load current forecast condition weather image
                    string fCondition = wxDailyForecast.symbolName;

                    if (fCondition.ToString().ToLower().Contains("(day)"))
                    {
                        fCondition = fCondition.ToString().Replace("(day)", "").Trim();
                    }// end of if block
                    else if (fCondition.ToString().ToLower().Contains("(night)"))
                    {
                        fCondition = fCondition.ToString().Replace("(night)", "").Trim();
                    }// end of if block

                    if (fCondition.ToString().ToLower().Contains("and"))
                    {
                        string[] conditions = fCondition.ToString().ToLower().Split(new string[] { "and" }, StringSplitOptions.None);

                        fCondition = conditions[0].Trim();
                    }// end of if block

                    string fConditionIcon = null;

                    if (UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null)
                    {
                        // sometimes the JSON data received is incomplete so this has to be taken into account 
                        foreach (string key in UtilityMethod.weatherImages.Keys)
                        {
                            if (key.StartsWith(currentCondition.ToString().ToLower()))
                            {
                                currentConditionIcon.Clear();
                                currentConditionIcon.Append((string)UtilityMethod.weatherImages[key]); // use the closest match
                                currentCondition.Clear(); // reset
                                currentCondition.Append(key);
                                break; // exit the loop
                            }// end of if block
                        }// end of for block

                        // if a match still could not be found, use the not available icon
                        if (currentConditionIcon == null)
                        {
                            currentConditionIcon.Clear();
                            currentConditionIcon.Append("na.png");
                        }// end of if block      	
                    }// end of if block 
                    else
                    {
                        fConditionIcon = (string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()];
                    }// end of if block

                    UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0],
                        "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");

                    // Set image tooltip to forecast condition string
                    UtilityMethod.AddControlToolTip(((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i}Image", true)[0]),
                        fCondition.ToString().ToProperCase());

                    currentFiveDayForecast.Add(
                            new FiveDayForecast(forecastDate,
                                dailyReading[string.Format("{0:MMMM dd, yyyy}", wxDailyForecast.timeFrom)][0, 0].ToString(),
                                dailyReading[string.Format("{0:MMMM dd, yyyy}", wxDailyForecast.timeFrom)][0, 1].ToString(),
                                fCondition));
                    if (i == 5)
                    {
                        break;
                    }// end of if block

                    i++; // increment sentinel
                }// end of if block

                if (wxDailyForecast.timePeriod == 3)
                {
                    x = 0;
                }// end of if block
            }// end of for loop

            // if the code gets to here then all was loaded successfully
            frmWeatherWidget.dataLoadedSuccessfully = true;

            wXML = new WeatherDataXMLService(WeatherLionMain.YR_WEATHER, timeUpdated,
                    currentCity.ToString(), currentCountry.ToString(), currentCondition.ToString(),
                    currentTemp.ToString().Substring(0, currentTemp.ToString().IndexOf(DEGREES)).Trim(),
                    currentFeelsLikeTemp.ToString(), currentHigh.ToString(), currentLow.ToString(),
                    currentWindSpeed.ToString(), currentWindDirection.ToString(), currentHumidity.ToString(),
                    sunriseTime.ToString(), sunsetTime.ToString(), currentFiveDayForecast);

            wXML.ProcessData();
        }// end of method LoadYrWeather

        /// <summary>
        /// Load Weather previously received from provider
        /// </summary>
        public void LoadPreviousWeatherData()
        {
            XmlNode xmlProvider = null;
            XmlNode xmlAstronomy;
            XmlNode xmlLocation;
            bool useMetric = false;

            // check for previous weather data stored locally
            if (File.Exists(previousWeatherDataXml))
            {
                weatherXML = new XmlDocument();

                try
                {
                    // download the document from the URL and build it
                    weatherXML.Load(previousWeatherDataXml);

                    // get the root node of the XML document
                    rootNode = weatherXML.DocumentElement;

                    // get the root node of the XML document
                    xmlProvider = rootNode.SelectSingleNode("//Provider");
                    xmlLocation = rootNode.SelectSingleNode("//Location");
                    xmlAtmosphere = rootNode.SelectSingleNode("//Atmosphere");
                    xmlWind = rootNode.SelectSingleNode("//Wind");
                    xmlAstronomy = rootNode.SelectSingleNode("//Astronomy");
                    xmlCurrent = rootNode.SelectSingleNode("//Current");
                    xmlForecast = rootNode.SelectSingleNode("//DailyForecast");
                    xmlForecastList = rootNode.SelectNodes("//DailyForecast/DayForecast");

                    currentCity.Clear();
                    currentCity.Append(xmlLocation.SelectSingleNode("//City").InnerText);

                    currentCountry.Clear();
                    currentCountry.Append(xmlLocation.SelectSingleNode("//Country").InnerText);

                    currentCondition.Clear(); // reset
                    currentCondition.Append(xmlCurrent.SelectSingleNode("//Condition").InnerText);

                    currentWindDirection.Clear(); // reset
                    currentWindDirection.Append(xmlWind.SelectSingleNode("//WindDirection").InnerText);

                    currentWindSpeed.Clear();
                    currentWindSpeed.Append(xmlWind.SelectSingleNode("//WindSpeed").InnerText);

                    currentHumidity.Clear();
                    currentHumidity.Append(xmlAtmosphere.SelectSingleNode("//Humidity").InnerText);

                    currentLocation.Clear();
                    currentLocation.Append(WeatherLionMain.storedPreferences.StoredPreferences.Location);

                    sunriseTime.Clear();
                    sunriseTime.Append(xmlAstronomy.SelectSingleNode("//Sunrise").InnerText.ToUpper());

                    sunsetTime.Clear();
                    sunsetTime.Append(xmlAstronomy.SelectSingleNode("//Sunset").InnerText.ToUpper());
                }// end of try block 
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block 
            }// end of if block    	

            UpdateTemps(false); // call update temps here
            RunAstronomyCheck();

            // Some providers like Yahoo! loves to omit a zero on the hour mark example: 7:0 am
            if (sunriseTime.Length == 6)
            {
                string[] ft = sunriseTime.ToString().Split(':');
                sunriseTime.Clear();
                sunriseTime.Append(ft[0] + ":0" + ft[1]);
            }// end of if block
            else if (sunsetTime.Length == 6)
            {
                string[] ft = sunsetTime.ToString().Split(':');
                sunsetTime.Clear();
                sunsetTime.Append(ft[0] + ":0" + ft[1]);
            }// end if else if block

            UpdateWidgetControl(frmWeatherWidget.lblWeatherCondition, "Text",
               currentCondition.ToString());

            UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                 $"{currentWindDirection} {Math.Round(float.Parse(currentWindSpeed.ToString()))}" +
                 (useMetric ? " km/h" : " mph"));

            // Yr's Weather Service does not track humidity
            if (currentHumidity.ToString().Length == 0) currentHumidity.Append("0");

            currentHumidity = currentHumidity.ToString().Contains("%")
                ? new StringBuilder(currentHumidity.ToString().Replace("%", ""))
                : currentHumidity; // remove before parsing

            UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text",
                 Math.Round(float.Parse(currentHumidity.ToString())) +
                 (!currentHumidity.ToString().Contains("%") ? "%" : ""));           

            DateTime? timeUpdated = null;
            CultureInfo provider = CultureInfo.InvariantCulture;
            StringBuilder tz = new StringBuilder();
            string format = "ddd MMM dd HH:mm:ss zzz yyyy";
            StringBuilder xmlDate = new StringBuilder();

            try
            {
                xmlDate.Clear();
                xmlDate.Append(xmlProvider.SelectSingleNode("//Date").InnerText);

                // use the timezones hashtable to replace any possible timezone abbreviations.
                // C# requires time values instead of these abbreviations.
                foreach (string key in UtilityMethod.worldTimezonesOffsets.Keys)
                {
                    if (Regex.IsMatch(xmlDate.ToString(), $@"(^|\s){key}(\s|$)"))
                    {
                        tz.Clear();
                        tz.Append(xmlDate.ToString().Replace(key, (string)UtilityMethod.worldTimezonesOffsets[key]));
                        break;
                    }// end of if block                    
                }// end of for each loop               

                timeUpdated = DateTime.ParseExact(tz.ToString(), format, provider);
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            // Update the current location and update time stamp
            string ts = currentLocation.ToString().Contains(",")
                    ? currentLocation.ToString().Substring(0, currentLocation.ToString().IndexOf(","))
                        + ", " + string.Format("{0:ddd h:mm tt}", timeUpdated)
                    : currentLocation + ", "
                        + string.Format("{0:ddd h:mm tt}", timeUpdated);

            UpdateWidgetControl(frmWeatherWidget.btnLocation, "Text", ts);
            UpdateWidgetControl(frmWeatherWidget.btnSunrise, "Text", sunriseTime.ToString());
            UpdateWidgetControl(frmWeatherWidget.btnSunset, "Text", sunsetTime.ToString());
            
            // Load current condition weather image
            DateTime rightNow = DateTime.Now;
            DateTime rn = DateTime.Now; // date time right now (rn)
            DateTime? nf = null; // date time night fall (nf)
            DateTime? su = null; // date time sun up (su)                     

            try
            {
                string sunsetTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} {UtilityMethod.Get24HourTime(sunsetTime.ToString())}";
                string sunriseTwenty4HourTime = $"{rightNow.ToString("yyyy-MM-dd")} {UtilityMethod.Get24HourTime(sunriseTime.ToString())}";
                nf = Convert.ToDateTime(sunsetTwenty4HourTime);
                su = Convert.ToDateTime(sunriseTwenty4HourTime);
            } // end of try block
            catch (FormatException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block 

            string currentConditionIcon = null;

            if (rn == nf || rn > nf || rn < su)
            {
                if (currentCondition.ToString().ToLower().Contains("(night)"))
                {
                    currentConditionIcon = UtilityMethod.weatherImages[currentCondition].ToString();
                }// end of if block
                else
                {
                    if (currentCondition.ToString().ToLower().Contains("(night)"))
                    {
                        currentConditionIcon = UtilityMethod.weatherImages[currentCondition].ToString();
                    }// end of if block
                    else
                    {
                        if (UtilityMethod.weatherImages.ContainsKey($"{currentCondition.ToString().ToLower()} (night)"))
                        {
                            currentConditionIcon = UtilityMethod.weatherImages[$"{currentCondition.ToString().ToLower()} (night)"].ToString();
                        }// end of if block
                        else
                        {
                            currentConditionIcon = UtilityMethod.weatherImages[currentCondition.ToString().ToLower()].ToString();
                        }// end of else block
                    }// end of else block
                }// end of else block
            }// end of if block
            else
            {
                currentConditionIcon = UtilityMethod.weatherImages[currentCondition.ToString().ToLower()].ToString();
            }// end of else block

            currentConditionIcon = UtilityMethod.weatherImages[
                currentCondition.ToString().ToLower()] == null ?
                    "na.png" :
                    currentConditionIcon;

            // Load Icon sized 140 x 140 to current condition label
            UpdateWidgetControl(frmWeatherWidget.picCurrentConditions, "Image",
                $"{frmWeatherWidget.WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}/weather_{currentConditionIcon}");

            // Set image tooltip to current condition string
            UtilityMethod.AddControlToolTip(frmWeatherWidget.picCurrentConditions,
                currentCondition.ToString().ToProperCase());

            int x = 0;

            for (int i = 0; i < xmlForecastList.Count; i++)
            {
                x++;

                XmlElement wxDailyForecast = (XmlElement)xmlForecastList[i];

                string fCondition = wxDailyForecast.GetElementsByTagName("Condition")[0].InnerText.ToLower();

                foreach (string word in dWords)
                {
                    if (fCondition.ToString().Contains($" {word} "))
                    {
                        fCondition = fCondition.ToString().Substring(0, fCondition.ToString().IndexOf($" {word} ")).Trim();
                    }// end of if block                            
                }// end of for each block                        

                DateTime? forecastDate = null;
                string xmlForecastDate = wxDailyForecast.GetElementsByTagName("Date")[0].InnerText;

                try
                {
                    xmlDate.Clear();
                    xmlDate.Append(xmlForecastDate);

                    // use the timezones hashtable to replace any possible timezone abbreviations.
                    // C# requires time values instead of these abbreviations.
                    foreach (string key in UtilityMethod.worldTimezonesOffsets.Keys)
                    {
                        if (Regex.IsMatch(xmlDate.ToString(), $@"(^|\s){key}(\s|$)"))
                        {
                            tz.Clear();
                            tz.Append(xmlDate.ToString().Replace(key, (string)UtilityMethod.worldTimezonesOffsets[key]));
                            break;
                        }// end of if block                    
                    }// end of for each loop    

                    forecastDate = DateTime.ParseExact(tz.ToString(), format, provider);
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block                     

                UpdateWidgetControl(((Label)frmWeatherWidget.Controls.Find($"lblDay{i + 1}Day", true)[0]),
                    "Text", string.Format("{0:ddd d}", forecastDate));

                // Load current forecast condition weather image
                if (fCondition.ToString().ToLower().Contains("(day)"))
                {
                    fCondition = fCondition.ToString().Replace("(day)", "").Trim();
                }// end of if block
                else if (fCondition.ToString().ToLower().Contains("(night)"))
                {
                    fCondition = fCondition.ToString().Replace("(night)", "").Trim();
                }// end of if block

                string fConditionIcon
                    = UtilityMethod.weatherImages[fCondition.ToString().ToLower()] == null
                        ? "na.png" : (string)UtilityMethod.weatherImages[fCondition.ToString().ToLower()];

                UpdateWidgetControl((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i + 1}Image", true)[0],
                     "Image", $@"{WEATHER_IMAGE_PATH_PREFIX}{WeatherLionMain.storedPreferences.StoredPreferences.IconSet}\weather_{fConditionIcon}");
                
                // Set image tooltip to forecast condition string
                UtilityMethod.AddControlToolTip(((PictureBox)frmWeatherWidget.Controls.Find($"picDay{i + 1}Image", true)[0]),
                    fCondition.ToString().ToProperCase());

                currentFiveDayForecast.Add(
                        new FiveDayForecast((DateTime)forecastDate,
                                wxDailyForecast.GetElementsByTagName("HighTemperature")[0].InnerText,
                                wxDailyForecast.GetElementsByTagName("LowTemperature")[0].InnerText,
                                fCondition.ToString()));

                if (i == 4)
                {
                    break;
                }// end of if block            	
            }// end of for loop

            // Update the weather provider
            UpdateWidgetControl(frmWeatherWidget.btnWeatherProvider, "Text",
                xmlProvider.SelectSingleNode("//Name").InnerText);

            string providerIcon = $"res/assets/img/icons/{xmlProvider.SelectSingleNode("//Name").InnerText.ToLower()}.png";
            UpdateWidgetControl(frmWeatherWidget.btnWeatherProvider, "Image", $"{providerIcon}");

            if (UtilityMethod.refreshRequested)
            {
                UtilityMethod.refreshRequested = false;
            }// end of if block

            if (!frmWeatherWidget.Visible)
            {
                UpdateWidgetControl(frmWeatherWidget, "Visible", "true");
            }// end of if block

            frmWeatherWidget.usingPreviousData = true; // indicate that old weather data is being used
        }// end of method LoadPreviousWeatherData        

        /// <summary>
        /// Update the numerical values displayed on the widget
        /// </summary>
        /// <param name="hasConnection">hasConnection A <see cref="bool"/> value representing Internet Connectivity.</param>
        private void UpdateTemps(bool hasConnection)
        {
            StringBuilder today = new StringBuilder();
            int i;
            StringBuilder temps = new StringBuilder();
            StringBuilder fHigh = new StringBuilder();
            StringBuilder fLow = new StringBuilder();

            if (hasConnection)
            {
                switch (WeatherLionMain.storedPreferences.StoredPreferences.Provider)
                {
                    case WeatherLionMain.DARK_SKY:
                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(darkSky.currently.temperature))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(darkSky.currently.apparentTemperature))}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(darkSky.daily.data[0].temperatureMax))}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(UtilityMethod.CelsiusToFahrenheit(darkSky.daily.data[0].temperatureMin))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MphToKmh(darkSky.currently.windSpeed))}");
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(darkSky.currently.temperature)}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(darkSky.currently.apparentTemperature)}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(darkSky.daily.data[0].temperatureMax)}");

                            currentLow.Clear();
                            currentLow.Append($"{Math.Round(darkSky.daily.data[0].temperatureMin)}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(darkSky.currently.windSpeed)}");
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                            $"Current High Temp {currentHigh}{DEGREES}F");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                           $"Current Low Temp {currentLow}{DEGREES}F");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                        // Five Day Forecast
                        i = 1;
                        hl = new int[5, 2];

                        foreach (DarkSkyWeatherDataItem.Daily.Data wxForecast in darkSky.daily.data)
                        {
                            temps.Clear();
                            fHigh.Clear();
                            fLow.Clear();

                            if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                            {
                                fHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(wxForecast.temperatureMax))}");
                                fLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(wxForecast.temperatureMin))}");
                            }// end of if block
                            else
                            {
                                fHigh.Append($"{Math.Round(wxForecast.temperatureMax)}");
                                fLow.Append($"{Math.Round(wxForecast.temperatureMin)}");
                            }// end of else block
                            
                            temps.Append(string.Format("{0}° {1}°", fLow, fHigh));

                            hl[i - 1, 0] = int.Parse(fHigh.ToString());
                            hl[i - 1, 1] = int.Parse(fLow.ToString());

                            UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Temps", true)[0],
                                "Text", temps.ToString());

                            if (i == 5)
                            {
                                break;
                            }// end of if block

                            i++; // increment sentinel
                        }// end of for each loop

                        break;
                    case WeatherLionMain.HERE_MAPS:
                        double fl = float.Parse(hereWeatherWx.observations.location[0].observation[0].comfort);

                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append(
                                $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(hereWeatherWx.observations.location[0].observation[0].temperature)))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)fl))}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(hereWeatherWx.observations.location[0].observation[0].highTemperature)))}");

                            currentLow.Clear();
                            currentLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(hereWeatherWx.observations.location[0].observation[0].lowTemperature)))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append(
                                $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(hereWeatherWx.observations.location[0].observation[0].windSpeed)))}"
                            );
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append(
                                $"{Math.Round(float.Parse(hereWeatherWx.observations.location[0].observation[0].temperature))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round((float)fl)}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(float.Parse(hereWeatherWx.observations.location[0].observation[0].highTemperature))}");

                            currentLow.Clear();
                            currentLow.Append($"{Math.Round(float.Parse(hereWeatherWx.observations.location[0].observation[0].lowTemperature))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append(
                                $"{Math.Round(float.Parse(hereWeatherWx.observations.location[0].observation[0].windSpeed))}"
                            );
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                            $"Current High Temp {currentHigh}{DEGREES}F");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                           $"Current Low Temp {currentLow}{DEGREES}F");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                        // Five Day Forecast
                        List<HereMapsWeatherDataItem.ForecastData.DailyForecasts.ForecastLocation.Forecast> hFdf =
                                hereWeatherFx.dailyForecasts.forecastLocation.forecast;
                        i = 1;
                        hl = new int[5, 2];

                        foreach (HereMapsWeatherDataItem.ForecastData.DailyForecasts.ForecastLocation.Forecast wxForecast in hFdf)
                        {
                            temps.Clear();
                            fHigh.Clear();
                            fLow.Clear();

                            if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                            {
                                fHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxForecast.highTemperature)))}");
                                fLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxForecast.lowTemperature)))}");
                            }
                            else
                            {
                                fHigh.Append($"{Math.Round(float.Parse(wxForecast.highTemperature))}");
                                fLow.Append($"{Math.Round(float.Parse(wxForecast.lowTemperature))}");
                            }// end of else block

                            temps.Append(string.Format("{0}° {1}°", fLow, fHigh));

                            hl[i - 1, 0] = int.Parse(fHigh.ToString());
                            hl[i - 1, 1] = int.Parse(fLow.ToString());

                            UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Temps", true)[0],
                                "Text", temps.ToString());

                            if (i == 5)
                            {
                                break;
                            }// end of if block

                            i++; // increment sentinel
                        }// end of for each loop

                        break;
                    case WeatherLionMain.OPEN_WEATHER:
                        fl = UtilityMethod.HeatIndex(openWeatherWx.main.temp, openWeatherWx.main.humidity);

                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(openWeatherWx.main.temp))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)fl))}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(openWeatherFx.list[0].temp.max))}");

                            currentLow.Clear();
                            currentLow.Append($"{Math.Round(UtilityMethod.CelsiusToFahrenheit(openWeatherFx.list[0].temp.min))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MphToKmh(openWeatherWx.wind.speed))}");
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(openWeatherWx.main.temp)}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round((float)fl)}");

                            currentHigh.Clear();
                            currentHigh.Append($"{Math.Round(openWeatherFx.list[0].temp.max)}");

                            currentLow.Clear();
                            currentLow.Append($"{Math.Round(openWeatherFx.list[0].temp.min)}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(openWeatherWx.wind.speed)}");
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                            $"Current High Temp {currentHigh}{DEGREES}F");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                           $"Current Low Temp {currentLow}{DEGREES}F");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                        // Five Day Forecast
                        List<OpenWeatherMapWeatherDataItem.ForecastData.Data> oFdf = openWeatherFx.list;
                        i = 1;
                        hl = new int[5, 2];

                        foreach (OpenWeatherMapWeatherDataItem.ForecastData.Data wxForecast in oFdf)
                        {
                            temps.Clear();
                            fHigh.Clear();
                            fLow.Clear();

                            if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                            {
                                fHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(wxForecast.temp.max))}");
                                fLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius(wxForecast.temp.min))}");
                            }// end of if block
                            else
                            {
                                fHigh.Append($"{Math.Round(wxForecast.temp.max)}");
                                fLow.Append($"{Math.Round(wxForecast.temp.min)}");
                            }// end of else block

                            temps.Append(string.Format("{0}° {1}°", fLow, fHigh));

                            hl[i - 1, 0] = int.Parse(fHigh.ToString());
                            hl[i - 1, 1] = int.Parse(fLow.ToString());

                            UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Temps", true)[0],
                               "Text", temps.ToString());

                            if (i == 5)
                            {
                                break;
                            }// end of if block

                            i++; // increment sentinel
                        }// end of for each loop

                        break;
                    case WeatherLionMain.WEATHER_BIT:
                        fl = weatherBitWx.data[0].appTemp == 0
                            ? weatherBitWx.data[0].temp
                            : weatherBitWx.data[0].appTemp;

                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)fl))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)weatherBitWx.data[0].appTemp))}");

                            // not supplied by provider
                            currentHigh.Clear();
                            currentHigh.Append($"{0}");

                            // not supplied by provider
                            currentLow.Clear();
                            currentLow.Append($"{0}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MphToKmh(weatherBitWx.data[0].wind_spd))}");
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round((float)fl)}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round((float)weatherBitWx.data[0].appTemp)}");

                            // not supplied by provider
                            currentHigh.Clear();
                            currentHigh.Append($"{0}");

                            // not supplied by provider
                            currentLow.Clear();
                            currentLow.Append($"{0}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(weatherBitWx.data[0].wind_spd)}");
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                            $"Current High Temp {currentHigh}{DEGREES}F");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                           $"Current Low Temp {currentLow}{DEGREES}F");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                        // Five Day Forecast
                        List<WeatherBitWeatherDataItem.SixteenDayForecastData.Data> wFdf = weatherBitFx.data;
                        int count = wFdf.Count; // number of items in the array
                        double lowTemp = 0;
                        double highTemp = 0;
                        i = 1;
                        hl = new int[5, 2];

                        today.Clear();
                        today.Append(string.Format("{0:yyyy-MM-dd}", DateTime.Now));

                        foreach (WeatherBitWeatherDataItem.SixteenDayForecastData.Data wxForecast in wFdf)
                        {
                            temps.Clear();
                            string fxDate = null;

                            try
                            {                               
                                fxDate = string.Format("{0:yyyy-MM-dd}", wxForecast.datetime);
                            } // end of try block
                            catch (FormatException e)
                            {
                                UtilityMethod.LogMessage("severe", e.Message,
                                    $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                            }// end of catch block                             

                            if (fxDate.Equals(today))
                            {
                                currentHigh.Clear();
                                currentHigh.Append($"{Math.Round(wFdf[i].max_temp)}");

                                currentLow.Clear();
                                currentLow.Append($"{Math.Round(wFdf[i].min_temp)}");

                                UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text",
                                     (int.Parse(currentHigh.ToString()) > int.Parse(currentTemp.ToString().Replace("°F", ""))
                                     ? currentHigh.ToString() : int.Parse(currentTemp.ToString().Replace("°F", "")) + DEGREES));
                                UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text",
                                    $"{currentLow}{DEGREES}");

                                highTemp = wFdf[i].max_temp > double.Parse(currentTemp.ToString().Replace("°F", ""))
                                        ? wFdf[i].max_temp : double.Parse(currentTemp.ToString().Replace("°F", ""));
                            }// end of if block
                            else
                            {
                                highTemp = wFdf[i].max_temp;
                            }// end of else block

                            // this data that they provide is inaccurate but it will be used
                            lowTemp = wFdf[i].min_temp;

                            if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                            {
                                highTemp = Math.Round(UtilityMethod.FahrenheitToCelsius((float)highTemp));
                                lowTemp = Math.Round(UtilityMethod.FahrenheitToCelsius((float)lowTemp));
                            }// end of if block
                            else
                            {
                                highTemp = Math.Round(highTemp);
                                lowTemp = Math.Round(lowTemp);
                            }// end of else block

                            hl[i - 1, 0] = (int)highTemp;
                            hl[i - 1, 1] = (int)lowTemp;

                            fHigh.Clear();
                            fLow.Clear();

                            fHigh.Append($"{highTemp.ToString()}");
                            fLow.Append($"{lowTemp.ToString()}");

                            temps.Append(string.Format("{0}° {1}°", fLow, fHigh));

                            UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Temps", true)[0],
                                "Text", temps.ToString());

                            if (i == 5)
                            {
                                break;
                            }// end of if block

                            i++; // increment sentinel
                        }// end of for each loop

                        break;
                    case WeatherLionMain.YAHOO_WEATHER:
                        currentWindSpeed.Clear();
                        currentWindSpeed.Append($"{Math.Round(yahoo19.current_observation.wind.speed)}");

                        currentWindDirection.Clear();
                        currentWindDirection.Append(UtilityMethod.CompassDirection(
                                yahoo19.current_observation.wind.direction));

                        fl = UtilityMethod.HeatIndex(
                                yahoo19.current_observation.condition.temperature,
                                yahoo19.current_observation.atmosphere.humidity);

                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)yahoo19.current_observation.condition.temperature))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)fl))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MphToKmh(yahoo19.current_observation.wind.speed))}");
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round((float)yahoo19.current_observation.condition.temperature)}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round((float)fl)}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(yahoo19.current_observation.wind.speed)}");
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", $"{currentHumidity}%");

                        List<YahooWeatherYdnDataItem.Forecast> yFdf = yahoo19.forecasts;

                        hl = new int[5, 2];                        

                        for (i = 0; i <= yFdf.Count - 1; i++)
                        {
                            temps.Clear();
                            string fDate = string.Format("{0:yyyy-MM-dd}", UtilityMethod.GetDateTime(yFdf[i].date));
                            today.Clear();
                            today.Append(string.Format("{0:yyyy-MM-dd}", DateTime.Now));

                            string fh;
                            fLow.Clear();

                            if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                            {
                                if (fDate.Equals(today.ToString()))
                                {
                                    currentHigh.Clear();
                                    currentHigh.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)yFdf[i].high))}");

                                    currentLow.Clear();
                                    currentLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)yFdf[i].low))}");

                                    UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}{DEGREES}");
                                    UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}{DEGREES}");

                                    UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                                        $"Current High Temp {currentHigh}{DEGREES}F");
                                    UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                                        $"Current Low Temp {currentLow}{DEGREES}F");
                                }// end of if block

                                fh = $"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)yFdf[i].high))}";
                                fLow.Append($"{Math.Round(UtilityMethod.FahrenheitToCelsius((float)yFdf[i].low))}");
                                temps.Append(string.Format("{0}° {1}°", fLow, fh));

                            }// end of if block
                            else
                            {
                                if (fDate.Equals(today.ToString()))
                                {
                                    currentHigh.Clear();
                                    currentHigh.Append($"{(int)yFdf[i].high}");

                                    currentLow.Clear();
                                    currentLow.Append($"{(int)yFdf[i].low}");

                                    UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}{DEGREES}");
                                    UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}{DEGREES}");

                                    UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                                        $"Current High Temp {currentHigh}{DEGREES}F");
                                    UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                                        $"Current Low Temp {currentLow}{DEGREES}F");
                                }// end of if block

                                fh = $"{Math.Round(yFdf[i].high)}";
                                fLow.Append($"{Math.Round(yFdf[i].low)}");
                              
                                temps.Append(string.Format("{0}° {1}°", fLow, fh));
                            }// end of else block

                            hl[i, 0] = int.Parse(fh);
                            hl[i, 1] = int.Parse(fLow.ToString());

                            UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i + 1}Temps", true)[0],
                               "Text", temps.ToString());

                            if (i == 4)
                            {
                                break;
                            }// end of if block
                        }// end of for loop

                        break;
                    case WeatherLionMain.YR_WEATHER:
                        currentWindDirection.Clear();
                        currentWindDirection.Append(yrWeatherData.forecast[0].windDirCode);

                        if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{yrWeatherData.forecast[0].temperatureValue}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{yrWeatherData.forecast[0].temperatureValue}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MpsToKmh(yrWeatherData.forecast[0].windSpeedMps))}");
                        }// end of if block
                        else
                        {
                            currentTemp.Clear();
                            currentTemp.Append($"{Math.Round(UtilityMethod.CelsiusToFahrenheit(yrWeatherData.forecast[0].temperatureValue))}");

                            currentFeelsLikeTemp.Clear();
                            currentFeelsLikeTemp.Append($"{Math.Round(UtilityMethod.CelsiusToFahrenheit(yrWeatherData.forecast[0].temperatureValue))}");

                            currentWindSpeed.Clear();
                            currentWindSpeed.Append($"{Math.Round(UtilityMethod.MpsToMph(yrWeatherData.forecast[0].windSpeedMps))}");
                        }// end of else block

                        // Display weather data on widget
                        UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                        UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                            $"Current High Temp {currentHigh}{DEGREES}F");
                        UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                           $"Current Low Temp {currentLow}{DEGREES}F");
                        UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                           $"{currentWindDirection} {currentWindSpeed}" +
                           $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                        UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                        List<YrWeatherDataItem.Forecast> fdf = yrWeatherData.forecast;

                        // Five Day Forecast
                        i = 1;
                        //float fHigh = 0;    // forecasted high
                        //float fLow = 0;     // forecasted low
                        DateTime currentDate = new DateTime();
                        dailyReading = new Dictionary<string, float[,]>();
                        int x = 0;                        

                        // get the highs and lows from the forecast first
                        foreach (YrWeatherDataItem.Forecast wxTempReading in fdf)
                        {
                            x++;

                            if (x == 1)
                            {
                                currentDate = wxTempReading.timeFrom;
                                fHigh.Append((float)Math.Round(UtilityMethod.CelsiusToFahrenheit(wxTempReading.temperatureValue)));
                                fLow.Append((float)Math.Round(UtilityMethod.CelsiusToFahrenheit(wxTempReading.temperatureValue)));
                            }// end of if block

                            // monitor date change
                            if (string.Format("{0:MMMM dd, yyyy}", wxTempReading.timeFrom).Equals(string.Format("{0:MMMM dd, yyyy}", currentDate)))
                            {
                                float cr = (float)Math.Round(UtilityMethod.CelsiusToFahrenheit(wxTempReading.temperatureValue));

                                if (cr > float.Parse(fHigh.ToString()))
                                {
                                    fHigh.Clear();
                                    fHigh.Append(cr);
                                }// end of if block

                                if (cr < float.Parse(fLow.ToString()))
                                {
                                    fLow.Clear();
                                    fLow.Append(cr);
                                }// end of if block                     
                            }// end of if block

                            if (wxTempReading.timePeriod == 3)
                            {
                                x = 0;
                                float[,] hl = { { float.Parse(fHigh.ToString()), float.Parse(fLow.ToString()) } };
                                dailyReading.Add(string.Format("{0:MMMM dd, yyyy}", wxTempReading.timeFrom), hl);
                            }// end of if block
                        }// end of first for each loop 

                        x = 0;

                        // repeat the loop and store the five day forecast
                        foreach (YrWeatherDataItem.Forecast wxForecast in fdf)
                        {
                            x++;

                            string fDate = string.Format("{0:MMMM dd, yyyy}", wxForecast.timeFrom);

                            // the first time period is always the current reading for this moment
                            if (x == 1)
                            {
                                fHigh.Clear();
                                fLow.Clear();

                                fHigh.Append(dailyReading[string.Format("{0:MMMM dd, yyyy}", wxForecast.timeFrom)][0, 0]);
                                fLow.Append(dailyReading[string.Format("{0:MMMM dd, yyyy}", wxForecast.timeFrom)][0, 1]);

                                if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                                {
                                    fHigh.Append(Math.Round(
                                        UtilityMethod.CelsiusToFahrenheit(
                                            dailyReading[string.Format("{0:MMMM dd, yyyy}", wxForecast.timeFrom)][0, 0])));
                                    fLow.Append(Math.Round(
                                        UtilityMethod.CelsiusToFahrenheit(
                                            dailyReading[string.Format("{0:MMMM dd, yyyy}", wxForecast.timeFrom)][0, 1])));

                                    UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{fHigh}{DEGREES}");
                                    UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{fLow}{DEGREES}");

                                    temps.Clear();
                                    temps.Append(string.Format("{0}° {1}°", int.Parse(fLow.ToString()), int.Parse(fHigh.ToString())));

                                }// end of if block
                                else
                                {
                                    if (fDate.Equals(string.Format("{0:MMMM dd, yyyy}", new DateTime())))
                                    {
                                        currentHigh.Clear();
                                        currentHigh.Append($"{int.Parse(fHigh.ToString())}");

                                        currentLow.Clear();
                                        currentHigh.Append($"{int.Parse(fLow.ToString())}");

                                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}{DEGREES}");
                                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}{DEGREES}");
                                    }// end of if block

                                    temps.Clear();
                                    temps.Append(string.Format("{0}° {1}°", int.Parse(fLow.ToString()), int.Parse(fHigh.ToString())));
                                }// end of else block

                                UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i}Temps", true)[0],
                                    "Text", temps.ToString());

                                if (i == 5)
                                {
                                    break;
                                }// end of if block                   

                                i++; // increment sentinel
                            }// end of if block

                            if (wxForecast.timePeriod == 3)
                            {
                                x = 0;
                            }// end of if block  

                        }// end of second for each loop

                        break;
                    default:
                        break;
                }// end of switch block
            }// end of if block
            else // if there is no Internet connection
            {
                tempUnits = WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? CELSIUS : FAHRENHEIT;

                // get the root node of the XML document
                xmlCurrent = rootNode.SelectSingleNode("//Current");
                xmlForecast = rootNode.SelectSingleNode("//DailyForecast");
                xmlForecastList = rootNode.SelectNodes("//DailyForecast/DayForecast");

                // populate the global variables
                currentWindDirection.Clear();
                currentWindDirection.Append(xmlWind.SelectSingleNode("//WindDirection").InnerText);

                currentWindSpeed.Clear();
                currentWindSpeed.Append(xmlWind.SelectSingleNode("//WindSpeed").InnerText);

                currentHumidity.Clear();
                currentHumidity.Append(xmlAtmosphere.SelectSingleNode("//Humidity").InnerText);
                                
                currentTemp.Clear();
                currentTemp.Append(
                    $"{Math.Round(float.Parse(xmlCurrent.SelectSingleNode("//Temperature").InnerText))}");

                currentFeelsLikeTemp.Clear();
                currentFeelsLikeTemp.Append(
                    $"{Math.Round(float.Parse(xmlCurrent.SelectSingleNode("//FeelsLike").InnerText))}");

                currentHigh.Clear();
                currentHigh.Append(
                    $"{Math.Round(float.Parse(xmlCurrent.SelectSingleNode("//HighTemperature").InnerText))}");

                currentLow.Clear();
                currentLow.Append(
                    $"{Math.Round(float.Parse(xmlCurrent.SelectSingleNode("//LowTemperature").InnerText))}");

                currentWindSpeed.Clear();
                currentWindSpeed.Append(
                    $"{Math.Round(float.Parse(xmlWind.SelectSingleNode("//WindSpeed").InnerText))}");

                // Display weather data on widget
                UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "Text", $"{currentTemp}{tempUnits}{tempUnits}");
                UpdateWidgetControl(frmWeatherWidget.lblFeelsLike, "Text", $"{frmWeatherWidget.FEELS_LIKE} {currentFeelsLikeTemp.ToString()}{DEGREES}");
                UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text", $"{currentHigh}{DEGREES}");
                UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text", $"{currentLow}{DEGREES}");
                UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayHigh,
                    $"Current High Temp {currentHigh}{DEGREES}F");
                UtilityMethod.AddControlToolTip(frmWeatherWidget.lblDayLow,
                   $"Current Low Temp {currentLow}{DEGREES}F");
                UpdateWidgetControl(frmWeatherWidget.btnWindReading, "Text",
                   $"{currentWindDirection} {currentWindSpeed}" +
                   $" {(WeatherLionMain.storedPreferences.StoredPreferences.UseMetric ? " km/h" : " mph")}");
                UpdateWidgetControl(frmWeatherWidget.btnHumidity, "Text", currentHumidity.ToString());

                xmlForecastList = rootNode.SelectNodes("//DailyForecast/DayForecast");
                hl = new int[5, 2];

                CultureInfo provider = CultureInfo.InvariantCulture;
                StringBuilder tz = new StringBuilder();
                string format = "ddd MMM dd HH:mm:ss zzz yyyy";
                StringBuilder xmlDate = new StringBuilder();
                StringBuilder fh = new StringBuilder();

                for (i = 0; i <= xmlForecastList.Count; i++)
                {
                    fh.Clear();
                    temps.Clear();
                    XmlElement wxDailyForecast = (XmlElement)xmlForecastList[i];
                    DateTime? forecastDate = null;
                    string xmlForecastDate = wxDailyForecast.GetElementsByTagName("Date")[0].InnerText;

                    try
                    {
                        xmlDate.Clear();
                        xmlDate.Append(xmlForecastDate);

                        // use the timezones hashtable to replace any possible timezone abbreviations.
                        // C# requires time values instead of these abbreviations.
                        foreach (string key in UtilityMethod.worldTimezonesOffsets.Keys)
                        {
                            if (Regex.IsMatch(xmlDate.ToString(), $@"(^|\s){key}(\s|$)"))
                            {
                                tz.Clear();
                                tz.Append(xmlDate.ToString().Replace(key, (string)UtilityMethod.worldTimezonesOffsets[key]));
                                break;
                            }// end of if block                    
                        }// end of for each loop    

                        forecastDate = DateTime.ParseExact(tz.ToString(), format, provider);
                    }// end of try block
                    catch (Exception e)
                    {
                        UtilityMethod.LogMessage("severe", e.Message,
                            $"WidgetUpdateService::LoadPreviousWeatherData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                    }// end of catch block

                    string fDate = string.Format("{0:dd MMM yyyy}", forecastDate);
                    today.Clear();
                    today.Append(string.Format("{0:dd MMM yyyy}", DateTime.Now));

                    if (WeatherLionMain.storedPreferences.StoredPreferences.UseMetric)
                    {
                        UpdateWidgetControl(frmWeatherWidget.lblDayHigh, "Text",
                            $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxDailyForecast.SelectSingleNode("//HighTemperature").InnerText)))}{ DEGREES}");
                        UpdateWidgetControl(frmWeatherWidget.lblDayLow, "Text",
                            $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxDailyForecast.SelectSingleNode("//LowTemperature").InnerText)))}{ DEGREES}");

                        fh.Append(
                            $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxDailyForecast.SelectSingleNode("//HighTemperature").InnerText)))}"
                            );

                        fLow.Clear();
                        fLow.Append(
                            $"{Math.Round(UtilityMethod.FahrenheitToCelsius(float.Parse(wxDailyForecast.SelectSingleNode("//LowTemperature").InnerText)))}"
                        );

                        temps.Append(string.Format("{0}° {1}°", (int)float.Parse(fLow.ToString()), (int)float.Parse(fh.ToString())));

                    }// end of if block
                    else
                    {
                        fh.Append(wxDailyForecast.GetElementsByTagName("HighTemperature")[0].InnerText);

                        fLow.Clear();
                        fLow.Append(wxDailyForecast.GetElementsByTagName("LowTemperature")[0].InnerText);

                        if (fh.ToString().Equals(""))
                        {
                            fh.ToString().Equals("0");
                        }// end of if block
                        else if (fLow.ToString().Equals(""))
                        {
                            fLow.Clear();
                            fLow.Append("0");
                        }// end of if block

                        temps.Append(string.Format("{0}° {1}°", (int)float.Parse(fLow.ToString()), (int)float.Parse(fh.ToString())));
                    }// end of else block

                    hl[i, 0] = (int)float.Parse(fh.ToString());
                    hl[i, 1] = (int)float.Parse(fLow.ToString());

                    UpdateWidgetControl((Label)frmWeatherWidget.Controls.Find($"lblDay{i + 1}Temps", true)[0],
                        "Text", temps.ToString());

                    if (i == 4)
                    {
                        break;
                    }// end of if block
                }// end of for loop              
            }// end of else block 

            // Update the color of the temperature label
            UpdateWidgetControl(frmWeatherWidget.lblCurrentTemperature, "ForeColor", UtilityMethod.TemperatureColor(int.Parse(
                UtilityMethod.ReplaceAll(currentTemp.ToString(), "\\D", ""))).Name);          
        }// end of method UpdateTemps       

        /// <summary>
        /// Executes the <see cref="Thread"/>.
        /// </summary>
        public List<string> Run()
        {
            thread = new Thread(GetWeatherData);
            thread.Start();

            return null;
        }// end of method Run
    }// end of class WidgetUpdateService
}// end of namespace WeatherLion
