using System;
using System.Threading;

namespace mtBot
{
    class Logger
    {
        private readonly string m_Area;

        public Logger( string area )
        {
            m_Area = area;
        }

        public void Error( string message )
        {
            if( Loggers.LogLevel.Error >= Loggers.CurrentLevel )
            {
                Output("ERROR", message);    
            }
        }
        public void Warn( string message )
        {
            if( Loggers.LogLevel.Warn >= Loggers.CurrentLevel )
            {
                Output( "WARN", message );
            }
        }
        public void Info( string message )
        {
            if( Loggers.LogLevel.Info >= Loggers.CurrentLevel )
            {
                Output( "INFO", message );
            }
        }
        public void Trace( string message )
        {
            if (Loggers.LogLevel.Trace >= Loggers.CurrentLevel)
            {
                Output( "TRACE", message );
            }
        }

        private void Output( string level, string message )
        {
            Loggers.Outputter.Output(String.Format("({0},{1}) [{2} {3}] {4}", DateTime.Now, Thread.CurrentThread.ManagedThreadId, m_Area, level, message));
        }
    }
}
