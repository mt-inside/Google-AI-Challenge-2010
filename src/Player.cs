using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    class Player
    {
        private readonly int m_Id;

        public Player( int id )
        {
            m_Id = id;
        }

        public int Id
        {
            get { return m_Id; }
        }

        public bool IsAlive( GameState pw )
        {
            return pw.Planets.Any(x => x.Owner == this) || pw.Fleets.Any(x => x.Owner == this);
        }

        public override string ToString()
        {
            return new List<string>() {"Neutral", "Me", "You"}[m_Id];
        }
    }
}
