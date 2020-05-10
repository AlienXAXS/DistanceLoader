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

        private int _incrementValue = 0;

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
                        System.IO.File.AppendAllText(LogFileName, $"{exception}{Environment.NewLine}");
                        if ( exception.InnerException != null )
                            System.IO.File.AppendAllText(LogFileName, $"{exception.InnerException}{Environment.NewLine}");
                    }
                }
                catch (Exception ex)
                {
                    // what do now?
                }
            }
        }
        
        public void IncrementLogValue(string tag, int value = -1)
        {
            if ( value != -1 )
                _incrementValue = value;

            Log($"< IncrementValue | {tag} > {_incrementValue}");

            _incrementValue++;
        }
    }
}
