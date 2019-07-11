using System;
using System.Net;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          NetworkHelper
///   Description:    Determines is the computer has an open 
///                   Internet connection.
///   Author:         Paul O. Patterson     Date: May 18, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    public class NetworkHelper
    {
        private const string TAG = "NetworkHelper";

        /// <summary>
        /// Determines is the computer has an open Internet connection
        /// </summary>
        /// <returns>True/False dependent on the outcome of the check.</returns>
        public static bool HasNetworkAccess()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }// end of inner using
                }// end of outer using
            }// end of try block
            catch(Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                   $"{TAG}::HasNetworkAccess [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                return false;
            }// end of catch block
        }// end of method HasNetworkAccess
    }// end of class NetworkHelper
}// end of namespace WeatherLion
