using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mtBot
{
    class Planet : ITurnable<Planet>, IOwned, IEquatable<Planet>
    {
        private static readonly Logger s_Logger = Loggers.GetLogger(typeof (Planet));

        private readonly int m_Id;
        private readonly Player m_Owner;
        private readonly int m_ShipCount;
        private readonly int m_GrowthRate;
        private readonly double m_X;
        private readonly double m_Y;
        private readonly GameState m_Gs;
        private readonly PlanetGeometryCache m_GeomCache = ObjectFactory.Get<PlanetGeometryCache>( );

        public Planet(
            int id,
            Player owner,
            int shipCount,
            int growthRate,
            double x,
            double y,
            GameState gs
            )
        {
            m_Id = id;
            m_Owner = owner;
            m_ShipCount = shipCount;
            m_GrowthRate = growthRate;
            m_X = x;
            m_Y = y;
            m_Gs = gs;

            s_Logger.Trace(String.Format("new {0}", this));
        }

        public int Id
        {
            get { return m_Id; }
        }

        public Player Owner
        {
            get { return m_Owner; }
        }

        public int ShipCount
        {
            get { return m_ShipCount; }
        }

        public int GrowthRate
        {
            get { return m_GrowthRate; }
        }

        public double X
        {
            get { return m_X; }
        }

        public double Y
        {
            get { return m_Y; }
        }

        public int DistanceTo( Planet other )
        {
            /* Defer to the per-process instance distance cache */
            return m_GeomCache[this, other];
        }

        public int FuturedAvailableShips
        {
            get
            {
                int availShips = 0;

                /* We used to keep inspecting futures until there were no more fleets in flight, but this doesn't halt when the stream prediction kicks in. */
                /* Consider turn + 1, i.e. when all fleets have arrived */
                int turnCount = m_Gs.Fleets.Count == 0 ? 1 : m_Gs.Fleets.Max( f => f.TurnsRemaining ) + 1;

                try
                {
                    availShips =
                        m_Gs.Futures.Take( turnCount )
                        .Where(gs => gs.Planets[m_Id].Owner == Players.Singleton.Me)
                        .Select(gs => gs.Planets[m_Id].ShipCount)
                        .Min();
                }
                catch( InvalidOperationException )
                {
                }

                return availShips;                
            }
        }

        /* Calculate new state of planet, one turn after gs */
        public Planet Turn( GameState newGs )
        {
            int newShipCount = m_ShipCount;
            Player newOwner = m_Owner;

            /* Departure */
            /* Currently we don't render any orders pending in gs... */

            /* Advancement */
            if (m_Owner != Players.Singleton.Neutral)
            {
                newShipCount += m_GrowthRate;
            }

            /* Arrival */
            /* Work out aggregate sizes of incoming fleets */
            Dictionary<Player, int> ships = new Dictionary<Player, int>(3) //This structure is a set<"force"> from the spec
            {
                { Players.Singleton.Neutral,
                  0 /* Neutral player has no fleets in flight */ },
                { Players.Singleton.Me, 
                  m_Gs.Fleets.Mine. 
                      Where( f => f.DestinationPlanet == this && f.TurnsRemaining == 1 ).
                      Sum( f => f.ShipCount ) },
                { Players.Singleton.You,
                  m_Gs.Fleets.Yours.
                      Where( f => f.DestinationPlanet == this && f.TurnsRemaining == 1 ).
                      Sum( f => f.ShipCount ) }
            };

            /* Add forces already on planet */
            ships[m_Owner] += newShipCount;

            IOrderedEnumerable<KeyValuePair<Player, int>> orderedShips = ships.OrderByDescending( x => x.Value );

            newShipCount = orderedShips.ElementAt( 0 ).Value - orderedShips.ElementAt( 1 ).Value;
            Debug.Assert( newShipCount >= 0 );

            if (newShipCount != 0)
            {
                newOwner = orderedShips.ElementAt( 0 ).Key;
            }

            return new Planet(m_Id, newOwner, newShipCount, m_GrowthRate, m_X, m_Y, newGs);
        }

        public override string ToString()
        {
            return String.Format("Planet id {0} owner {1} ships {2} +{3} ({4},{5})",
                                 m_Id, m_Owner, m_ShipCount, m_GrowthRate, m_X, m_Y);
        }

        public bool Equals(Planet other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return (obj is Planet) ? this == (Planet)obj : false;
        }

        public override int GetHashCode()
        {
            return m_Id;
        }

        public static bool operator==( Planet a, Planet b )
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            /* By default, two planets are the same if they have the same id. It does *not* matter if they're from the same point in time or not. */
            return a.m_Id == b.m_Id;
        }
        public static bool operator!=( Planet a, Planet b )
        {
            return !(a == b);
        }

        class PlanetTemporalEquals : IEqualityComparer<Planet>
        {
            /* NB: this will NOT compare equal two planets from two different GSs that represent the same turn number,
             *     as will happen if they are futured from two different points. */
            public bool Equals(Planet a, Planet b)
            {
                return a.Id == b.Id &&
                       a.m_Gs == b.m_Gs;
            }

            public int GetHashCode(Planet obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
