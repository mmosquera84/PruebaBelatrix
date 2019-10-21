using System;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class JobLogger
    {
        /// <summary>
        ///  Obtiene y Establece el valor de Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogToFile
        /// </summary>
        public bool LogToFile { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogToConsole
        /// </summary>
        public bool LogToConsole { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogMessage
        /// </summary>
        public bool LogMessage { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogWarning
        /// </summary>
        public bool LogWarning { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogError
        /// </summary>
        public bool LogError { get; set; }
        /// <summary>
        /// Obtiene y Establece el valor de LogToDatabase
        /// </summary>
        public bool LogToDatabase { get; set; }
        //private bool _initialized;

        /// <summary>
        /// Constructor JobLogger 
        /// </summary>
        public JobLogger()
        {
            
        }

        /// <summary>
        /// Metodo que registra log en la base de datos y archivo plano
        /// </summary>       
        public void AddLogMessage()
        {
            Message.Trim();
            if (Message == null || Message.Length == 0)
            {
                return;
            }
            if (!LogToConsole && !LogToFile && !LogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if (!LogError && !LogMessage && !LogWarning)
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            try
            {
                SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                connection.Open();
                int t = 0;
                if (LogMessage)
                {
                    t = 1;
                }
                if (LogError)
                {
                    t = 2;
                }
                if (LogWarning)
                {
                    t = 3;
                }
                SqlCommand command = new SqlCommand("Insert into Log Values('" + Message + "', " + t.ToString() + ")", connection);

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

                if (LogError)
                {
                    l = l + DateTime.Now.ToShortDateString() + Message;
                }
                if (LogWarning)
                {
                    l = l + DateTime.Now.ToShortDateString() + Message;
                }
                if (LogMessage)
                {
                    l = l + DateTime.Now.ToShortDateString() + Message;
                }

                System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("yyyyMMdd") + ".txt", l);

                if (LogError)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (LogWarning)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (LogMessage)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(DateTime.Now.ToShortDateString() + Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio el siguiente error:" + ex.Message);
            }            
        }
    }
}