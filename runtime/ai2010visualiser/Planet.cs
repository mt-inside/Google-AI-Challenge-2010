using System;
using System.Diagnostics;
using System.Linq;

namespace mtBot
{
    public class Planet
    {
        private readonly int m_Id;
        private readonly Player m_Owner;
        private readonly int m_ShipCount;
        private readonly int m_GrowthRate;
        private readonly double m_X;
        private readonly double m_Y;

        public Planet(
            int id,
            Player owner,
            int shipCount,
            int growthRate,
            double x,
            double y
            )
        {
            m_Id = id;
            m_Owner = owner;
            m_ShipCount = shipCount;
            m_GrowthRate = growthRate;
            m_X = x;
            m_Y = y;
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

        public override string ToString()
        {
            return String.Format("Planet id {0} owner {1} ships {2} +{3} ({4},{5})",
                                 m_Id, m_Owner, m_ShipCount, m_GrowthRate, m_X, m_Y);
        }
    }
}
