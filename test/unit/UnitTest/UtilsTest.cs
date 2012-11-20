using System.Collections.Generic;
using System.Diagnostics;
using mtBot;

namespace UnitTest
{
    static class UtilsTest
    {
        static internal bool Test( )
        {
            return TestPolynomial( );
        }

        static private bool TestPolynomial( )
        {
            Polynomial p1 = new Polynomial( new List<int>() { 2, 3, 5 } ); /* 2x^2 + 3x + 5 */

            Debug.Assert(p1 * 0 ==  5);
            Debug.Assert(p1 * 1 == 10);
            Debug.Assert(p1 * 2 == 19);
            Debug.Assert(p1 * 3 == 32);

            Debug.Assert(0 * p1 == 5);
            Debug.Assert(1 * p1 == 10);
            Debug.Assert(2 * p1 == 19);
            Debug.Assert(3 * p1 == 32);

            Polynomial p2 = new Polynomial( 2, 3, 5 ); /* 2x^2 + 3x + 5 */

            Debug.Assert(p2 * 0 == 5);
            Debug.Assert(p2 * 1 == 10);
            Debug.Assert(p2 * 2 == 19);
            Debug.Assert(p2 * 3 == 32);

            Polynomial p3 = new Polynomial(20, 30, 50); /* 20x^2 + 30x + 50 */

            Debug.Assert(p3 * 0 == 50);
            Debug.Assert(p3 * 1 == 100);
            Debug.Assert(p3 * 2 == 190);
            Debug.Assert(p3 * 3 == 320);

            return true;
        }
    }
}
