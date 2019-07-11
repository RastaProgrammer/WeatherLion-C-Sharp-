using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WeatherLionMain
///   Description:    This class is responsible for the main 
///                   execution of the program and to ensure that all
///                   required components and access is available
///                   before the program's launch.
///   Author:         Paul O. Patterson     Date: June 09, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// The 'real' main entry point of the program.
    /// </summary>
    public abstract class WeatherLionMain
    {
        public static SQLiteConnection conn = null;

        // local assets
        public static readonly string RES_PATH = $@"{AppDomain.CurrentDomain.BaseDirectory}\res\";
        public static readonly string ASSETS_PATH = $@"{RES_PATH}assets\";
        public static readonly string IMAGES_PATH = $@"{ASSETS_PATH}img\";
        public static readonly string MAIN_STORAGE_DIR = $@"{RES_PATH}storage\";
        public static readonly string DATA_DIRECTORY_PATH = $@"{MAIN_STORAGE_DIR}weather_data\";

        public const string PROGRAM_NAME = "Weather Lion";
        public const string MAIN_DATABASE_NAME = "WeatherLion.db";
        public const string CITIES_DATABASE_NAME = "WorldCities.db";
        public const string WAK_DATABASE_NAME = "wak.db";

        // local weather data file
        public const string WEATHER_DATA_XML = "WeatherData.xml";

        // preferences constants
        public const string WEATHER_SOURCE_PREFERENCE = "Provider";
        public const string UPDATE_INTERVAL = "Interval";
        public const string CURRENT_LOCATION_PREFERENCE = "Location";
        public const string USE_SYSTEM_LOCATION_PREFERENCE = "UseSystemLocation";
        public const string USE_METRIC_PREFERENCE = "UseMetric";
        public const string WIDGET_BACKGROUND_PREFERENCE = "WidgetBackground";
        public const string ICON_SET_PREFERENCE = "IconSet";

        // weather provider and web API constants
        public const string DARK_SKY = "Dark Sky Weather";
        public const string GEO_NAMES = "GeoNames";
        public const string HERE_MAPS = "Here Maps Weather";
        public const string OPEN_WEATHER = "Open Weather Map";
        public const string WEATHER_BIT = "Weather Bit";
        public const string YAHOO_WEATHER = "Yahoo! Weather";
        public const string YR_WEATHER = "Yr.no (Norwegian Metrological Institute)";
        public const string WEATHER_UNDERGROUND = "Weather Underground";
        public static string[] providerNames = new string[] {
                DARK_SKY, GEO_NAMES, HERE_MAPS, OPEN_WEATHER, WEATHER_BIT, YAHOO_WEATHER, YR_WEATHER };

        public static string[] authorizedProviders;

        // Default program icon set name
        public const string DEFAULT_ICON_SET = "miui";
        public static readonly string WEATHER_ICONS_PATH = $@"{ASSETS_PATH}img\weather_images\";
        public static readonly string WIDGET_BACKGROUNDS_PATH = $@"{IMAGES_PATH}backgrounds\";
        public static readonly string WIDGET_ICONS_PATH = $@"{IMAGES_PATH}icons\";
        public static string iconSet = null; // To be updated

        // Default Icon Set Images
        public static readonly string DEFAULT_ICON_IMAGE = $@"{WEATHER_ICONS_PATH}{DEFAULT_ICON_SET}\weather_10.png";
        public static readonly string COLOR_ICON_IMAGE = $@"{WEATHER_ICONS_PATH}color\weather_10.png";
        public static readonly string IKONO_ICON_IMAGE = $@"{WEATHER_ICONS_PATH}ikono\weather_10.png";
        public static readonly string MONO_ICON_IMAGE = $@"{WEATHER_ICONS_PATH}mono\display_icon.png";

        // Default Background Icon Images
        public static readonly string DEFAULT_BACKGROUND_IMAGE = $@"{WIDGET_BACKGROUNDS_PATH}default_bg.png";
        public static readonly string ANDROID_BACKGROUND_IMAGE = $@"{WIDGET_BACKGROUNDS_PATH}android_bg.png";
        public static readonly string RABALAC_BACKGROUND_IMAGE = $@"{WIDGET_BACKGROUNDS_PATH}rabalac_bg.png";

        // Other Images
        public static readonly string LOADING_IMAGE = $@"{WIDGET_ICONS_PATH}loading.gif";

        // the default interval will be 30 mins of 1800000 ms
        public static readonly int DEFAULT_WIDGET_UPDATE_INTERVAL = 1800000;

        // system location
        public static string systemLocation;

        // stored preferences
        public static Preference storedPreferences = new Preference();
        
        // miscellaneous
        public static CityData currentCity;
        
        // track whether the data was received from the provider or
        // locally due to Internet connectivity
        public static bool weatherLoadedFromProvider;
        public static bool noAccessToStoredProvider;

        public static List<string> iconPackList = new List<string>();
        public static Hashtable iconSetControls;
        public static bool iconPacksLoaded;

        // instance of all the forms to be accessed declarations
        public static PreferencesForm preferences = new PreferencesForm();
        public static AccessKeysForm keys = new AccessKeysForm();
        public static WidgetForm runningWidget = null;
        public static bool connectedToInternet = false;

        // pending preference update
        public static List<string> pendingUpdates = new List<string>();

        private const string TAG = "WeatherLionMain";

        /// <summary>
        /// Load a required assets and prepare for program execution
        /// </summary>
        public static void Launch()
        {
            #region WeatherLion launch sequence

            UtilityMethod.LogMessage("info", "Initiating startup...", "WeatherLionMain::Launch");

            // build the required storage files
            if (BuildRequiredDatabases() == 1)
            {
                UtilityMethod.LogMessage("info",
                        "All required databases constructed successfully.",
                        "WeatherLionMain::Launch");
            }// end of if block
            else
            {
                UtilityMethod.LogMessage("severe",
                        "All required databases were not constructed successfully.",
                        "WeatherLionMain::Launch");
            }// end of else block

            // check that the required access keys are available
            LionSecurityManager.Init();

            // Load only the providers who have access keys assigned to them
            List<string> wxOnly = LionSecurityManager.webAccessGranted;

            wxOnly.Sort();  // sort the list

            // GeoNames is not a weather provider so it cannot be select here
            if (wxOnly.Contains("GeoNames")) wxOnly.Remove("GeoNames");

            authorizedProviders = wxOnly.ToArray();
            
            // ensure that program has all the default assets needed for functioning properly
            HealthCheck();

            // load user preferences
            storedPreferences = Preference.GetSavedPreferences();

            string previousWeatherData = $"{DATA_DIRECTORY_PATH}{WEATHER_DATA_XML}";

            connectedToInternet = UtilityMethod.HasInternetConnection();

            // check for an Internet connection or previous weather data stored local
            if (!connectedToInternet && !File.Exists(previousWeatherData))
            {
                UtilityMethod.ShowMessage("The program will not run without a working internet connection or "
                        + "data that was previously stored locally" +
                        "\nResolve your Internet connection and relaunch the program.", null);

                Application.Exit(); // terminate the program
            }// end of if block
            else if (connectedToInternet)
            {
                // obtain the current city of the connected Internet service
                currentCity = UtilityMethod.GetSystemLocation();

                if (currentCity != null)
                {
                    if (currentCity.regionCode != null)
                    {
                        systemLocation = $"{currentCity.cityName}, {currentCity.regionCode}";
                    }// end of if block
                    else
                    {
                        systemLocation = $"{currentCity.cityName}, {currentCity.countryName}";
                    }// end of else block
                }// end of if block

                // if the user requires the current detected city location to be used as default
                if (storedPreferences.StoredPreferences.UseSystemLocation)
                {
                    if (systemLocation != null)
                    {
                        // use the detected city location as the default
                        storedPreferences.StoredPreferences.Location = systemLocation;

                        if (!storedPreferences.StoredPreferences.Location.Equals(systemLocation))
                        {
                            // update the preferences file
                            Preference.SaveProgramConfiguration("prefs", "Location", systemLocation);

                            // save the city to the local WorldCites database
                            UtilityMethod.AddCityToDatabase(
                                    currentCity.cityName, currentCity.countryName,
                                    currentCity.countryCode, currentCity.regionName,
                                    currentCity.regionCode, currentCity.latitude,
                                    currentCity.longitude);

                            JSONHelper.ExportToJSON(currentCity);
                            XMLHelper.ExportToXML(currentCity);
                        }// end of if block
                    }// end of if block
                    else
                    {
                        UtilityMethod.ShowMessage("The program was unable to obtain your system's location."
                                + "\nYour location will have to be set manually using the preferences dialog.", null);

                        PreferencesForm pf = new PreferencesForm();
                        pf.Show();
                    }// end of else block
                }// end of if block
            }// end of else if block

            Init();

            #endregion
        }// end of method Launch

        private static int AttachDatabase(string dbName, string alias)
        {
            string attachSQL = $"ATTACH DATABASE '{dbName}' as {alias}";

            try
            {
                //conn.Open();

                using (SQLiteCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = attachSQL;
                    IDataReader dr = comm.ExecuteReader();
                    return 1;
                }// end of using block

            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                         $"WeatherLionMain::AttachDatabase [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                return 0;
            }// end of catch block            
        }// end of method AttachDatabase

        /// <summary>
        /// Build all required databases that the program will use.
        /// </summary>
        /// <returns></returns>
        public static int BuildRequiredDatabases()
        {
            string mainStorageFile = $"{MAIN_STORAGE_DIR}{MAIN_DATABASE_NAME}";
            string cityStorageFile = $"{MAIN_STORAGE_DIR}{CITIES_DATABASE_NAME}";
            string wakStorageFile = $"{MAIN_STORAGE_DIR}{WAK_DATABASE_NAME}";
            StringBuilder keySuccess = new StringBuilder();
            StringBuilder citySuccess = new StringBuilder();

            int success = 0;

            // create all necessary files if they are not present
            if (!Directory.Exists(MAIN_STORAGE_DIR))
            {
                Directory.CreateDirectory(MAIN_STORAGE_DIR);
            }// end of if block

            if (!File.Exists(mainStorageFile))
            {
                try
                {
                    SQLiteConnection.CreateFile(mainStorageFile);
                }// end of try black 
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::BuildRequiredDatabases [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            if (!File.Exists(cityStorageFile))
            {
                try
                {
                    SQLiteConnection.CreateFile(cityStorageFile);
                    citySuccess.Append("World cities database successfully created");
                }// end of try black 
                catch (IOException e)
                {

                    UtilityMethod.LogMessage("severe", e.Message,
                        $"WeatherLionMain::BuildRequiredDatabases [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            if (!File.Exists(wakStorageFile))
            {
                try
                {
                    SQLiteConnection.CreateFile(wakStorageFile);
                    keySuccess.Append("Weather access database successfully created");
                }// end of try black 
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::BuildRequiredDatabases [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            if (File.Exists(mainStorageFile) && File.Exists(cityStorageFile) && File.Exists(wakStorageFile))
            {
                UtilityMethod.LogMessage("info", "The required storage files are present.",
                    $"{TAG}::BuildRequiredDatabases");

                // Establish connection with the databases and open it
                if (conn == null) conn = ConnectionManager.GetInstance().GetConnection();

                try
                {
                    conn.Open();
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::BuildRequiredDatabases [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block

            }// end of if block
            else
            {
                UtilityMethod.LogMessage("severe", "All the required storage files are not present.",
                    $"{TAG}::BuildRequiredDatabases");
                return 0;
            }// end of else block

            // attach required databases to the main database file
            if (AttachDatabase(wakStorageFile, "wak") == 1)
            {
                if (!UtilityMethod.CheckIfTableExists("wak", "access_keys"))
                {
                    UtilityMethod.CreateWSADatabase();
                }// end of if block

                success = 1;
                if (keySuccess.Length > 0)
                {
                    keySuccess.Append(" and attached to main connection");
                }// end of if block
                else
                {
                    keySuccess.Append("Weather access database attached to main connection");
                }// end of else block              
            }// end of if block
            else
            {
                success = 0;
            }// end of else block

            if (AttachDatabase(cityStorageFile, "WorldCities") == 1)
            {
                if (!UtilityMethod.CheckIfTableExists("WorldCities", "world_cities"))
                {
                    UtilityMethod.CreateWorldCitiesDatabase();
                }// end of if block

                success = 1;

                if (citySuccess.Length > 0)
                {
                    citySuccess.Append(" and attached to main connection");
                }// end of if block
                else
                {
                    citySuccess.Append("WorldCities database attached to main connection");
                }// end of else block
            }// end of if block
            else
            {
                success = 0;
            }// end of else block

            return success;
        }// end of method BuildRequiredDatabases

        private static void Init()
        {
            if (!LocationCheck())
            {
                UtilityMethod.ShowMessage("The program will not run without a location set.\n"
                    + "Enjoy the weather!", null, title: $"{PROGRAM_NAME} - Setup");

                Application.Exit(); // terminate the program
            }// end of if block
            else
            {
                UtilityMethod.LogMessage("info", "Necessary requirements met...",
                        $"{TAG}::Init");
                UtilityMethod.LogMessage("info", "Launching Weather Widget...",
                        $"{TAG}::Init");
            }// end of else block
        }// end of method Init

        private static void HealthCheck()
        {
            // the program CANNOT RUN with the assets directory
            if (!Directory.Exists(ASSETS_PATH))
            {
                UtilityMethod.MissingRequirementsPrompt("Missing Assets Directory");
            }// end of if block
            else
            {
                UtilityMethod.subDirectoriesFound.Clear(); // clear any previous list

                List<string> iconPacks = UtilityMethod.GetSubdirectories(WEATHER_ICONS_PATH);

                if (iconPacks == null || iconPacks.Count == 0)
                {
                    UtilityMethod.MissingRequirementsPrompt("Empty Assets Directory");
                }// end of if block
                else
                {
                    UtilityMethod.LogMessage("info",
                        "Found " + iconPacks.Count + " icon " +
                            (iconPacks.Count > 1 ? "packs..." : "pack..."),
                            $"{TAG}::HealthCheck");

                    if (!iconPacks.Contains(DEFAULT_ICON_SET))
                    {
                        UtilityMethod.MissingRequirementsPrompt("Missing Default Icons");
                    }// end of if block
                    else if (!iconPacks.Contains(Preference.GetSavedPreferences().StoredPreferences.IconSet))
                    {
                        UtilityMethod.LogMessage("warning",
                            $"The {storedPreferences.StoredPreferences.IconSet.ToUpper()}" +
                            $" icon pack could not be found so the default {DEFAULT_ICON_SET.ToUpper()}" +
                            " will be used!", $"{TAG}::HealthCheck");
                        
                        Preference.SaveProgramConfiguration("prefs", "IconSet", "default");
                    }// end of else if block
                    else
                    {
                        string iconsInUse =
                                $"{WEATHER_ICONS_PATH}{storedPreferences.StoredPreferences.IconSet}/";
                        int imageCount = UtilityMethod.GetFileCount(iconsInUse);

                        if (imageCount < 23)
                        {
                            UtilityMethod.MissingRequirementsPrompt("Insufficient Icon Count");
                        }// end of if block
                        else
                        {
                            UtilityMethod.LogMessage("info", $"Found {imageCount}" +
                                (imageCount > 1 ? " images" : " image") + " in the " +
                                UtilityMethod.ToProperCase(storedPreferences.StoredPreferences.IconSet) +
                                " icon pack...", $"{TAG}::HealthCheck");
                        }// end of else block

                        // check for the background and icon  images                       

                        if (!Directory.Exists(WIDGET_BACKGROUNDS_PATH))
                        {
                            UtilityMethod.MissingRequirementsPrompt("Missing Background Image Directory");
                        }// end of if block
                        else
                        {
                            imageCount = UtilityMethod.GetFileCount(WIDGET_BACKGROUNDS_PATH);

                            if (imageCount < 3)
                            {
                                UtilityMethod.MissingRequirementsPrompt(imageCount > 1 ? "Missing Background Images" :
                                    "Missing Background Image");
                            }// end of if block
                            else
                            {
                                UtilityMethod.LogMessage("info",
                                    "Found " + imageCount + (imageCount > 1 ? " images" : " image")
                                    + " in the backgrounds directory...", $"{TAG}::HealthCheck");
                            }// end of else block
                        }// end of else block

                        if (!Directory.Exists(WIDGET_ICONS_PATH))
                        {
                            UtilityMethod.MissingRequirementsPrompt("Missing Background Image Directory");
                        }// end of if block
                        else
                        {
                            imageCount = UtilityMethod.GetFileCount(WIDGET_ICONS_PATH);

                            if (imageCount < 11)
                            {
                                UtilityMethod.MissingRequirementsPrompt(imageCount > 1 ? "Missing Icon Images" :
                                    "Missing Icon Image");
                            }// end of if block
                            else
                            {
                                UtilityMethod.LogMessage("info",
                                    "Found " + imageCount +
                                    (imageCount > 1 ? " images" : " image") +
                                    " in the icons directory...",
                                    $"{TAG}::HealthCheck");
                            }// end of else block
                        }// end of else block
                    }// end of else block
                }// end of else block
            }// end of else block
        }// end of method HealthCheck

        public static bool LocationCheck()
        {
            bool locationSet = false;

            if (storedPreferences.StoredPreferences.Location.Equals("not set"))
            {
                DialogResult setCurrentCity;

                if (systemLocation != null)
                {
                    string prompt = "You must specify a current location in order to run the program. " +
                            $"Your current location is detected as {systemLocation}.\n" +
                            "Would you like to use it as your current location?";

                    DialogResult useSystemLocation = UtilityMethod.ResponseBox(prompt, PROGRAM_NAME + " - Setup");

                    if (useSystemLocation == DialogResult.Yes)
                    {
                        storedPreferences.StoredPreferences.UseSystemLocation = true;
                        storedPreferences.StoredPreferences.Location = systemLocation;
                                              
                        Preference.SaveProgramConfiguration("prefs", "UseSystemLocation", "true");
                        Preference.SaveProgramConfiguration("prefs", "Location", systemLocation);

                        // save the city to the local WorldCites database
                        UtilityMethod.AddCityToDatabase(
                                currentCity.cityName, currentCity.countryName, currentCity.countryCode,
                                currentCity.regionName, currentCity.regionCode, currentCity.latitude,
                                currentCity.longitude);

                        JSONHelper.ExportToJSON(currentCity);
                        XMLHelper.ExportToXML(currentCity);

                        locationSet = true;
                        Application.Run(new WidgetForm());
                    }// end of if block
                    else
                    {
                        prompt = "You must specify a current location in order to run the program.\n" +
                                            "Would you like to specify it now?";

                        setCurrentCity = UtilityMethod.ResponseBox(prompt, PROGRAM_NAME + " - Setup");

                        if (setCurrentCity == DialogResult.Yes)
                        {
                            preferences.ShowDialog();

                            // loop until a city is selected
                            while (PreferencesForm.locationSelected)
                            {
                                UtilityMethod.LogMessage("warning", "Waiting for location to be set!", 
                                    $"{TAG}::LocationCheck");
                            }// end of while loop

                            locationSet = PreferencesForm.locationSelected;
                        }// end of if block		
                    }// end of else block
                }// end of if block
                else
                {
                    setCurrentCity = UtilityMethod.ResponseBox("You must specify a current location in order to run the program.\n"
                            + "Would you like to specify it now?",
                            WeatherLionMain.PROGRAM_NAME + " Setup");

                    if (setCurrentCity == DialogResult.Yes)
                    {
                        preferences.Show();

                        // loop until a city is selected
                        while (!PreferencesForm.locationSelected)
                        {
                            Console.WriteLine("Waiting for location to be set!");
                        }// end of while loop

                        locationSet = PreferencesForm.locationSelected;
                    }// end of if block		
                    else
                    {
                        UtilityMethod.ShowMessage("The program will not run without a location set.\nGoodbye.", null,
                                       title: $"{WeatherLionMain.PROGRAM_NAME}", buttons: MessageBoxButtons.OK,
                                       mbIcon: MessageBoxIcon.Information);
                        Application.Exit();     //Exit the application.						
                    }// end of else block
                }// end of else block
            }// end of if block
            else
            {
                // the location was already set
                locationSet = true;
            }// end of else block

            return locationSet;
        }// end of method LocationCheck
    }// end of class WeatherLionMain
}// end of namespace WeatherLion
