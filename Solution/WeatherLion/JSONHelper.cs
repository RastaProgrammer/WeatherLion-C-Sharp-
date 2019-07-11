using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          JSONHelper
///   Description:    A helper class for handling JSON data.
///   Author:         Paul O. Patterson     Date: May 14, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A helper class for handling JSON data.
    /// </summary>
    public class JSONHelper
    {
        private const string TAG = "JSONHelper";
        public static List<CityData> cityDataList = new List<CityData>();
        public static readonly string PREVIOUSLY_FOUND_CITIES_JSON =
            $@"{WeatherLionMain.RES_PATH}\storage\locations_used\json\previous_cities.json";

        /// <summary>
        /// Accepts a CityData object and exports it to a JSON file.  
        /// </summary>
        /// <param name="cityData">The CityData object to be written to a JSON file.</param>
        /// <returns>A <see cref="bool"/> value representing success or failure</returns>
        public static bool ExportToJSON(CityData dataItem)
        {
            if (dataItem != null)
            {
                try
                {
                    string strJSON = null;
                    string convertedJson = null;
                    List<CityData> cityDataList;

                    // check for the parent directory
                    if (!Directory.Exists(Directory.GetParent(PREVIOUSLY_FOUND_CITIES_JSON).FullName))
                    {
                        Directory.CreateDirectory(Directory.GetParent(PREVIOUSLY_FOUND_CITIES_JSON).FullName);
                    }// end of if block

                    if (File.Exists(PREVIOUSLY_FOUND_CITIES_JSON))
                    {
                        strJSON = File.ReadAllText(PREVIOUSLY_FOUND_CITIES_JSON);
                        cityDataList = JsonConvert.DeserializeObject<List<CityData>>(strJSON);
                        cityDataList.Add(dataItem);
                        convertedJson = JsonConvert.SerializeObject(cityDataList, Formatting.Indented);
                    }// end of if block
                    else
                    {
                        cityDataList = new List<CityData> { dataItem }; // create an array so that more items can be added later
                        convertedJson = JsonConvert.SerializeObject(cityDataList, Formatting.Indented);
                    }// end of else block

                    using (var jsonWriter = new StreamWriter(PREVIOUSLY_FOUND_CITIES_JSON, false))
                    {
                        jsonWriter.WriteLine(JsonPrettify(convertedJson));
                    }// end of using block

                    return true;
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::ExportToJSON [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                    return false;
                }// end of catch block
            }// enn of if block
            else
            {
                return false;
            }// end of else block
        }// end of method ExportToJSON

        /// <summary>
        /// Converts JSON data and converts them into a list of CityData objects. 
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing CityData objects that were converted from JSON</returns>
        public static List<CityData> ImportFromJSON()
        {
            string strJSON = null;
            List<CityData> cityDataList = null;

            try
            {
                // if there is a file present then it will contain a list with at least one object
                if (File.Exists(PREVIOUSLY_FOUND_CITIES_JSON))
                {
                    strJSON = File.ReadAllText(PREVIOUSLY_FOUND_CITIES_JSON);

                    // convert the file JSON into a list of objects
                    cityDataList = JsonConvert.DeserializeObject<List<CityData>>(strJSON);
                }// end of if block
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::ImportFromJSON [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
            
            return cityDataList;
        }// end of method ImportFromJSON

        /// <summary>
        /// Saves JSON data to a local file for quicker access later.
        /// </summary>
        /// <param name="jsonData">JSON data formatted as a <see cref="string"/>.</param>
        /// <param name="path">The path where the data will reside locally.</param>
        /// <returns>True/False depending on the success of the operation.</returns>
        public static bool SaveToJSONFile(string jsonData, string path)
        {
            bool fileSaved = false;

            try
            {
                if (jsonData != null)
                {
                    string parentPath = Directory.GetParent(path).ToString();

                    // the storage directory must be present before file creation
                    if (!Directory.Exists(parentPath))
                    {
                        Directory.CreateDirectory(parentPath);
                    }// end of if block

                    if (!File.Exists(path))
                    {
                        using (var jsonWriter = new StreamWriter(path, false))
                        {
                            jsonWriter.WriteLine(JsonPrettify(jsonData));
                        }// end of using block
                    }// end of if block
                    else
                    {
                        // file already exists
                        fileSaved = false;
                    }// end of else block

                    return true;
                }// end of if block        
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                   $"{TAG}::SaveToJSONFile [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            return fileSaved;
        }// end of method SaveToJSONFile

        /// <summary>
        /// Returns JSON data to in a formatted (pretty) structure
        /// </summary>
        /// <param name="json">JSON data formatted as a <see cref="string"/></param>
        /// <see cref="https://stackoverflow.com/a/30329731"/>
        /// <returns>The formatted JSON <see cref="string"/></returns>
        public static string JsonPrettify(string json)
        {
            if (json != null)
            {
                using (var stringReader = new StringReader(json))
                using (var stringWriter = new StringWriter())
                {
                    var jsonReader = new JsonTextReader(stringReader);
                    var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                    jsonWriter.WriteToken(jsonReader);

                    return stringWriter.ToString();
                }// end of using block
            }// end of if block
            else
            {
                // return whatever was sent originally
                return json;
            }// end of else block            
        }// end of method JsonPrettify
    }// end of class JSONHelper
}// and of namespace WeatherLion
