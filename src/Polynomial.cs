using System;
using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    class Polynomial
    {
        private readonly IList<int> m_Coefficients;

        public Polynomial( IList<int> coefficients )
        {
            m_Coefficients = coefficients;
        }

        public Polynomial( params int[] args )
            : this( args.ToList(  ) )
        {
            
        }

        public static double operator* (Polynomial p, double x)
        {
            double ret = 0, pow;
            int len = p.m_Coefficients.Count - 1;

            for( int i = len; i >= 0; i-- )
            {
                pow = Math.Pow( x, len - i );
                ret += pow * p.m_Coefficients[i];
            }

            return ret;
        }

        public static double operator* (double x, Polynomial p)
        {
            return p * x;
        }
    }
}
