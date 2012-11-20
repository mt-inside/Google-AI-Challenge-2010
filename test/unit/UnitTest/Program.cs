using System.Diagnostics;
using mtBot;

namespace UnitTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory.Get<PlanetGeometryCache>().Initialise( 50 ); // Should be enough

            Debug.Assert( UtilsTest.Test( ) );
            Debug.Assert( PlanetTest.Test( ) );
            Debug.Assert( GameStateTest.Test( ) );
            
            //TODO: test availships, hard!
        }
    }
}
