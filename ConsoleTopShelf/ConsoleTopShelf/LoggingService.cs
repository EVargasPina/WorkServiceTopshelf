using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Topshelf;

namespace ConsoleTopShelf
{
    class LoggingService : ServiceControl
    {
        private const string _logFileLocation = @"C:\Users\ENZO\Documents\servicelog.txt";

        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }

        public bool Start(HostControl hostControl)
        {
            Log("Starting");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log("Stopping");
            return true;
        }
    }
}
