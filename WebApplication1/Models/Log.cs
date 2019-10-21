using System;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class JobLogger
    {

        /// <summary>
        /// 
        /// </summary>
        private static bool _logToFile;
        /// <summary>
        /// 
        /// </summary>
        private static bool _logToConsole;
        /// <summary>
        /// 
        /// </summary>
        private static bool _logMessage;
        /// <summary>
        /// 
        /// </summary>
        private static bool _logWarning;
        /// <summary>
        /// 
        /// </summary>
        private static bool _logError;
        /// <summary>
        /// 
        /// </summary>
        private static bool LogToDatabase;
        //private bool _initialized;

        /// <summary>
        /// Constructor JobLogger 
        /// </summary>
        /// <param name="logToFile">Aplica si el log se guarda en Archivo</param>
        /// <param name="logToConsole">Aplica si se visualiza el mensaje en consola</param>
        /// <param name="logToDatabase">Aplica si se inserta el log en la BD</param>
        /// <param name="logMessage">Aplica Mensaje</param>
        /// <param name="logWarning">Aplica si el mensaje es advertencia</param>
        /// <param name="logError">Aplica si el log es Error</param>
        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            LogToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        /// <summary>
        /// Metodo que registra log en la base de datos y archivo plano
        /// </summary>
        /// <param name="strmessage">Mensaje a insertar</param>
        /// <param name="message">bolean para saber si aplica mensaje</param>
        /// <param name="warning">bolean para saber si aplica advertencia</param>
        /// <param name="error">bolean para saber si aplica Error/param>
        public void LogMessage(string strmessage, bool message, bool warning, bool error)
        {
            strmessage.Trim();
            if (strmessage == null || strmessage.Length == 0)
            {
                return;
            }
            if (!_logToConsole && !_logToFile && !LogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            try
            {
                SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                connection.Open();
                int t = 0;
                if (message && _logMessage)
                {
                    t = 1;
                }
                if (error && _logError)
                {
                    t = 2;
                }
                if (warning && _logWarning)
                {
                    t = 3;
                }
                SqlCommand command = new SqlCommand("Insert into Log Values('" + strmessage + "', " + t.ToString() + ")", connection);

                command.ExecuteNonQuery();

                string l = string.Empty;
                if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("yyyyMMdd") + ".txt"))
                {
                    l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                }
                else
                {
                    System.IO.File.Create(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                }

                if (error && _logError)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }
                if (warning && _logWarning)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }
                if (message && _logMessage)
                {
                    l = l + DateTime.Now.ToShortDateString() + message;
                }

                System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("yyyyMMdd") + ".txt", l);

                if (error && _logError)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (warning && _logWarning)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (message && _logMessage)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(DateTime.Now.ToShortDateString() + message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio el siguiente error:" + ex.Message);
            }            
        }
    }
}