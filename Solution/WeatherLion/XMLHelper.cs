using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          XMLHelper
///   Description:    A helper class for handling XML data.
///   Author:         Paul O. Patterson     Date: May 14, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A helper class for handling XML data.
    /// </summary>
    public class XMLHelper
    {
        private const string TAG = "WeatherLionMain";
        public static readonly string PREVIOUSLY_FOUND_CITIES_XML = 
            $@"{WeatherLionMain.RES_PATH}\storage\locations_used\xml\previous_cities.xml";

        /// <summary>
        /// Accepts a CityData object and exports it to an XML file.  
        /// </summary>
        /// <param name="cityData">The CityData object to be written to an XML file.</param>
        /// <returns>A <see cref="bool"/> value representing success or failure</returns>       
        public static bool ExportToXML(CityData cityData)
        {
            if (cityData != null)
            {
                try
                {
                    bool append;
                    XmlWriter writer;
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = ("\t"),
                        CloseOutput = true,
                        OmitXmlDeclaration = false
                    };

                    // check for the parent directory
                    if (!Directory.Exists(Directory.GetParent(PREVIOUSLY_FOUND_CITIES_XML).FullName))
                    {
                        Directory.CreateDirectory(Directory.GetParent(PREVIOUSLY_FOUND_CITIES_XML).FullName);
                    }// end of if block

                    if (File.Exists(PREVIOUSLY_FOUND_CITIES_XML))
                    {
                        xmlDoc.Load(PREVIOUSLY_FOUND_CITIES_XML);
                        XmlElement root = xmlDoc.DocumentElement;

                        writer = root.CreateNavigator().AppendChild();
                        append = true;
                    }// end of if block
                    else
                    {
                        writer = XmlWriter.Create(PREVIOUSLY_FOUND_CITIES_XML, settings);
                        append = false;
                    }// end of else block

                    // Root element
                    if (!append) writer.WriteStartElement("WorldCities");
                    writer.WriteStartElement("City");
                    writer.WriteElementString("CityName", cityData.cityName);
                    writer.WriteElementString("CountryName", cityData.countryName);
                    writer.WriteElementString("CountryCode", cityData.countryCode);
                    writer.WriteElementString("RegionName", cityData.regionName);
                    writer.WriteElementString("RegionCode", cityData.regionCode);
                    writer.WriteElementString("Latitude", cityData.latitude.ToString());
                    writer.WriteElementString("Longitude", cityData.longitude.ToString());
                    writer.WriteEndElement();
                    if (!append) writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();

                    if (append) xmlDoc.Save(PREVIOUSLY_FOUND_CITIES_XML);

                    return true;
                }// end of try block
                catch (Exception e)
                {
                    UtilityMethod.LogMessage("severe", e.Message,
                        $"{TAG}::ExportToXML [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                    return false;
                }// end of catch block
            }// end of if block
            else
            {
                return false;
            }// end of else block            
        }// end of method ExportToXML       

        /// <summary>
        /// Converts XML data and converts them into a list of CityData objects. 
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing CityData objects that were converted from XML</returns>
        public static List<CityData> ImportFromXML()
        {
            List<CityData> cd = new List<CityData>();

            try
            {
                XmlDocument serviceData = new XmlDocument();
                serviceData.Load(PREVIOUSLY_FOUND_CITIES_XML);

                if (serviceData != null)
                {
                    XmlNodeList xnlCity = serviceData.SelectNodes("//City");

                    for (int i = 0; i < xnlCity.Count; i++)
                    {
                        string cityName = xnlCity[i].ChildNodes[0].InnerText;
                        string countryName = xnlCity[i].ChildNodes[1].InnerText;
                        string countryCode = xnlCity[i].ChildNodes[2].InnerText;
                        string regionName = xnlCity[i].ChildNodes[3].InnerText;
                        string regionCode = xnlCity[i].ChildNodes[4].InnerText;
                        string latitude = xnlCity[i].ChildNodes[5].InnerText;
                        string Longitude = xnlCity[i].ChildNodes[6].InnerText;

                        cd.Add(
                                new CityData(
                                    xnlCity[i].ChildNodes[0].InnerText, xnlCity[i].ChildNodes[1].InnerText,
                                    xnlCity[i].ChildNodes[2].InnerText, xnlCity[i].ChildNodes[3].InnerText,
                                    xnlCity[i].ChildNodes[4].InnerText, float.Parse(xnlCity[i].ChildNodes[5].InnerText),
                                    float.Parse(xnlCity[i].ChildNodes[6].InnerText)
                               ));

                    }// end of for loop    		 		
                }// end of if block
            }// end of try block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::ImportFromXML [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block          

            return cd;
        }// end of method ImportFromXML
    }// end of class XMLHelper
}// and of namespace WeatherLion
