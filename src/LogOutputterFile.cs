using System;
using System.IO;

namespace mtBot
{
    class LogOutputterFile : ILogOutputter, IDisposable
    {
        private readonly StreamWriter m_File;
        private bool m_Disposed = false;

        public LogOutputterFile( string path )
        {
            m_File = new StreamWriter(path, false);
        }

        public void Output(string message)
        {
            m_File.WriteLine(message);
            m_File.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LogOutputterFile()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_File.Dispose();
                }

                m_Disposed = true;
            }
        }
    }
}
