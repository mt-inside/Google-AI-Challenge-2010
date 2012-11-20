using System;

namespace mtBot
{
    public class Fleet
    {
        private readonly Player m_Owner;
        private readonly int m_ShipCount;
        private readonly Planet m_SourcePlanet;
        private readonly Planet m_DestinationPlanet;
        private readonly int m_TotalTripLength;
        private readonly int m_TurnsRemaining;

        public Fleet(GameState gs,
                     Player owner,
                     int shipCount,
                     int sourcePlanet,
                     int destinationPlanet,
                     int totalTripLength,
                     int turnsRemaining)
        {
            m_Owner = owner;
            m_ShipCount = shipCount;
            m_SourcePlanet = gs.Planets[sourcePlanet];
            m_DestinationPlanet = gs.Planets[destinationPlanet];
            m_TotalTripLength = totalTripLength;
            m_TurnsRemaining = turnsRemaining;
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

        public override string ToString()
        {
            return String.Format("Fleet owner {2} ships {3} {0}->{1} {4}/{5}",
                m_SourcePlanet, m_DestinationPlanet, m_Owner, m_ShipCount, m_TurnsRemaining, m_TotalTripLength);
        }
    }
}
