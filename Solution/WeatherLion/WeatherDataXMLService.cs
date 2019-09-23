using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace WeatherLion
{
    /// <summary>
    /// The data model/service is reponsible for storing retrieved weather data as an XML XmlDocument locally
    /// </summary>
    public class WeatherDataXMLService
    {
        private const string TAG = "WeatherDataXMLService";

        public string providerName;
        public DateTime datePublished;
        public string cityName;
        public string countryName;
        public string currentConditions;
        public string currentTemperature;
        public string currentFeelsLikeTemperature;
        public string currentHigh;
        public string currentLow;
        public string currentWindSpeed;
        public string currentWindDirection;
        public string currentHumidity;
        public string sunriseTime;
        public string sunsetTime;
        public List<FiveDayForecast> fiveDayForecast;

        private static string previousWeatherDataXML = 
            $"{WeatherLionMain.DATA_DIRECTORY_PATH}{WeatherLionMain.WEATHER_DATA_XML}";

        public WeatherDataXMLService()
        {
        }// end of default constructor

        public WeatherDataXMLService(string providerName, DateTime datePublished,
                string cityName, string countryName, string currentConditions,
                string currentTemperature, string feelsLikeTemperature, string currentHigh,
                string currentLow, string currentWindSpeed, string currentWindDirection,
                string currentHumidity, string sunriseTime, string sunsetTime,
                List<FiveDayForecast> fiveDayForecast)
        {
            this.providerName = providerName;
            this.datePublished = datePublished;
            this.cityName = cityName;
            this.countryName = countryName;
            this.currentConditions = currentConditions;
            this.currentTemperature = currentTemperature;
            currentFeelsLikeTemperature = feelsLikeTemperature;
            this.currentHigh = currentHigh;
            this.currentLow = currentLow;
            this.currentWindSpeed = currentWindSpeed;
            this.currentWindDirection = currentWindDirection;
            this.currentHumidity = currentHumidity;
            this.sunriseTime = sunriseTime;
            this.sunsetTime = sunsetTime;
            this.fiveDayForecast = fiveDayForecast;
        }// end of fifteen-argument constructor

        public void ProcessData()
        {
            SaveCurrentWeatherXML(providerName, datePublished,
                    cityName, countryName, currentConditions,
                    currentTemperature, currentFeelsLikeTemperature,
                    currentHigh, currentLow, currentWindSpeed,
                    currentWindDirection, currentHumidity, sunriseTime,
                    sunsetTime, fiveDayForecast);
        }// end of method ProcessData

        public static bool SaveCurrentWeatherXML(string providerName, DateTime datePublished, string cityName,
                string countryName, string currentConditions, string currentTemperature, string currentFeelsLikeTemperture,
                string currentHigh, string currentLow, string currentWindSpeed, string currentWindDirection, string currentHumidity,
                string sunriseTime, string sunsetTime, List<FiveDayForecast> fiveDayForecast)
        {
            string weatherDataDirectory = WeatherLionMain.DATA_DIRECTORY_PATH;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            string tzAbbr = UtilityMethod.ReplaceAll(localZone.DaylightName, "[a-z]", "").Replace(" ", "");

            // create all necessary files if they are not present
            if (!Directory.Exists(weatherDataDirectory))
            {
                Directory.CreateDirectory(weatherDataDirectory);
            }// end of if block

            try
            {
                XmlWriter writer;
                XmlDocument xmlDoc = new XmlDocument();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = ("\t"),
                    CloseOutput = true,
                    OmitXmlDeclaration = false
                };                

                // create a new file each time
                writer = XmlWriter.Create(previousWeatherDataXML, settings);                    

                // Root XmlElement
                writer.WriteStartElement("WeatherData");
                
                // Provider Details
                writer.WriteStartElement("Provider");
                writer.WriteElementString("Name", providerName);
                writer.WriteElementString("Date", string.Format("{0:ddd MMM dd HH:mm:ss} {1} {0:yyyy}", datePublished, tzAbbr));
                writer.WriteEndElement();

                // Location Readings
                writer.WriteStartElement("Location");
                writer.WriteElementString("City", cityName);
                writer.WriteElementString("Country", countryName);
                writer.WriteEndElement();

                // Atmospheric Readings
                writer.WriteStartElement("Atmosphere");
                writer.WriteElementString("Humidity", currentHumidity);                
                writer.WriteEndElement();

                // Wind Readings
                writer.WriteStartElement("Wind");
                writer.WriteElementString("WindSpeed", currentWindSpeed);
                writer.WriteElementString("WindDirection", currentWindDirection);
                writer.WriteEndElement();

                // Astronomy readings
                writer.WriteStartElement("Astronomy");
                writer.WriteElementString("Sunrise", sunriseTime);
                writer.WriteElementString("Sunset", sunsetTime);
                writer.WriteEndElement();

                // Current Weather
                writer.WriteStartElement("Current");
                writer.WriteElementString("Condition", UtilityMethod.ToProperCase(currentConditions));
                writer.WriteElementString("Temperature", currentTemperature);
                writer.WriteElementString("FeelsLike", currentFeelsLikeTemperture);
                writer.WriteElementString("HighTemperature", currentHigh);
                writer.WriteElementString("LowTemperature", currentLow);
                writer.WriteEndElement();

                // list of forecast data
                writer.WriteStartElement("DailyForecast");

                // Five Day Forecast                
                foreach (FiveDayForecast forecast in fiveDayForecast)
                {                    
                    writer.WriteStartElement("DayForecast");                    
                        writer.WriteElementString("Date", string.Format("{0:ddd MMM dd hh:mm:ss} {1} {0:yyyy}", forecast.forecastDate, tzAbbr));
                        writer.WriteElementString("Condition", UtilityMethod.ToProperCase(forecast.forecastCondition));
                        writer.WriteElementString("LowTemperature", forecast.forecastLowTemp);
                        writer.WriteElementString("HighTemperature", forecast.forecastHighTemp);
                    writer.WriteEndElement();                                        
                }// end of for each loop

                writer.WriteEndElement();

                // close the root element
                writer.WriteEndElement();

                writer.Flush();
                writer.Close();                

                UtilityMethod.LogMessage("info", providerName + "'s weather data was stored locally!",
                    "WeatherDataXMLService::SaveCurrentWeatherXML");

            }// end of try block
            catch (FileNotFoundException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                     $"{TAG}::SaveCurrentWeatherXML [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
            catch (IOException e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::SaveCurrentWeatherXML [line: {UtilityMethod.GetExceptionLineNumber(e)}]");               
            }// end of catch block

            return true;
        }// end of method SaveCurrentWeatherXML

        public static void UpdateUnits(string currentTemperature, string currentFeelsLikeTemperture,
            string currentHigh, string currentLow, string currentWindSpeed, List<FiveDayForecast> fiveDayForecast)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(previousWeatherDataXML);

                XmlNode node = null;
                XmlNode root = xmlDoc.DocumentElement;


                node = xmlDoc.SelectSingleNode("//Temperature");
                node.InnerText = currentTemperature;

                node = xmlDoc.SelectSingleNode("//FeelsLike");
                node.InnerText = currentFeelsLikeTemperture;

                node = xmlDoc.SelectSingleNode("//HighTemperature");
                node.InnerText = currentHigh;

                node = xmlDoc.SelectSingleNode("//LowTemperature");
                node.InnerText = currentLow;

                node = xmlDoc.SelectSingleNode("//WindSpeed");
                node.InnerText = currentWindSpeed;

                // Five Day Forecast
                XmlNodeList forecastList = xmlDoc.SelectNodes("//ForecastList/ForecastData");

                for (int i = 0; i < forecastList.Count; i++)
                {
                    XmlNode lowTemp = ((XmlElement)forecastList[i]).SelectSingleNode(".//LowTemperature");
                    lowTemp.InnerText = fiveDayForecast[i].forecastLowTemp;

                    XmlNode highTemp = ((XmlElement)forecastList[i]).SelectSingleNode(".//HighTemperature");
                    highTemp.InnerText = fiveDayForecast[i].forecastHighTemp;
                }// end of for loop

                // save the updated units
                xmlDoc.Save(previousWeatherDataXML);
            }// end of try block
            catch(Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::UpdateUnits [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
        }// end of method UpdateUnits
    }// end of class WeatherDataXMLService
}// end of namespace WeatherLion
