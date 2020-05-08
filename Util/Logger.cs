using System;
using System.Reflection;

namespace DistanceLoader.Util
{
    public class Logger
    {
        public static Logger Instance = _instance ?? (_instance = new Logger());
        private static readonly Logger _instance;
        private const string LogFileName = "DistanceLoader.log";

        private readonly object _lockable = new object();

        public Logger()
        {
            System.IO.File.WriteAllText(LogFileName, "");
        }

        public void Log(string message, Exception exception = null)
        {
            lock (_lockable)
            {
                var dt = DateTime.Now;
                var compiledMessage = $"[{dt.Year}/{dt.Month}/{dt.Day} {dt.Hour}:{dt.Minute}:{dt.Second}] | {message}";

                try
                {
                    System.IO.File.AppendAllText(LogFileName, $"{compiledMessage}{Environment.NewLine}");

                    if (exception != null)
                    {

                    }
                }
                catch (Exception ex)
                {
                    // what do now?
                }
            }
        }
    }
}
