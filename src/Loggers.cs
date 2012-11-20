using System;
using System.Collections.Generic;

namespace mtBot
{
    static class Loggers
    {
        public enum LogLevel
        {
            Trace,
            Info,
            Warn,
            Error
        }

        private static readonly IDictionary<string, Logger> s_Loggers = new Dictionary<string, Logger>();
        private static readonly ILogOutputter s_Outputter;
        private static LogLevel s_CurrentLevel;

        static Loggers( )
        {
            s_Outputter = new LogOutputterFile("mtBot.log");
        }

        static public void DisposeAll()
        {
            IDisposable outputter = s_Outputter as IDisposable;
            if (outputter != null) outputter.Dispose();
        }

        public static LogLevel CurrentLevel
        {
            get { return s_CurrentLevel; }
            set { s_CurrentLevel = value; }
        }

        static public Logger GetLogger( Type type )
        {
            return GetLogger(type.Name);
        }

        static public Logger GetLogger( string area )
        {
            if (s_Loggers.ContainsKey(area)) return s_Loggers[area];

            return s_Loggers[area] = new Logger(area);
        }

        static public ILogOutputter Outputter
        {
            get { return s_Outputter; }
        }
    }
}
