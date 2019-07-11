using System;
using System.Drawing;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          ConfigurationDataObject
///   Description:    This class is used as a model for storing data 
///                   about the program's configuration to local 
///                   storage.
///   Author:         Paul O. Patterson     Date: May 13, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// Stores the program's configuration data
    /// </summary>
    [Serializable()]
    public class ConfigurationDataObject
    {
        private Point m_oStartPos;
        
        public Point StartPos
        {
            get { return m_oStartPos; }
            set { m_oStartPos = value; }
        }
    }// end of class ConfigurationDataObject
}// end of namespace WeatherLion
