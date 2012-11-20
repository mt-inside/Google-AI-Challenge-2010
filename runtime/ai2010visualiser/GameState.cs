using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace mtBot
{
    public class GameState
    {
        private readonly Planets m_Planets = new Planets();
        private readonly Fleets m_Fleets = new Fleets();

        public GameState(  )
        {
        }

        public Planets Planets
        {
            get { return m_Planets; }
        }
        public Fleets Fleets
        {
            get { return m_Fleets; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            m_Planets.Select( x => sb.Append( x ) );
            m_Fleets.Select( x => sb.Append( x ) );

            return sb.ToString();
        }
    }
}