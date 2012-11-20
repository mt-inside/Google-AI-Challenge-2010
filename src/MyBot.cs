using System;

namespace mtBot
{
    static class mtBot
    {
        static readonly Logger s_Logger = Loggers.GetLogger( typeof( mtBot ) );

        public static void Main( string[] args )
        {
            switch( args.Length )
            {
                case 1:
                    if( args[0] == "/tcpio")
                    {
                        ObjectFactory.Get<Settings>( ).TcpIo = true;
                    }
                    break;
            }

            int c;
            string line = "";
            string message = "";
            ISourceSink source = ObjectFactory.Get<ISourceSink>();
            int turn = 0;

            Loggers.CurrentLevel = Loggers.LogLevel.Info;

            while( ( c = source.Source(  ) ) > 0)
            {
                switch (c)
                {
                    case '\r':
                        /* Swallow */
                        break;

                    case '\n':
                        if (line == "go")
                        {
                            GameState gs = new GameState(message, turn++);

                            if( !ObjectFactory.Get<PlanetGeometryCache>(  ).Initialised )
                            {
                                ObjectFactory.Get<PlanetGeometryCache>(  ).Initialise( gs.Planets.Count );
                            }

                            /* We get given winning states at the end of games, where there's nothing for us to do.
                             * To avoid potentially embarrassing problems with the logic infinite-looping, don't call it */
                            if (gs.EndState)
                            {
                                s_Logger.Error( String.Format( "{0} Wins! ", gs.Winner ) );
                                gs.Orders.Done(  );
                                break;
                            }

                            DateTime start = DateTime.Now;
                            Strategy4.Turn(gs);
                            DateTime end = DateTime.Now;

                            s_Logger.Info( String.Format( "Turn {0} took {1}ms", turn, (end - start).TotalMilliseconds ) );

                            message = "";
                        }
                        else
                        {
                            message += line + "\n";
                        }
                        line = "";
                        break;

                    default:
                        line += (char)c;
                        break;
                }
            }

            AppDomain.CurrentDomain.DomainUnload += (x, y) => Loggers.DisposeAll();
        }
    }
}