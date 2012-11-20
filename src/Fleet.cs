using System;

namespace mtBot
{
    class Fleet : ITurnable<Fleet>, IOwned
    {
        private static readonly Logger s_Logger = Loggers.GetLogger(typeof(Fleet));

        private readonly Player m_Owner;
        private readonly int m_ShipCount;
        private readonly Planet m_SourcePlanet;
        private readonly Planet m_DestinationPlanet;
        private readonly int m_TotalTripLength;
        private readonly int m_TurnsRemaining;
        private readonly GameState m_Gs;

        public Fleet(Player owner,
                     int shipCount,
                     Planet sourcePlanet,
                     Planet destinationPlanet,
                     int totalTripLength,
                     int turnsRemaining,
                     GameState gs)
        {
            m_Owner = owner;
            m_ShipCount = shipCount;
            m_SourcePlanet = sourcePlanet;
            m_DestinationPlanet = destinationPlanet;
            m_TotalTripLength = totalTripLength;
            m_TurnsRemaining = turnsRemaining;
            m_Gs = gs;

            s_Logger.Trace(String.Format("new {0}", this));
        }

        public Player Owner
        {
            get { return m_Owner; }
        }

        public int ShipCount
        {
            get { return m_ShipCount; }
        }

        public Planet SourcePlanet
        {
            get { return m_SourcePlanet; }
        }
        public Planet DestinationPlanet
        {
            get { return m_DestinationPlanet; }
        }

        public int TotalTripLength
        {
            get { return m_TotalTripLength; }
        }
        public int TurnsRemaining
        {
            get { return m_TurnsRemaining; }
        }
        public int TurnsTravelled
        {
            get { return m_TotalTripLength - m_TurnsRemaining; }
        }

        public Fleet Turn( GameState newGs )
        {
            Fleet newFleet = null;
            int newTurns;

            /* Departure */
            /* Irellevant for a fleet that already exists */

            /* Advancement */
            newTurns = m_TurnsRemaining - 1;

            /* Arrival */
            if (newTurns != 0)
            {
                newFleet = new Fleet(m_Owner, m_ShipCount, m_SourcePlanet, m_DestinationPlanet, m_TotalTripLength, newTurns, newGs);
            }

            return newFleet;
        }

        public override string ToString()
        {
            return String.Format("Fleet owner {2} ships {3} {0}->{1} {4}/{5}",
                m_SourcePlanet, m_DestinationPlanet, m_Owner, m_ShipCount, m_TurnsRemaining, m_TotalTripLength);
        }
    }
}
