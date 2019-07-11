using System;
using System.Net;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          HttpHelper
///   Description:    Helper class for downloading data form a remote
///                   web server.
///   Author:         Paul O. Patterson     Date: May 17, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Helper class for downloading data form a remote web server.
    /// </summary>
    public class HttpHelper
    {
        private const string TAG = "HttpHelper";

        public static string DownloadUrl(string address)
        {
            string serviceData = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            try
            {
                using (WebClient client = new WebClient())
                {
                    serviceData = client.DownloadString(address);
                }// end of outer using                                
            }// end of try block
            catch (WebException we)
            {
                UtilityMethod.LogMessage("severe", we.Message,
                    $"{TAG}::DownloadUrl [line: {UtilityMethod.GetExceptionLineNumber(we)}]");
            }// end of catch block
            catch (Exception e)
            {
                UtilityMethod.LogMessage("severe", e.Message,
                    $"{TAG}::DownloadUrl [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block

            return serviceData;
        }// end of method DownloadUrl        
    }// end of class HttpHelper
}// end of namespace WeatherLion
