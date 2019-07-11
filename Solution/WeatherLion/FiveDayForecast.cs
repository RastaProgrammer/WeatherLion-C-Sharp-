using System;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WeatherLionMain
///   Description:    This class serves as an object for storing five
///                   day weather forcast information in a list.
///   Author:         Paul O. Patterson     Date: May 28, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Data Model for a five day weather forecast
    /// </summary>
    public class FiveDayForecast
    {
        public DateTime forecastDate { get; set; }
        public string forecastHighTemp { get; set; }
        public string forecastLowTemp { get; set; }
        public string forecastCondition { get; set; }

        public FiveDayForecast()
        {
            new FiveDayForecast(new DateTime(), null, null, null);
        }// end of default constructor

        public FiveDayForecast(DateTime forecastDate, string forecastHighTemp,
                string forecastLowTemp, string forecastCondition)
        {
            this.forecastDate = forecastDate;
            this.forecastHighTemp = forecastHighTemp;
            this.forecastLowTemp = forecastLowTemp;
            this.forecastCondition = forecastCondition;
        }// end of four-argument constructor
    }// end of class FiveDayForecast
}// end of namespace WeatherLion
