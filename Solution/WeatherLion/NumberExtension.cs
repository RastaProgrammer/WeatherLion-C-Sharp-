using System;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          NumberExtension
///   Description:    The class provides methods to handle various
///                   numerical calculations.
///   Author:         Paul O. Patterson     Date: October 03, 2017
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// The class provides methods to handle various numerical calculations.
    /// </summary>
    public static class NumberExtension
    {
        private static string[] compassSectors = { "N", "NNE", "NE", "ENE", "E", "ESE",
                                "SE", "SSE", "S", "SSW", "SW", "WSW",
                                "W", "WNW", "NW", "NNW" };


        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Celsius and converts it to Fahrenheit.
        /// </summary>
        /// <param name="celsius"> The temperature in Celsius</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Fahrenheit.</returns>
        public static float CelsiusToFahrenheit(this float celsius)
        {
            return (float)(celsius * 1.8 + 32);
        }// end of method CelsiusToFahrenheit

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Celsius and converts it to Kelvin.
        /// </summary>
        /// <param name="celsius"> The temperature in Celsius</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Kelvin.</returns>
        public static double CelsiusToKelvin(this float celsius)
        {
            return celsius + 273.15;
        }// end of method CelsiusToKelvin       

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Fahrenheit and converts it to Celsius.
        /// </summary>
        /// <param name="fahrenheit">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Celsius.</returns>
        public static float FahrenheitToCelsius(this float fahrenheit)
        {
            return (float)(Math.Round((fahrenheit - 32) / 1.8, 2));
        }// end of method FahrenheitToKelvin

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Fahrenheit and converts it to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Kelvin.</returns>
        public static float FahrenheitToKelvin(this float fahrenheit)
        {
            return (float)(Math.Round((fahrenheit + 459.67) * 0.5555555555555556, 2));
        }// end of method FahrenheitToKelvin

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Fahrenheit and converts it to Kelvin.
        /// </summary>
        /// <param name="fahrenheit">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Kelvin.</returns>
        public static DateTime GetDateTime(this long unixTimestamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimestamp).ToLocalTime();
            return dtDateTime;
        }// end of method GetDateTime

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Kelvin and converts it to Celsius.
        /// </summary>
        /// <param name="kelvin">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Celsius.</returns>
        public static float KelvinToCelsius(this float kelvin)
        {
            return (float)(Math.Round(kelvin - 273.15, 2));
        }// end of method KelvinToCelsius

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a temperature in Kelvin and converts it to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted value in Fahrenheit.</returns>
        public static float KelvinToFahrenheit(this float kelvin)
        {
            return (float)(Math.Round(kelvin * 1.8 - 459.67, 2));
        }// end of method KelvinToFahrenheit

        /// <summary>
        /// Converts milliseconds to minutes.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to be converted.</param>
        /// <remarks>The value must be numeric.</remarks> 
        /// <returns>The converted time value.</returns>
        public static int MillisecondsToMinutes(this int milliseconds)
        {
            return milliseconds / 60000;
        }// end of method MillisecondsToMinutes

        /// <summary>
        /// Converts a minute time value to milliseconds.
        /// </summary>
        /// <param name="minutes">The number of minutes to be converted.</param>
        /// <remarks>The value must be numeric.</remarks> 
        /// <returns>The converted time value.</returns>
        public static int MinutesToMilliseconds(this int minutes)
        {
            return minutes * 60000;
        }// end of method MinutesToMillisesonds  

        /// <summary>
        /// Accepts a numeric value of type float that represents  
        /// a rate of speed in Mps (Miles per second) and converts it to Mph (Miles per hour).
        /// </summary>
        /// <param name="mps">The rate of speed in Mps (Miles per second).</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted rate of speed value in Mph (Miles per hour).</returns>
        public static float MpsToMph(this float mps)
        {
            return (float)(Math.Round(mps * 2.23694, 2));
        }// end of method MpsToMph         

        // Original script is by Ron Murphy.
        /// <summary>
        /// Calculates the wind chill based on the temperature and the rate of speed.
        /// </summary>
        /// <param name="tempF">The temperature in Fahrenheit.</param>
        /// <param name="mph">The rate of speed in Mps (Miles per hour).</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted wind chill.</returns>
        public static float GetWindchill(float tempF, float mph)
        {
            if (tempF > 41)
            {
                return 0;
            }// end of if block
            else
            {
                if (mph <= 4)
                {
                    return tempF;
                }// end of if block
                else
                {
                    float wc = (float)(Math.Round(35.74 + (0.6215 * tempF) - (35.75 * Math.Pow(mph, 0.16)) + (0.4275 * tempF * Math.Pow(mph, 0.16)), 0));

                    return wc;
                }// end of else block
            }// end of else block           
        }// end of method GetWindchill

        // Original script is by Ron Murphy.
        /// <summary>
        /// Calculates the relative humidity based on the air temperature and the dew point.
        /// </summary>
        /// <param name="airTemp">The temperature in Fahrenheit.</param>
        /// <param name="dewPointTempF">The temperature in Fahrenheit.</param>
        /// <remarks>The value must be numeric.</remarks>
        /// <returns>The converted dew point.</returns>
        public static float GetRelativeHumidity(this float airTemp, float dewPointTempF)
        {
            float tc = (float)((airTemp - 32) * .556);
            float tdc = (float)((dewPointTempF - 32) * .556);

            if (tc < tdc)
            {
                // The dew point temperature cannot be higher than the air temperature
                return 0;
            }// end of if block
            else
            {
                float rh = (float)(Math.Round(100.0 * (Math.Pow((112 - (0.1 * tc) + tdc) / (112 + (0.9 * tc)), 8)), 0));

                return rh;
            }// end of else block
        }// end of method GetRelativeHumidity
    }// end of class NumberExtension
}// end of namespace WeatherLion
