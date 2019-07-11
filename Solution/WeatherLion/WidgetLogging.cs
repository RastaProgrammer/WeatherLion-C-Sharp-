using System;
using System.IO;
using System.Text;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          WidgetLogging
///   Description:    This class is responsible for logging data
///                   to local log files.
///   Author:         Paul O. Patterson     Date: May 15, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    ///  Performs logging functionality for the program.
    /// </summary>
    public class WidgetLogging
    {
        private static readonly string logDate = string.Format("{0:MMM-dd-yy}", DateTime.Now);
        private static readonly string mainLogDirectory = @"res/log";
        private static readonly string htmlLogDirectory = $@"{mainLogDirectory}/html";
        private static readonly string txtLogDirectory = $@"{mainLogDirectory}/txt";        
        private readonly string previousHtmlLog = $@"{mainLogDirectory}/html/WidgetLog_{logDate}.html";
        private readonly string previousTextLog = $@"{mainLogDirectory}/txt/WidgetLog_{logDate}.txt";       

        public WidgetLogging()
        {
            // ensure that all neccessary log directories are in place
            if (!Directory.Exists(mainLogDirectory))
            {
                Directory.CreateDirectory(mainLogDirectory);
                Directory.CreateDirectory(htmlLogDirectory);
                Directory.CreateDirectory(txtLogDirectory);
            }// end of if block

            if (!File.Exists(previousHtmlLog))
            {
                using (var stream = File.Create(previousHtmlLog)) { };
            }// end of if block

            if (!File.Exists(previousTextLog))
            {
                using (var stream = File.Create(previousTextLog)) { };
            }// end of if block
        }// default constructor

        private string HTMLFormatter(string message, string level, string source)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<tr>");

            if (level.ToLower().Equals("warning"))
            {
                html.Append("<td style=\"color:orange\">");
                html.Append("<b>");
                html.Append(level.ToUpper());
                html.Append("</b>");
            }// end of if block
            else if (level.ToLower().Equals("severe"))
            {
                html.Append("<td style=\"color:red\">");
                html.Append("<b>");
                html.Append(level.ToUpper());
                html.Append("</b>");
            }// end of else if block
            else
            {
                html.Append("\t<td>");
                html.Append(level.ToUpper());
            }// end of else block
           
            html.Append("</td>\n");
            html.Append("\t<td>");
            html.Append(string.Format("{0:MMM dd,yyyy h:mm tt}", DateTime.Now));
            html.Append("</td>\n");
            html.Append("\t<td>");
            html.Append(source);
            html.Append("</td>\n");
            html.Append("\t<td>");
            html.Append(message);
            html.Append("</td>\n");
            html.Append("</tr>\n");

            return html.ToString();
        }// end of method HTMLFormatter

        public void Log(string message, string level, string source)
        {
            string htmlHead = GetHTMLHead();
            string htmlFooter = GetHTMLFooter();
            string text = GetHTMLHead();
            bool appendHTML = File.Exists(previousHtmlLog);
            bool appendTEXT = File.Exists(previousTextLog);

            // Write the log to the HTML file
            using (var logWriter = new StreamWriter(previousHtmlLog, appendHTML))
            {
                logWriter.WriteLine(htmlHead);
                logWriter.WriteLine(HTMLFormatter(message, level, source));
                logWriter.WriteLine(htmlFooter);
            }// end of using block

            // Write the log to the text file
            using (var logWriter = new StreamWriter(previousTextLog, appendTEXT))
            {
                logWriter.WriteLine(GetLogText(message, level, source));                
            }// end of using block           
        }// end of method Log

        private string GetHTMLHead()
        {
            // a file with three (3) bytes is empty and has this size because it previously contained data
            bool fileContainsData = new FileInfo(previousHtmlLog).Length  > 0 ? true : false;

            if (!fileContainsData)
            {
                return "<html>"
                        + "<head>"
                        + "<style type='text/css'>"
                        + "table { width: 100%; border-collapse: collapse; }\n"
                        + "th { font:bold 10pt Tahoma; }\n"
                        + "td { font:normal 10pt Tahoma; }\n"
                        + "h1 {font:normal 11pt Tahoma;}\n"
                        + "</style>\n"
                        + "</head>\n"
                        + "<body>\n"
                        + "<table border=\"0\" cellpadding=\"5\" cellspacing=\"3\">\n"
                        + "<tr align=\"left\">\n"
                        + "\t<th style=\"width:10%\">Log Level</th>\n"
                        + "\t<th style=\"width:15%\">Time</th>\n"
                        + "\t<th style=\"width:15%\">Source</th>\n"
                        + "\t<th style=\"width:75%\">Log Message</th>\n"
                        + "</tr>\n";
            }// end of if block
            else
            {
                return "<table border=\"0\" cellpadding=\"5\" cellspacing=\"3\">\n"
                        + "<tr align=\"left\">\n"
                        + "\t<th style=\"width:10%\"></th>\n"
                        + "\t<th style=\"width:15%\"></th>\n"
                        + "\t<th style=\"width:15%\"></th>\n"
                        + "\t<th style=\"width:75%\"></th>\n"
                        + "</tr>\n";              
            }// end of else block            
        }// end of method GetHTMLHead

        private string GetLogText(string message, string level, string source)
        {
            string dateString = string.Format("{0:MMM dd, yyyy}", DateTime.Now);

            return $"{dateString} {source} \n{level.ToUpper()}: {message}";
        }// end of method GetLogText

        private string GetHTMLFooter()
        {
            if (!File.Exists(previousHtmlLog))
            {
                return "</table>\n</body>\n</html>";
                
            }// end of if block
            else
            {
                return "</table>\n";               
            }// end of else block          
        }// end of method WriteHTMLFooter

       
    }// end of class WidgetLogging
}// end of namespace WeatherLion
