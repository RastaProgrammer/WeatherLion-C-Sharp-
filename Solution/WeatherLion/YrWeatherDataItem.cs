using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          YrWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Yr.no (Norwegian Metrological Institute)
///                   Weather Service.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Yr.no (Norwegian Metrological Institute) weather data object.
    /// </summary>
    public class YrWeatherDataItem
    {
        #region Class Properties

        public string name { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public string tzId { get; set; }
        public float tzUtcOffsetMinutes { get; set; }
        public float locAltitude { get; set; }
        public float locLatitude { get; set; }
        public float locLongitude { get; set; }
        public string geonames { get; set; }
        public long geobaseid { get; set; }
        public DateTime lastupdate { get; set; }
        public DateTime sunrise { get; set; }
        public DateTime sunset { get; set; }

        public List<Forecast> forecast { get; set; }
        //public List<Forecast> forecast = new List<Forecast>();

        #endregion

        public static YrWeatherDataItem yrWeatherDataItem;

        public YrWeatherDataItem()
        {
        }   

        public class Forecast
        {
            public DateTime timeFrom { get; set; }
            public DateTime timeTo { get; set; }
            public int timePeriod { get; set; }
            public int symbolNumber { get; set; }
            public int symbolNumberEx { get; set; }
            public string symbolName { get; set; }
            public string symbolVar { get; set; }
            public float precipValue { get; set; }
            public float windDirDeg { get; set; }
            public string windDirCode { get; set; }
            public string windDirName { get; set; }
            public float windSpeedMps { get; set; }
            public string windSpeedName { get; set; }
            public string temperatureUnit { get; set; }
            public float temperatureValue { get; set; }
            public string pressureUnit { get; set; }
            public float pressureValue { get; set; }

            public Forecast(DateTime timeFrom, DateTime timeTo, int timePeriod,
                            int symbolNumber, int symbolNumberEx, string symbolName,
                            string symbolVar, float precipValue, float windDirDeg, string windDirCode,
                            string windDirName, float windSpeedMps, string windSpeedName,
                            string temperatureUnit, float temperatureValue, string pressureUnit, float pressureValue)
            {
                this.timeFrom = timeFrom;
                this.timeTo = timeTo;
                this.timePeriod = timePeriod;
                this.symbolNumber = symbolNumber;
                this.symbolNumberEx = symbolNumberEx;
                this.symbolName = symbolName;
                this.symbolVar = symbolVar;
                this.precipValue = precipValue;
                this.windDirDeg = windDirDeg;
                this.windDirCode = windDirCode;
                this.windDirName = windDirName;
                this.windSpeedMps = windSpeedMps;
                this.windSpeedName = windSpeedName;
                this.temperatureUnit = temperatureUnit;
                this.temperatureValue = temperatureValue;
                this.pressureUnit = pressureUnit;
                this.pressureValue = pressureValue;
            }// end of contructor
        }// end of inner class Forecast 

        public static bool DeserializeYrXML(string xmlString, ref YrWeatherDataItem yrWeatherData)
        {
            if (yrWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                XmlDocument weatherXML = new XmlDocument();
                weatherXML.LoadXml(xmlString);

                XmlNodeList xnlCurrentLocation = weatherXML.SelectNodes("//location");

                yrWeatherData.name = xnlCurrentLocation[0].ChildNodes[0].InnerText;
                yrWeatherData.type = xnlCurrentLocation[0].ChildNodes[1].InnerText;
                yrWeatherData.country = xnlCurrentLocation[0].ChildNodes[2].InnerText;
                yrWeatherData.tzId = xnlCurrentLocation[0].ChildNodes[3].Attributes["id"].Value;
                yrWeatherData.tzUtcOffsetMinutes = float.Parse(xnlCurrentLocation[0].ChildNodes[3].Attributes["utcoffsetMinutes"].Value);
                yrWeatherData.locAltitude = float.Parse(xnlCurrentLocation[0].ChildNodes[4].Attributes["altitude"].Value);
                yrWeatherData.locLatitude = float.Parse(xnlCurrentLocation[0].ChildNodes[4].Attributes["latitude"].Value);
                yrWeatherData.locLongitude = float.Parse(xnlCurrentLocation[0].ChildNodes[4].Attributes["longitude"].Value);
                yrWeatherData.geonames = xnlCurrentLocation[0].ChildNodes[4].Attributes["geobase"].Value;
                yrWeatherData.geobaseid = long.Parse(xnlCurrentLocation[0].ChildNodes[4].Attributes["geobaseid"].Value);
                yrWeatherData.lastupdate = DateTime.Parse(weatherXML.SelectSingleNode("//meta/lastupdate").InnerText);
                yrWeatherData.sunrise = DateTime.Parse(weatherXML.SelectSingleNode("//sun").Attributes["rise"].Value);
                yrWeatherData.sunset = DateTime.Parse(weatherXML.SelectSingleNode("//sun").Attributes["set"].Value);

                XmlNodeList elemList = weatherXML.SelectNodes("//forecast/tabular/time");
                yrWeatherData.forecast = new List<Forecast>();

                for (int i = 0; i < elemList.Count; i++)
                {
                    DateTime timeFrom = DateTime.Parse(elemList[i].Attributes["from"].Value);
                    DateTime timeTo = DateTime.Parse(elemList[i].Attributes["to"].Value);
                    int timePeriod = int.Parse(elemList[i].Attributes["period"].Value);
                    int symNum = int.Parse(elemList[i].SelectSingleNode("symbol").Attributes["number"].Value);
                    int symNumEx = int.Parse(elemList[i].SelectSingleNode("symbol").Attributes["numberEx"].Value);
                    string symName = elemList[i].SelectSingleNode("symbol").Attributes["name"].Value;
                    string symVar = elemList[i].SelectSingleNode("symbol").Attributes["var"].Value;
                    float precipValue = float.Parse(elemList[i].SelectSingleNode("precipitation").Attributes["value"].Value);
                    float wdDeg = float.Parse(elemList[i].SelectSingleNode("windDirection").Attributes["deg"].Value);
                    string wdCode = elemList[i].SelectSingleNode("windDirection").Attributes["code"].Value;
                    string wdName = elemList[i].SelectSingleNode("windDirection").Attributes["name"].Value;
                    float wsMps = float.Parse(elemList[i].SelectSingleNode("windSpeed").Attributes["mps"].Value);
                    string wsName = elemList[i].SelectSingleNode("windSpeed").Attributes["name"].Value;
                    string tempUnit = elemList[i].SelectSingleNode("temperature").Attributes["unit"].Value;
                    float tempValue = float.Parse(elemList[i].SelectSingleNode("temperature").Attributes["value"].Value);
                    string pressureUnit = elemList[i].SelectSingleNode("pressure").Attributes["unit"].Value;
                    float pressureValue = float.Parse(elemList[i].SelectSingleNode("pressure").Attributes["value"].Value);                                      

                    yrWeatherData.forecast.Add(new Forecast(timeFrom, timeTo, timePeriod, symNum,
                                               symNumEx, symName, symVar, precipValue,
                                               wdDeg, wdCode, wdName, wsMps, wsName,
                                               tempUnit, tempValue, pressureUnit, pressureValue));
                }// end of for loop

                return true;
            }// end of else block           
        }// end of method DeserializeYrXML

        /// <summary>
        /// Removes unwanted escape characters from a string if the exists
        /// </summary>
        /// <param name="data">The <see cref="string"/> that could possibly contain unwanted characters</param>
        /// <returns>A <see cref="string"/>n with all inappropriate characters removed</returns>
        public static string RemoveEscapeCharacters(string data)
        {
            StringBuilder sb = new StringBuilder();
            string[] parts = data.Split(
                new char[] { ' ', '\a', '\b', '\n', '\t', '\r', '\f', '\v' },
                    StringSplitOptions.RemoveEmptyEntries);
            int size = parts.Length;
            string escapedData;

            for (int i = 0; i < size; i++)
            {
                sb.AppendFormat("{0} ", parts[i]);
            }// end of for loop

            if (sb.Length > 0)
            {
                // This means that characters were found and removed
                escapedData = Regex.Replace(sb.ToString(), "\"", "'");

                return escapedData.Trim();
            }// end of if block
            else
            {
                // non of the specified characters were found
                return data;
            }// end of else block
        }// end of method RemoveEscapeCharacters
    }// end of class YrWeatherDataItem    
}// end of namespace WeatherLion
