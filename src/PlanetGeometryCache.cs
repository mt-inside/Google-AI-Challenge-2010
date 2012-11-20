using System;

namespace mtBot
{
    class PlanetGeometryCache
    {
        private bool m_Initialised = false;
        private int m_NumPlanets;
        private int[,] m_DistanceMatrix;

        public bool Initialised
        {
            get { return m_Initialised; }
        }

        public void Initialise(int numPlanets)
        {
            if (m_Initialised) throw new InvalidStateException();

            m_NumPlanets = numPlanets;
            m_DistanceMatrix = new int[numPlanets, numPlanets];

            m_Initialised = true;
        }

        public int this[Planet p1, Planet p2]
        {
            get
            {
                if (!m_Initialised) throw new InvalidStateException();

                int x = p1.Id, y = p2.Id;

                /* Use only half of the matrix to avoid double calculations (DistanceTo is a symmetric relation) */
                if( x + y > m_NumPlanets )
                {
                    int tmp = x;
                    x = y;
                    y = tmp;
                }

                int ret = m_DistanceMatrix[x, y];

                if( ret == 0 )
                {
                    double dx = p1.X - p2.X,
                           dy = p1.Y - p2.Y;

                    ret = m_DistanceMatrix[p1.Id, p2.Id] = (int) Math.Ceiling( Math.Sqrt( dx*dx + dy*dy ) );
                }

                return ret;
            }
        }
    }
}