using System;
using System.Security.Cryptography;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          LionSecurityManager
///   Description:    This class is responsible for encryption and
///                   decryption functionality.
///   Author:         Paul O. Patterson     Date: May 14, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// <para>This class is responsible for encryption and decryption functionality thus allowing
    /// the program to hide sensitive access within a local database storage.</para>There are more
    /// robust ways of carrying out these security functionality so, this is just a simple 
    /// approach for this home application.
    /// </summary>
    public class LionSecurityManager
    {
        private const string TAG = "LionSecurityManager";
        public static readonly string[] darkSkyRequiredKeys = new string[] { "api_key" };
        public static readonly string[] geoNamesRequiredKeys = new string[] { "username" };
        public static readonly string[] hereMapsRequiredKeys = new string[] { "app_id", "app_code" };
        public static readonly string[] openWeatherMapRequiredKeys = new string[] { "api_key" };
        public static readonly string[] weatherBitRequiredKeys = new string[] { "api_key" };
        public static readonly string[] yahooRequiredKeys = new string[] { "app_id", "consumer_key", "consumer_secret" };

        public static string databasePath = AppDomain.CurrentDomain.BaseDirectory + @"res/storage/";        
        public static string databaseFile = databasePath + WeatherLionMain.WAK_DATABASE_NAME;

        public static readonly string announcement = "Weather Lion is designed to consume data from web services such as:\n"
                                            + "Dark Sky Weather, Here Maps, Yahoo! Weather, Open Weather Map, Weather Bit, Weather Underground,"
                                            + "and Geonames.\nIn order for the program to us any of these providers, you must acquire key from each"
                                            + " services provider.\n\nThe following URLs can be used to obtain access to the websites:\n"
                                            + "   1. Dark Sky: https://darksky.net/dev\n"
                                            + "   2. Geonames: http://www.geonames.org/\n"
                                            + "   3. Here Maps: https://developer.here.com/\n"
                                            + "   4. Open Weather Map: https://openweathermap.org/api\n"
                                            + "   5. Weather Bit: https://www.weatherbit.io/api\n"
                                            + "   6. Yahoo Weather: https://developer.yahoo.com/weather/\n"
                                            + "\nThe program will be able to display weather from Yr.no (Norwegian Meteorological Institute) as they don't require a key.\n"
                                            + "\n**Access must be supplied for any the specified weather providers and a username to use "
                                            + "the geonames website http://www.geonames.org/ for city search.";

        public static List<string> keysMissing;
        public static List<string> webAccessGranted;

        // this flag will inform the program that valid access keys have been loaded
        public static bool accessGranted = false;

        // the program utilizes the services of 
        private static bool geoNamesAccountLoaded;

        private static AccessKeysForm addKeys = new AccessKeysForm();              

        /**
	    * No argument, default constructor
	    */
        public LionSecurityManager()
        {
        }// end of default constructor

        public static void Init()
        {
            // create all necessary files if they are not present
            if (!Directory.Exists(databasePath))
            {
                try
                {
                    Directory.CreateDirectory(databasePath);                
                    File.Create(databaseFile);                    
                }// end of try black 
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                         $"{TAG}::init [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block

            if (!File.Exists(databaseFile))
            {
                try
                {
                    File.Create(databaseFile);
                    UtilityMethod.CreateWSADatabase();
                    addKeys.Show();
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::init [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                }// end of catch block
            }// end of if block
            else if (!UtilityMethod.CheckIfTableExists("wak", "access_keys"))
            {
                UtilityMethod.CreateWSADatabase();
                UtilityMethod.ShowMessage(announcement, addKeys, WeatherLionMain.PROGRAM_NAME + " - IMPORTANT", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }// end of else if block

            LoadAccessProviders();
        }// end of method init

        /// <summary>
        /// Checks to see if any provider stored in the database is missing a key
        /// that is required.
        /// </summary>
        /// <returns></returns>
        public static DialogResult CheckForMissingKeys()
        {
            string mks = string.Join(", ", keysMissing);
            string fMks = null;

            if (UtilityMethod.NumberOfCharacterOccurences(',', mks) > 1)
            {
                fMks = UtilityMethod.ReplaceLast(",", ", and", mks);
            }// end of if block
            else if (UtilityMethod.NumberOfCharacterOccurences(',', mks) == 1)
            {
                fMks = mks.Replace(",", " and");
            }// end of else block
            else
            {
                fMks = mks;
            }// end of else block

            string prompt = "Yahoo! Weather requires the following missing " +
                            (keysMissing.Count > 1 ? "keys" : "key") + ":\n"
                            + fMks + "\nDo you wish to add " +
                            (keysMissing.Count > 1 ? "them" : "it") + " now?";
            DialogResult result = UtilityMethod.ResponseBox(prompt, WeatherLionMain.PROGRAM_NAME + " - Add Missing Key");

            return result;
        }// end of message CheckForMissingKeys

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra security</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);            

            string hexKey = GetUniqueKey(100);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hexKey));
                hashmd5.Clear();
            }// end of if block
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(hexKey);
            }// end of else block

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length) + ":" + hexKey;
        }// end of method Encrypt

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, string hexKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hexKey));
                hashmd5.Clear();
            }// end of if block
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(hexKey);
            }// end of else block

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = null;

            try
            {
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::Decrypt [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }// end of method Decrypt

        /// <summary>
        /// Generates a cryptographically sound alpha numeric string
        /// <para>
        /// Code retrieved from : <seealso cref="https://stackoverflow.com/a/1344255/72350"/>
        /// author: Eric J.
        /// </para>
        /// </summary>
        /// <param name="size">The maximum size in <see cref="byte"/>s that the string should be.</param>
        /// <returns></returns>
        public static string GetUniqueKey(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }// end of using block

            StringBuilder result = new StringBuilder(size);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }// end of for each block

            return result.ToString();
        }// end of method GetUniqueKey

        /// <summary>
        /// Retrieves an encrypted access key from a local SQLite 3 database
        /// </summary>
        /// <param name="keyProvider"> The name of the web service that supplies the key</param>
        /// <remarks></remarks>
        /// <returns>An {@code ArrayList} containing the keys assigned to the specified provider</returns>
        public static ArrayList GetSiteKeyFromDatabase(string keyProvider)
        {
            string SQL = null;
            ArrayList ak = new ArrayList();
            int found = 0;

            if (keyProvider != null)
            {
                SQL = $"SELECT KeyName, KeyValue, Hex FROM wak.access_keys WHERE KeyProvider = @provider";
            }// end of if block

            try
            {
                using (SQLiteCommand comm = WeatherLionMain.conn.CreateCommand())
                {
                    comm.CommandText = $"SELECT KeyName, KeyValue, Hex FROM wak.access_keys WHERE KeyProvider = @provider";
                    comm.Parameters.Add("@provider", DbType.String);
                    comm.Parameters["@provider"].Value = keyProvider;
                    IDataReader dr = comm.ExecuteReader();

                    while (dr.Read())
                    {
                        ak.Add(dr.GetString(0) + ":" + dr.GetString(1) + ":" + dr.GetString(2));
                        found++;
                    }// end of while loop
                }// end of using block

                if (found > 0)
                {
                    return ak;
                }// end of if block
                else
                {
                    return null;
                }// end of else block
            }// end of try block
            catch (SQLiteException)
            {
                return null;
            }// end of catch block            
        }// end of method GetSiteKeyFromDatabase

        /**
	    * Load all access providers stored in the database
	    */
        public static void LoadAccessProviders()
        {
            ArrayList appKeys = new ArrayList();
            webAccessGranted = new List<string>();

            try
            {
                foreach (string provider in WeatherLionMain.providerNames)
                {
                    appKeys = GetSiteKeyFromDatabase(provider);

                    if (appKeys != null)
                    {
                        switch (provider)
                        {
                            case "Dark Sky Weather":

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)darkSkyRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        WidgetUpdateService.darkSkyApiKey = Decrypt(kv[1], kv[2], true);
                                    }// end of if block
                                }// end of for each loop

                                if (WidgetUpdateService.darkSkyApiKey != null)
                                {
                                    webAccessGranted.Add("Dark Sky Weather");
                                    UtilityMethod.LogMessage("info",
                                        "Dark Sky key loaded!", "LionSecurityManager::LoadAccessProviders");
                                }// end of if block

                                break;

                            case "GeoNames":

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)geoNamesRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        WidgetUpdateService.geoNameAccount = Decrypt(kv[1], kv[2], true);
                                    }// end of if block                               
                                }// end of for each loop

                                if (WidgetUpdateService.geoNameAccount != null)
                                {
                                    webAccessGranted.Add("GeoNames");
                                    geoNamesAccountLoaded = true;
                                    UtilityMethod.LogMessage("info",
                                        "GeoNames user account loaded!", $"{TAG}::LoadAccessProviders");
                                }// end of if block

                                break;
                            case "Open Weather Map":

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)openWeatherMapRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        WidgetUpdateService.openWeatherMapApiKey = Decrypt(kv[1], kv[2], true);
                                    }// end of if block                               
                                }// end of for each loop

                                if (WidgetUpdateService.openWeatherMapApiKey != null)
                                {
                                    webAccessGranted.Add("Open Weather Map");
                                    UtilityMethod.LogMessage("info",
                                        "Open Weather Map key loaded!", $"{TAG}::LoadAccessProviders");
                                }// end of if block

                                break;
                            case "Weather Bit":

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)weatherBitRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        WidgetUpdateService.weatherBitApiKey = Decrypt(kv[1], kv[2], true);
                                    }// end of if block                               
                                }// end of for each loop

                                if (WidgetUpdateService.weatherBitApiKey != null)
                                {
                                    webAccessGranted.Add("Weather Bit");
                                    UtilityMethod.LogMessage("info",
                                        "Weather Bit key loaded!", $"{TAG}::LoadAccessProviders");
                                }// end of if block

                                break;
                            case "Here Maps Weather":

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)hereMapsRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        switch (kv[0].ToLower())
                                        {
                                            case "app_id":
                                                WidgetUpdateService.hereAppId = Decrypt(kv[1], kv[2], true);
                                                break;
                                            case "app_code":
                                                WidgetUpdateService.hereAppCode = Decrypt(kv[1], kv[2], true);
                                                break;
                                            default:
                                                break;
                                        }// end of switch block
                                    }// end of if block                                
                                }// end of for each loop

                                if (WidgetUpdateService.hereAppId != null && WidgetUpdateService.hereAppCode != null)
                                {
                                    webAccessGranted.Add("Here Maps Weather");
                                    UtilityMethod.LogMessage("info",
                                        "Here Maps Weather keys loaded!", $"{TAG}::LoadAccessProviders");
                                }// end of if block
                                else if (WidgetUpdateService.hereAppId != null && WidgetUpdateService.hereAppCode == null)
                                {
                                    UtilityMethod.ShowMessage("Here Maps Weather requires an app_code which is"
                                        + " not stored in the database.", title: WeatherLionMain.PROGRAM_NAME + " - Missing Key",
                                        mbIcon: MessageBoxIcon.Error);
                                }// end of if block
                                else if (WidgetUpdateService.hereAppId == null && WidgetUpdateService.hereAppCode != null)
                                {
                                    UtilityMethod.ShowMessage("Here Maps Weather requires an app_id which is"
                                        + " not stored in the database.", title: WeatherLionMain.PROGRAM_NAME + " - Missing Key",
                                        mbIcon: MessageBoxIcon.Error);
                                }// end of if block
                                break;
                            case "Yahoo! Weather":
                                List<string> keysFound = new List<string>();

                                foreach (string key in appKeys)
                                {
                                    string[] kv = key.Split(':');

                                    if (((IList<string>)yahooRequiredKeys).Contains(kv[0].ToLower()))
                                    {
                                        switch (kv[0].ToLower())
                                        {
                                            case "app_id":
                                                WidgetUpdateService.yahooAppId = Decrypt(kv[1], kv[2], true);
                                                keysFound.Add("app_id");
                                                break;
                                            case "consumer_key":
                                                WidgetUpdateService.yahooConsumerKey = Decrypt(kv[1], kv[2], true);
                                                keysFound.Add("consumer_key");
                                                break;
                                            case "consumer_secret":
                                                WidgetUpdateService.yahooConsumerSecret = Decrypt(kv[1], kv[2], true);
                                                keysFound.Add("consumer_secret");
                                                break;
                                            default:
                                                break;
                                        }// end of switch block
                                    }// end of if block
                                }// end of for each loop

                                // remove all the keys that were found from the list
                                keysMissing = new List<string>(((IList<string>)yahooRequiredKeys).Except(keysFound).ToList());

                                if (keysMissing.Count == 0)
                                {
                                    webAccessGranted.Add("Yahoo! Weather");
                                    UtilityMethod.LogMessage("info", "Yahoo! Weather keys loaded!",
                                        $"{TAG}::LoadAccessProviders");
                                }// end of if block
                                else
                                {
                                    // do not check for missing keys if the form is already displayed
                                    if (!addKeys.Visible)
                                    {
                                        if (CheckForMissingKeys() == DialogResult.Yes)
                                        {
                                            AccessKeysForm.frmKeys.cboAccessProvider.SelectedItem = "Yahoo! Weather";
                                            AccessKeysForm.frmKeys.Visible = true;
                                            AccessKeysForm.frmKeys.Focus();
                                        }// end of if block
                                        else
                                        {
                                            string msg = "Yahoo!Weather cannot be used as a weather source without "
                                               + "first adding the missing " + (keysMissing.Count > 1 ? "keys" : "key") + ".";

                                            UtilityMethod.ShowMessage(msg, AccessKeysForm.frmKeys, title: $"{WeatherLionMain.PROGRAM_NAME} - Missing Key", buttons: MessageBoxButtons.OK,
                                                mbIcon: MessageBoxIcon.Information);
                                        }// end of else block
                                    }// end of if block
                                    else
                                    {
                                        AccessKeysForm.frmKeys.txtKeyName.Focus();
                                    }// end of else block
                                }// end of else block

                                break;
                            default:
                                break;
                        }// end of switch block
                    }// end of if block
                }// end of outer for each loop
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::LoadAccessProviders [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            // add the only weather provider that does not require a key
            webAccessGranted.Add(WeatherLionMain.YR_WEATHER);

            if (webAccessGranted.Count > 0)
            {
                string s = string.Join(", ", webAccessGranted);
                string fs = null;

                if (UtilityMethod.NumberOfCharacterOccurences(',', s) > 1)
                {
                    fs = UtilityMethod.ReplaceLast(",", ", and", s);
                }// end of if block
                else if (UtilityMethod.NumberOfCharacterOccurences(',', s) == 1)
                {
                    fs = s.Replace(",", " and");
                }// end of else block
                else
                {
                    fs = s;
                }// end of else block

                UtilityMethod.LogMessage("info", $"The following access providers were loaded:\n{fs}.",
                         $"{TAG}::LoadAccessProviders");
            }// end of if block
            else
            {
                UtilityMethod.LogMessage("severe", "No valid access privelages were stored in the database!",
                        $"{TAG}::LoadAccessProviders");                
            }// end of else block

            if (webAccessGranted.Count == 0)
            {
                if (NoAccessPrivialgesStored() == DialogResult.Yes)
                {
                    if (!AccessKeysForm.frmKeys.Visible)
                    {
                        AccessKeysForm.frmKeys.ShowDialog();
                        AccessKeysForm.frmKeys.Focus();
                    }// end of if block
                    else
                    {
                        AccessKeysForm.frmKeys.txtKeyName.Focus();
                    }// end of else block
                }
                else
                {
                    UtilityMethod.MissingRequirementsPrompt("Insufficient Access Privilages");
                }// end of if block
            }// end of if block

            if (webAccessGranted.Count >= 1 && !geoNamesAccountLoaded)
            {
                UtilityMethod.LogMessage("severe", "GeoNames user name not found!",
                         $"{TAG}::LoadAccessProviders");

                // confirm that user has a GeoNames account and want's to store it
                string prompt = "This program requires a geonames username\n" +
                                "which was not stored in the database. IT IS FREE!" +
                                "\nDo you wish to add it now?";

                DialogResult result = MessageBox.Show(prompt, $"{WeatherLionMain.PROGRAM_NAME} - Add Access Privialges",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    //AccessKeysForm kf = AccessKeysForm.frmKeys;

                    if (!AccessKeysForm.frmKeys.Visible)
                    {
                        AccessKeysForm.frmKeys.m_key_provider = "GeoNames";                        
                        AccessKeysForm.frmKeys.ShowDialog();
                    }// end of if block
                    else
                    {
                        AccessKeysForm.frmKeys.cboAccessProvider.SelectedItem = "GeoNames";
                        AccessKeysForm.frmKeys.pwdKeyValue.Focus();
                    }// end of else block
                }// end of if block
                else
                {
                    UtilityMethod.MissingRequirementsPrompt("Insufficient Access Privilages");
                }// end of else block
            }// end of else if block

            // if valid access was not loaded for the provider previously used, take it into account
            if (WeatherLionMain.storedPreferences != null)
            {
                if (!webAccessGranted.Contains(WeatherLionMain.storedPreferences.StoredPreferences.Provider))
                {
                    WeatherLionMain.noAccessToStoredProvider = true;
                }// end of if block
                else
                {
                    WeatherLionMain.noAccessToStoredProvider = false;
                }// end of else block
            }// end of if block
        }// end of method LoadAccessProcviders

        private static DialogResult NoAccessPrivialgesStored()
        {
            // check if the user wishes to provide some accounts for access
            // to weather services.
            DialogResult result = MessageBox.Show("The program will not run without access privialges!" +
                        "\nDo you wish to add some now?", $"{WeatherLionMain.PROGRAM_NAME} - Add Access Privialges",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            return result;
        }// end of message NoAccessPrivialgesStored()
    }// end of class LionSecurityManager    
}// end of namespace WeatherLion
