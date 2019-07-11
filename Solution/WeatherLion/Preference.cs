using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          Preference
///   Description:    This class is responsible accessing user
///                   preference data stored on the local machine.
///   Author:         Paul O. Patterson     Date: May 14, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Accesses user preference data stored on the local machine.
    /// </summary>
    public class Preference
    {
        private const string TAG = "Preference";
        private static ConfigurationData m_widget_config = new ConfigurationData();
        private static PreferencesData m_widget_preferences = new PreferencesData();
                
        public static readonly string SETTING_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"\res\settings\";
        public static readonly string CONFIG_FILE = $@"{SETTING_DIRECTORY}config.xml";
        public static readonly string PREFERENCE_FILE = $@"{SETTING_DIRECTORY}preferences.xml";

        public class ConfigurationData
        {
            public Point StartPosition { get; set; }// end of method StartPosition

            public ConfigurationData()
            {
            }// end of default constructor

            public ConfigurationData(Point position)
            {
                StartPosition = position;
            }// end of one-argument constructor
        }// end of class Config

        public class PreferencesData
        {
            public PreferencesData()
            {
            }// end of default constructor

            public PreferencesData(string provider, int interval, string location,
                bool useMetric, bool useSystemLocation, string widgetBackground, string iconSet)
            {
                Provider = provider;
                Interval = interval;
                Location = location;
                UseMetric = useMetric;
                UseSystemLocation = useSystemLocation;
                WidgetBackground = widgetBackground;
                IconSet = iconSet;
            }// end of five-argument constructor

            public string Provider { get; set; }       

            public int Interval { get; set; }

            public string Location { get; set; }

            public bool UseSystemLocation { get; set; }

            public bool UseMetric { get; set; }

            public string WidgetBackground { get; set; }

            public string IconSet { get; set; }
        }// end of class PreferencesData

        public ConfigurationData ConfigData
        {
            get { return m_widget_config; }
            set { m_widget_config = value; }
        }

        public PreferencesData StoredPreferences
        {
            get { return m_widget_preferences; }
            set { m_widget_preferences = value; }
        }
               
        public Preference()
        {
            GetConfigurationData();
            GetPreferencesData();
        }// end of default constructor        

        /// <summary>
        /// Creates the default configuration file for the program
        /// </summary>
        private static void CreateDefaultAppPreferences()
        {
            CreateDefaultUserPreferences();
            CreateDefaultAppConfiguration();
        }// end of method CreateDefaultAppPreferences

        /// <summary>
        /// 
        /// </summary>
        private void GetConfigurationData()
        {
            if (File.Exists(CONFIG_FILE))
            {
                StreamReader srReader = File.OpenText(CONFIG_FILE);
                Type tType = m_widget_config.GetType();
                XmlSerializer xsSerializer = new XmlSerializer(tType);
                object oData = xsSerializer.Deserialize(srReader);
                m_widget_config = (ConfigurationData)oData;
                srReader.Close();
            }// end of if block
        }// end of method GetConfigurationData

        /// <summary>
        /// 
        /// </summary>
        private void GetPreferencesData()
        {
            if (File.Exists(PREFERENCE_FILE))
            {
                try
                {
                    StreamReader srReader = File.OpenText(PREFERENCE_FILE);
                    Type tType = m_widget_preferences.GetType();
                    XmlSerializer xsSerializer = new XmlSerializer(tType);
                    object oData = xsSerializer.Deserialize(srReader);
                    m_widget_preferences = (PreferencesData)oData;
                    srReader.Close();
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::GetPreferencesData [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block
        }// end of method GetPreferencesData

        /// <summary>
        /// Creates the default user preferences file for the program
        /// </summary>
        private static void CreateDefaultUserPreferences()
        {
            // if the directory does not exist then no previous configuration exists
            if (!Directory.Exists(SETTING_DIRECTORY))
            {
                Directory.CreateDirectory(SETTING_DIRECTORY);

                try
                {
                    using (var stream = File.Create(PREFERENCE_FILE)) { };
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block	
            else if (!File.Exists(PREFERENCE_FILE))
            {
                try
                {
                    using (var stream = File.Create(PREFERENCE_FILE)) { };
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            try
            {
                using (StreamWriter swWriter = File.CreateText(PREFERENCE_FILE))
                {
                    m_widget_preferences = new PreferencesData(WeatherLionMain.authorizedProviders[0], 1800000,
                        "not set", false, false, "default", "miui");

                    Type tType = m_widget_preferences.GetType();
                    XmlSerializer xsSerializer = new XmlSerializer(tType);
                    xsSerializer.Serialize(swWriter, m_widget_preferences);
                }// end of using block
            }// end of try block
            catch (FileNotFoundException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
            catch (IOException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
        }// end of method CreateDefaultUserPreferences

        /// <summary>
        /// Create a default configuration file for the program
        /// </summary>
        private static void CreateDefaultAppConfiguration()
        {
            // if the directory does not exist then no previous configuration exists
            if (!Directory.Exists(SETTING_DIRECTORY))
            {
                Directory.CreateDirectory(SETTING_DIRECTORY);

                try
                {
                    using (var stream = File.Create(CONFIG_FILE)) { };
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block	
            else if (!File.Exists(CONFIG_FILE))
            {
                try
                {
                    using (var stream = File.Create(CONFIG_FILE)) { };
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            try
            {
                using (StreamWriter swWriter = File.CreateText(CONFIG_FILE))
                {
                    int screenHeight = Screen.PrimaryScreen.WorkingArea.Height / 2;
                    int screenWidth = Screen.PrimaryScreen.WorkingArea.Width / 2;

                    m_widget_config = new ConfigurationData(new Point(screenWidth, screenHeight));

                    Type tType = m_widget_config.GetType();
                    XmlSerializer xsSerializer = new XmlSerializer(tType);
                    xsSerializer.Serialize(swWriter, m_widget_config);
                }// end of using block
            }// end of try block
            catch (FileNotFoundException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
            catch (IOException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::CreatedefaultPreferencesPropertiesFile [line: " +
                        $"{UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
        }// end of method CreateDefaultAppConfiguration

        /// <summary>
        /// Stores a data to a specified to a local storage  
        /// </summary>
        /// <param name="dataFile">The file to store the data</param>
        public static void SaveProgramConfiguration(string dataFile, string pName, object pValue)
        {
            XmlDocument xmlDoc = null;
            XmlNode node = null;

            switch (dataFile.ToLower())
            {
                case "config":
                    // Store the program configuration data locally
                    xmlDoc = new XmlDocument();
                    
                    xmlDoc.Load(CONFIG_FILE);
                    node = xmlDoc.SelectSingleNode($"//{pName}");
                    XmlNode xNode = xmlDoc.SelectSingleNode($"//{pName}/X");
                    XmlNode yNode = xmlDoc.SelectSingleNode($"//{pName}/Y");
                    xNode.InnerText = ((Point)pValue).X.ToString();
                    yNode.InnerText = ((Point)pValue).Y.ToString();
                    xmlDoc.Save(CONFIG_FILE);
                    break;
                case "prefs":
                    // Store the user preferences data locally
                    xmlDoc = new XmlDocument();
                   
                    xmlDoc.Load(PREFERENCE_FILE);
                    node = xmlDoc.SelectSingleNode($"//{pName}");
                    node.InnerText = (string) pValue;
                    xmlDoc.Save(PREFERENCE_FILE);
                    break;
                default:
                    break;
            }// end of switch block           
        }// end of method SaveProgramConfiguration

        public static Preference GetSavedPreferences()
        {
            try
            {
                if (File.Exists(PREFERENCE_FILE))
                {
                    using (StreamReader srReader = File.OpenText(PREFERENCE_FILE))
                    {
                        Type tType = m_widget_preferences.GetType();
                        XmlSerializer xsSerializer = new XmlSerializer(tType);
                        object oData = xsSerializer.Deserialize(srReader);
                        m_widget_preferences = (PreferencesData)oData;
                    }// end of using block                        
                }// end of if block
                else
                {
                    // file does not exist so create the default one
                    CreateDefaultAppPreferences();
                }// end of else block
            }// end of try block
            catch (FileNotFoundException)
            {
                // file does not exist so create the default one
                CreateDefaultAppPreferences();

                try
                {
                    using (StreamReader srReader = File.OpenText(PREFERENCE_FILE))
                    {
                        Type tType = m_widget_preferences.GetType();
                        XmlSerializer xsSerializer = new XmlSerializer(tType);
                        object oData = xsSerializer.Deserialize(srReader);
                        m_widget_preferences = (PreferencesData)oData;
                    }// end of using block
                }// end of try block
                catch (FileNotFoundException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of try block
            }// end of try block
            catch (IOException ex)
            {
                UtilityMethod.LogMessage("severe", ex.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(ex)}]");
            }// end of try block            

            try
            {
                if (File.Exists(CONFIG_FILE))
                {
                    using (StreamReader srReader = File.OpenText(CONFIG_FILE))
                    {
                        Type tType = m_widget_config.GetType();
                        XmlSerializer xsSerializer = new XmlSerializer(tType);
                        object oData = xsSerializer.Deserialize(srReader);
                        m_widget_config = (ConfigurationData)oData;
                    }// end of using block                        
                }// end of if block
                else
                {
                    // file does not exist so create the default one
                    CreateDefaultAppConfiguration();
                }// end of else block
            }// end of try block
            catch (FileNotFoundException)
            {
                // file does not exist so create the default one
                CreateDefaultAppConfiguration();

                try
                {
                    using (StreamReader srReader = File.OpenText(CONFIG_FILE))
                    {
                        Type tType = m_widget_config.GetType();
                        XmlSerializer xsSerializer = new XmlSerializer(tType);
                        object oData = xsSerializer.Deserialize(srReader);
                        m_widget_config = (ConfigurationData)oData;
                    }// end of using block
                }// end of try block
                catch (FileNotFoundException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of try block
                catch (IOException e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of try block
            }// end of try block
            catch (IOException ex)
            {
                UtilityMethod.LogMessage("severe", ex.Message,
                        $"Preference::GetSavedPreferences [line: {UtilityMethod.GetExceptionLineNumber(ex)}]");
            }// end of try block 

            return new Preference();
        }// end of method GetSavedPreferences
    }// end of class Preference
}// end of namespace WeatherLion
