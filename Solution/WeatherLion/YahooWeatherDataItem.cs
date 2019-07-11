using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          YahooWeatherDataItem
///   Description:    This class is a model class for data received
///                   from the Yahoo Weather Service.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// A Yahoo! Weather weather data object.
    /// </summary>
    [Obsolete("YahooWeatherDataItem has been replaced by YahooWeatherYdnDataItem", true)]
    public class YahooWeatherDataItem
    {
        private static string[] yahooWeatherCodes = { "tornado", "tropical storm", "hurricane", "severe thunderstorms",
                                                 "thunderstorms", "mixed rain and snow", "mixed rain and sleet",
                                                 "mixed snow and sleet", "freezing drizzle", "drizzle", "freezing rain",
                                                 "showers", "showers", "snow flurries", "light snow showers", "blowing snow",
                                                 "snow", "hail", "sleet", "dust", "foggy", "haze", "smoky", "blustery", "windy",
                                                 "cold", "cloudy", "mostly cloudy (night)", "mostly cloudy (day)",
                                                 "partly cloudy (night)", "partly cloudy (day)", "clear (night)", "sunny",
                                                 "fair (night)", "fair (day)", "mixed rain and hail", "hot",
                                                 "isolated thunderstorms", "scattered thunderstorms", "scattered thunderstorms",
                                                 "scattered showers", "heavy snow", "scattered snow showers", "heavy snow",
                                                 "partly cloudy", "thundershowers", "snow showers", "isolated thundershowers" };

        private static string[] compassSectors = { "N", "NNE", "NE", "ENE", "E", "ESE",
                                "SE", "SSE", "S", "SSW", "SW", "WSW",
                                "W", "WNW", "NW", "NNW" };

        public Query query { get; set; }

        public class Query
        {
            public string count { get; set; }
            public string created { get; set; }
            public Results results { get; set; }

            public class Results
            {
                public Channel channel { get; set; }

                public class Channel
                {
                    public Units units { get; set; }
                    public Location location { get; set; }
                    public Wind wind { get; set; }
                    public Atmosphere atmosphere { get; set; }
                    public Astronomy astronomy { get; set; }
                    public Item item { get; set; }

                    public class Units
                    {
                        public string distance { get; set; }
                        public string pressure { get; set; }
                        public string speed { get; set; }
                        public string temperature { get; set; }
                    }// end of class Units

                    public class Location
                    {
                        public string city { get; set; }
                        public string country { get; set; }
                        public string region { get; set; }
                    }// end of class Location

                    public class Wind
                    {
                        public string chill { get; set; }
                        public string direction { get; set; }
                        public string speed { get; set; }
                    }// end of class Wind

                    public class Atmosphere
                    {
                        public string humidity { get; set; }
                        public string pressure { get; set; }
                        public string rising { get; set; }
                        public string visibility { get; set; }
                    }// end of class Atmosphere

                    public class Astronomy
                    {
                        public string sunrise { get; set; }
                        public string sunset { get; set; }
                    }// end of class Astronomy

                    public class Item
                    {
                        public string title { get; set; }
                        public string lat { get; set; }
                        public string longi { get; set; }
                        public string link { get; set; }
                        public string pubDate { get; set; }
                        public Condition condition { get; set; }
                        public List<Forecast> forecast { get; set; }
                        public string description { get; set; }
                        public GUID guid { get; set; }

                        public class Condition
                        {
                            public string code { get; set; }
                            public string date { get; set; }
                            public string temp { get; set; }
                            public string text { get; set; }
                        }// end of clasd Condition

                        public class Forecast
                        {
                            public string code { get; set; }
                            public string date { get; set; }
                            public string day { get; set; }
                            public string high { get; set; }
                            public string low { get; set; }
                            public string text { get; set; }
                        }// end of class Forecast

                        public class GUID
                        {
                            public bool guid { get; set; }
                        }
                    }// end of class Item
                }// end of class Results
            }// end of class Channel                
        }// end of class Query

        public static string CompassDirection(double degrees)
        {
            int index = (int)((degrees / 22.5) + 0.5);

            return compassSectors[index];
        }// end of method CompassDirection

        public static bool DeserializeYahooJSON(string strJSON, ref YahooWeatherDataItem yahooWeatherData)
        {
            yahooWeatherData = JsonConvert.DeserializeObject<YahooWeatherDataItem>(strJSON);

            if (yahooWeatherData == null)
            {
                return false;
            }// end of if block
            else
            {
                return true;
            }// end of else block

        }// end of method DeserializeJSON        

        public static void LoadYahooWeatherData(YahooWeatherDataItem wxData)
        {
            string city = $"{wxData.query.results.channel.location.city}, {wxData.query.results.channel.location.region}";
            string country = wxData.query.results.channel.location.country;
            string datePublished = wxData.query.results.channel.item.pubDate;
            string currentTemperature = wxData.query.results.channel.item.condition.temp +
                "\u00B0" + wxData.query.results.channel.units.temperature;
            string currentWXCondition = (!wxData.query.results.channel.item.condition.code.Equals("3200") ?
                yahooWeatherCodes[int.Parse(wxData.query.results.channel.item.condition.code)].ToProperCase() :
                "N/A");

            string windChill = wxData.query.results.channel.wind.chill + "\u00B0" +
                                wxData.query.results.channel.units.temperature;
            string windDirection = CompassDirection(double.Parse(wxData.query.results.channel.wind.direction));
            string windSpeed = wxData.query.results.channel.wind.speed;
            string wind = windDirection + " " + windSpeed + " " + wxData.query.results.channel.units.speed;

            string humidity = wxData.query.results.channel.atmosphere.humidity + "%";
            string pressure = wxData.query.results.channel.atmosphere.pressure + " " +
                                wxData.query.results.channel.units.pressure;
            string rising = wxData.query.results.channel.atmosphere.rising;
            string visibility = wxData.query.results.channel.atmosphere.visibility + " " +
                                wxData.query.results.channel.units.distance;

            string sunrise = wxData.query.results.channel.astronomy.sunrise;
            string sunset = wxData.query.results.channel.astronomy.sunset;

            // Header
            Console.WriteLine($"Current weather data for the city of {city} in {country}");
            Console.WriteLine();

            // Weather Conditions
            Console.WriteLine("\t*** Current Weather Conditions ***");
            Console.WriteLine("\t\tDate Published: " + datePublished);
            Console.WriteLine("\t\tCurrent Temperature: " + currentTemperature);
            Console.WriteLine("\t\tCurrent Conditions: " + currentWXCondition);
            Console.WriteLine();

            // Wind Readings
            Console.WriteLine("\t*** Wind ***");
            Console.WriteLine("\t\tWind Chill: " + windChill);
            Console.WriteLine("\t\tWind: " + wind);
            Console.WriteLine();

            // Atmospheric Readings
            Console.WriteLine("\t*** Atmosphere ***");
            Console.WriteLine("\t\tHumidity: " + humidity);
            Console.WriteLine("\t\tPressure: " + pressure);
            Console.WriteLine("\t\tRising: " + rising);
            Console.WriteLine("\t\tVisibility: " + visibility);
            Console.WriteLine();

            // Astronomy
            Console.WriteLine("\t*** Astronomy ***");
            Console.WriteLine("\t\tSunrise: " + sunrise);
            Console.WriteLine("\t\tSunset: " + sunset);
            Console.WriteLine();

            Console.WriteLine();

            // Five Day Forcast
            Console.WriteLine("\t*** Five Day Forecast ***");
            Console.WriteLine();

            int i = 1;

            // loop throught hte forcast data. only 5 days are needed
            foreach (var wxForecast in wxData.query.results.channel.item.forecast)
            {
                string fCondition = !wxData.query.results.channel.item.condition.code.Equals("3200") ?
                                   yahooWeatherCodes[int.Parse(wxForecast.code)].ToProperCase() :
                                   "N/A";
                string fDay = wxForecast.day;
                string fDate = (DateTime.Parse(wxForecast.date) == DateTime.Today ?
                                string.Format("{0:MMMM d, yyyy}", DateTime.Parse(wxForecast.date)) + " (Today)" :
                                string.Format("{0:MMMM d, yyyy}", DateTime.Parse(wxForecast.date)));
                string fHigh = wxForecast.high + "\u00B0" + wxData.query.results.channel.units.temperature;
                string fLow = wxForecast.low + "\u00B0" + wxData.query.results.channel.units.temperature;

                //Console.WriteLine( "\t\tCode: " + wxForecast.code );
                Console.WriteLine("\t\tDay: " + fDay);
                Console.WriteLine("\t\tDate: " + fDate);
                Console.WriteLine("\t\tHigh: " + fHigh);
                Console.WriteLine("\t\tLow: " + fLow);
                Console.WriteLine("\t\tCondition: " + fCondition);
                //Console.WriteLine( "\t\tText: " + wxForecast.text ); // Yahoo provides this data but others may not               
                Console.WriteLine();

                if (i == 5)
                {
                    break;
                }// end of if block

                i++; // increment sentinel
            }// end of for each loop

            Console.WriteLine();
        }// end of method LoadYahooWeatherData      
    }// end of class YahooWeatherDataItem
}
