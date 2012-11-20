using System;

namespace mtBot
{
    class ConsoleSink : ISourceSink
    {
        public int Source( )
        {
            return Console.Read( );
        }

        public void Sink( string message )
        {
            Console.WriteLine( message );
            Console.Out.Flush( );
        }
    }
}
