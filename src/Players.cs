using System.Collections.Generic;

namespace mtBot
{
    class Players
    {
        private static Players s_Singleton;
        static public Players Singleton
        {
            get
            {
                if( s_Singleton == null )
                {
                    s_Singleton = new Players( );
                }

                return s_Singleton;
            }
        }


        private readonly IList<Player> m_Players = new List<Player>(3);

        private Players( )
        {
            for (int i = 0; i < 3; i++)
            {
                m_Players.Insert(i, new Player(i));
            }
        }

        /* This is the monad from the ugly world of ints to our lovely OO nirvana */
        public Player this[ int id ]
        {
            get
            {
                return m_Players[id];    
            }
        }

        public Player Neutral
        {
            get { return m_Players[0]; }
        }
        public Player Me
        {
            get { return m_Players[1]; }
        }
        public Player You
        {
            get { return m_Players[2]; }
        }
    }
}
