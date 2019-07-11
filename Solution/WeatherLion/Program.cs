using System;
using System.Windows.Forms;

namespace WeatherLion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // use the operating system's look and feel
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new PreferencesForm());
            Application.Run(new WidgetForm());
        }// end of method Main

    }// end of class Program
}// end of namespace WeatherLion
