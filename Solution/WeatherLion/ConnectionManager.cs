using System;
using System.Data.SQLite;

#region Code Details

///-----------------------------------------------------------------
///   Namespace:      WeatherLion
///   Class:          ConnectionManager
///   Description:    This class is responsible for connecting to a
///                   local SQLite 3 database.
///   Author:         Paul O. Patterson     Date: May 13, 2019
///-----------------------------------------------------------------

#endregion

namespace WeatherLion
{
    /// <summary>
    /// This class is responsible for connecting to a local SQLite 3 database.
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        private const string TAG = "ConnectionManager";
        private static ConnectionManager instance = null;
        private static readonly string SQLITE_CONN_STRING = $"{WeatherLionMain.MAIN_STORAGE_DIR}{WeatherLionMain.MAIN_DATABASE_NAME}";
        private SQLiteConnection conn = null;

        // default constructor
        private ConnectionManager()
        {
        }// end of default constructor

        // gets an instance of the database connection
        public static ConnectionManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ConnectionManager();
            }// end of if block

            return instance;
        }// end of method GetInstance

        // method that opens a database connection
        private bool OpenConnection()
        {
            try
            {
                conn = new SQLiteConnection($"Data Source={@SQLITE_CONN_STRING};FailIfMissing=True;");
                return true;
            }// end of try black
            catch (Exception e)
            {
                UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                    $"{TAG}::OpenConnection [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
                return false;
            }// end of catch block
        }// end of method openConnection

        // method that gets a database connection
        public SQLiteConnection GetConnection()
        {
            if (conn == null)
            {
                if (OpenConnection())
                {
                    return conn;
                }// end of if block
                else
                {
                    return null;
                }// end of else block
            }// end of if block

            return conn;
        }// end of method GetConnection

        // method that closes the database connection
        public void Close()
        {
            try
            {
                conn.Close();
                conn = null;
            }// end of try block 
            catch (Exception e)
            {
                UtilityMethod.LogMessage(UtilityMethod.LogLevel.SEVERE, e.Message,
                    $"{TAG}::Close [line: {UtilityMethod.GetExceptionLineNumber(e)}]");
            }// end of catch block
        }// end of method close()

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                conn.Close();
            }// end of if block            
        }// end of virtual method Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }// end of method Dispose
    }// end of class ConnectionManager
}// end of namespace WeatherLion
