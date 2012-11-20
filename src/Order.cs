using System;

namespace mtBot
{
    class Order
    {
        private readonly Planet m_SourcePlanet;
        private readonly Planet m_DestinationPlanet;
        private readonly int m_ShipCount;

        public Order( Planet sourcePlanet, Planet destinationPlanet, int shipCount )
        {
            m_SourcePlanet = sourcePlanet;
            m_DestinationPlanet = destinationPlanet;
            m_ShipCount = shipCount;
        }

        public Planet SourcePlanet
        {
            get { return m_SourcePlanet; }
        }

        public Planet DestinationPlanet
        {
            get { return m_DestinationPlanet; }
        }

        public int ShipCount
        {
            get { return m_ShipCount; }
        }

        public override string ToString( )
        {
            return String.Format("{0} {1} {2}", m_SourcePlanet.Id, m_DestinationPlanet.Id, m_ShipCount);
        }
    }
}