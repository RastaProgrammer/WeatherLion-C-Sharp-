using System;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          IconUpdateService
///   Description:    Performs updates to the icons displayed on the
///                   widget.
///   Author:         Paul O. Patterson     Date: June 26, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Performs updates to the icons displayed on the widget.
    /// </summary>
    public class IconUpdateService
    {
        private const string TAG = "IconUpdateService";
        WidgetForm currentWidget;

        public IconUpdateService(WidgetForm widget)
        {
            currentWidget = widget;
            Run();            
        }// end of default constructor

        public void Run()
        {            
            try
            {                  
                // keep track of Internet connectivity
                if (UtilityMethod.TimeForConnectivityCheck())
                {
                    WeatherLionMain.connectedToInternet = UtilityMethod.HasInternetConnection();
                }// end of if block

                // If the program is not connected to the Internet, wait for a connection
                if (WeatherLionMain.connectedToInternet)
                {
                    // if there was no previous Internet connection, check for a return in connectivity
                    // and refresh the widget
                    if (currentWidget.usingPreviousData && UtilityMethod.UpdateRequired())
                    {
                        // run the weather service
                        WidgetUpdateService ws = new WidgetUpdateService(false, currentWidget);
                        ws.Run();
                    }// end of if block
                }// end of if block

                if (WeatherLionMain.connectedToInternet)
                {
                    currentWidget.picOffline.Visible = false;
                }// end of if block
                else
                {
                    currentWidget.picOffline.Visible = true;
                }// end of else block

                currentWidget.CheckAstronomy();
            }// end of try block 
            catch (Exception e)
            {
                UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message, $"{TAG}::Run");
            }// end of catch block            
        }// end of method Run
    }// end of class IconUpdateService
}// end of namespace WeatherLion
