using System.Collections.Generic;

namespace mtBot
{
    static public class Players
    {
        private static readonly IList<Player> m_Players = new List<Player>(3);

        static Players( )
        {
            for (int i = 0; i < 3; i++)
            {
                m_Players.Insert(i, new Player(i));
            }
        }

        /* This is the monad from the ugly world of ints to our lovely OO nirvana */
        static public Player GetById(int id) /* C# fail - no static indexers */
        {
            return m_Players[id];
        }

        static public Player Neutral
        {
            get { return m_Players[0]; }
        }
        static public Player Me
        {
            get { return m_Players[1]; }
        }
        static public Player You
        {
            get { return m_Players[2]; }
        }
    }
}
