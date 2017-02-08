using FontColorExperiment.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace FontColorExperiment
{
    /// <summary>
    /// The main control class for the server.
    /// Handles all data generated and used by the server.
    /// </summary>
    public static class Main
    {
        /// <summary>
        /// The path to the Data folder.
        /// This is where the experiment and server data is stored.
        /// </summary>
        private static readonly string DataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        /// <summary>
        /// The file that stores the full server data.
        /// </summary>
        private const string ServerDataFile = "ServerData.json";
        /// <summary>
        /// The full path to the server data file.
        /// </summary>
        private static readonly string ServerDataPath = Path.Combine(DataFolder, ServerDataFile);

        /// <summary>
        /// The file that stores the initial exerpiment data.
        /// Only used if the server data file does not exist.
        /// </summary>
        private const string ExperimentFile = "Experiment.json";
        /// <summary>
        /// The full path to the experiment data file.
        /// </summary>
        private static readonly string ExperimentPath = Path.Combine(DataFolder, ExperimentFile);

        /// <summary>
        /// The main server data manager.
        /// </summary>
        public static ServerManager Manager { get; private set; }
        /// <summary>
        /// The prefix to each log statement.
        /// </summary>
        public static string LogPrefix { get; set; } = "[MAIN] => ";
        /// <summary>
        /// The logger object given by ASP.NET Core
        /// </summary>
        private static ILogger m_logger;
        /// <summary>
        /// The full log stored as a string in order to use in the admin page.
        /// </summary>
        public static string LogStore { get; private set; } = "";

        /// <summary>
        /// The object to use as a locking object for the manager changed event.
        /// </summary>
        private static object m_managerLock = new object();

        /// <summary>
        /// Initializes the server data.
        /// </summary>
        public static void Init()
        {
            Log("Server process started, initializing...");
            // creates the data folder if it doesn't exist
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
                Log("Created data directory");
            }
            // loads the server from the server data file if it exists
            // otherwise, loads from the experiment data
            if (File.Exists(ServerDataPath))
            {
                try
                {
                    Manager = ServerManager.FromFile(ServerDataPath);
                    Log("Loaded server data from JSON file");
                }
                catch (Exception ex)
                {
                    Log("Unable to load from file, starting from nothing");
                    Log(ex);
                    InitNewManager();
                }
            }
            else
            {
                InitNewManager();
            }
            Manager.Changed += Manager_Changed;
            Log("Initializing complete");
        }

        /// <summary>
        /// Initializes the server manager from the experiment data file.
        /// If the file doesn't exist, then an empty manager is created.
        /// </summary>
        private static void InitNewManager()
        {
            Manager = new ServerManager();
            try
            {
                if (File.Exists(ExperimentPath))
                    Manager.LoadExperiment(ExperimentPath);
            }
            catch (Exception ex)
            {
                Log("Unable to load experiments, skipping...");
                Log(ex);
            }
            Log("Created new server data");
        }

        /// <summary>
        /// The event handler for the manager changed event.
        /// Saves the current state into the server data file to
        /// preserve changes.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        public static void Manager_Changed(object sender, EventArgs e)
        {
            lock(m_managerLock)
            {
                Manager.SaveState(ServerDataPath);
            }
        }

        /// <summary>
        /// Sets the experiment data file from received string.
        /// </summary>
        /// <param name="data">The data to set.</param>
        public static void SetExperimentData(string data)
        {
            File.Delete(ServerDataPath);
            Manager.ReplaceExperimentData(ExperimentPath, data);
        }

        /// <summary>
        /// Sets the logger object.
        /// </summary>
        /// <param name="logger">The ILogger object to use.</param>
        public static void SetLogger(ILogger logger)
        {
            m_logger = logger;
        }

        /// <summary>
        /// Writes the string to the log.
        /// </summary>
        /// <param name="data">The string to log.</param>
        public static void Log(string data)
        {
            data = LogPrefix + data;
            Console.WriteLine(data);
            System.Diagnostics.Debug.WriteLine(data);
            LogStore += data + Environment.NewLine;
            if (m_logger != null)
                m_logger.LogInformation(data);
        }

        /// <summary>
        /// Writes the string to the log using the format and objects.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="args">The objects to use in the <c>string.Format</c></param>
        public static void Log(string format, params object[] args) => Log(string.Format(format, args));

        /// <summary>
        /// Writes the exception to the log.
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex) => Log("An exception occured during runtime, details:{0}{1}", Environment.NewLine, ex.ToString());

        // debug logs that are unused
#if DEBUG

        public static void LogD(string data)
        {
            data = LogPrefix + data;
            Console.WriteLine(data);
            System.Diagnostics.Debug.WriteLine(data);
            LogStore += data + Environment.NewLine;
            if (m_logger != null)
                m_logger.LogInformation(data);
        }

        public static void LogD(string format, params object[] args) => Log(string.Format(format, args));

        public static void LogD(Exception ex) => LogD("An exception occured during runtime, details:{0}{1}", Environment.NewLine, ex.ToString());

#else

        public static void LogD(string data) { }

        public static void LogD(string format, params object[] args) { }

        public static void LogD(Exception ex) { }

#endif
    }
}
