using System.Diagnostics;
using System.Linq;
using mtBot;

namespace UnitTest
{
    static class GameStateTest
    {
        static public bool Test( )
        {
            return TestNoFleets( ) &&
                   TestOneFleetInsufficient( ) &&
                   TestOneFleetNeutralise( ) &&
                   TestOneFleetOverpower( ) &&
                   EndStates( );

            //TODO: test menage-a-trios on a neutral planet
            //TODO: test multiple fleets from the same player arriving at a planet.
        }

        static private bool TestNoFleets()
        {
            GameState gs = new GameState( TestStrings.GameStateNoFleets, 0 ),
                      gs1,
                      gs5,
                      gs1_4;

            Planet neutralPlanet, myPlanet, yourPlanet;

            Debug.Assert( gs.Planets.Count == 3 );

            Debug.Assert( gs.Planets.Neutral.Count( ) == 1 );
            neutralPlanet = gs.Planets.Neutral.Single( );
            Debug.Assert( neutralPlanet.ShipCount == 40 );
            Debug.Assert( neutralPlanet.GrowthRate == 4 );
            Debug.Assert( neutralPlanet.X == 10 );
            Debug.Assert( neutralPlanet.Y == 10 );

            Debug.Assert( gs.Planets.Mine.Count( ) == 1 );
            myPlanet = gs.Planets.Mine.Single( );
            Debug.Assert( myPlanet.ShipCount == 100 );
            Debug.Assert( myPlanet.GrowthRate == 5 );
            Debug.Assert( myPlanet.X == 2 );
            Debug.Assert( myPlanet.Y == 3 );

            Debug.Assert( gs.Planets.Yours.Count( ) == 1 );
            yourPlanet = gs.Planets.Yours.Single( );
            Debug.Assert( yourPlanet.ShipCount == 100 );
            Debug.Assert( yourPlanet.GrowthRate == 5 );
            Debug.Assert( yourPlanet.X == 17 );
            Debug.Assert( yourPlanet.Y == 18 );

            Debug.Assert( gs.Fleets.Count == 0 );


            gs1 = gs.Futures[ 1 ];

            Debug.Assert( gs1.Planets.Count == 3 );

            Debug.Assert( gs1.Planets.Neutral.Count( ) == 1 );
            neutralPlanet = gs1.Planets.Neutral.Single( );
            Debug.Assert( neutralPlanet.ShipCount == 40 );
            Debug.Assert( neutralPlanet.GrowthRate == 4 );
            Debug.Assert( neutralPlanet.X == 10 );
            Debug.Assert( neutralPlanet.Y == 10 );

            Debug.Assert( gs1.Planets.Mine.Count( ) == 1 );
            myPlanet = gs1.Planets.Mine.Single( );
            Debug.Assert( myPlanet.ShipCount == 105 );
            Debug.Assert( myPlanet.GrowthRate == 5 );
            Debug.Assert( myPlanet.X == 2 );
            Debug.Assert( myPlanet.Y == 3 );

            Debug.Assert( gs1.Planets.Yours.Count( ) == 1 );
            yourPlanet = gs1.Planets.Yours.Single( );
            Debug.Assert( yourPlanet.ShipCount == 105 );
            Debug.Assert( yourPlanet.GrowthRate == 5 );
            Debug.Assert( yourPlanet.X == 17 );
            Debug.Assert( yourPlanet.Y == 18 );

            Debug.Assert( gs1.Fleets.Count == 0 );


            gs5 = gs.Futures[ 5 ];

            Debug.Assert( gs5.Planets.Count == 3 );

            Debug.Assert( gs5.Planets.Neutral.Count( ) == 1 );
            neutralPlanet = gs5.Planets.Neutral.Single( );
            Debug.Assert( neutralPlanet.ShipCount == 40 );
            Debug.Assert( neutralPlanet.GrowthRate == 4 );
            Debug.Assert( neutralPlanet.X == 10 );
            Debug.Assert( neutralPlanet.Y == 10 );

            Debug.Assert( gs5.Planets.Mine.Count( ) == 1 );
            myPlanet = gs5.Planets.Mine.Single( );
            Debug.Assert( myPlanet.ShipCount == 125 );
            Debug.Assert( myPlanet.GrowthRate == 5 );
            Debug.Assert( myPlanet.X == 2 );
            Debug.Assert( myPlanet.Y == 3 );

            Debug.Assert( gs5.Planets.Yours.Count( ) == 1 );
            yourPlanet = gs5.Planets.Yours.Single( );
            Debug.Assert( yourPlanet.ShipCount == 125 );
            Debug.Assert( yourPlanet.GrowthRate == 5 );
            Debug.Assert( yourPlanet.X == 17 );
            Debug.Assert( yourPlanet.Y == 18 );

            Debug.Assert( gs5.Fleets.Count == 0 );

            Debug.Assert(gs5.EndState == false);
            Debug.Assert(gs5.Winner == null);


            gs1_4 = gs1.Futures[ 4 ];

            Debug.Assert( gs1_4.Planets.Count == 3 );

            Debug.Assert( gs1_4.Planets.Neutral.Count( ) == 1 );
            neutralPlanet = gs1_4.Planets.Neutral.Single( );
            Debug.Assert( neutralPlanet.ShipCount == 40 );
            Debug.Assert( neutralPlanet.GrowthRate == 4 );
            Debug.Assert( neutralPlanet.X == 10 );
            Debug.Assert( neutralPlanet.Y == 10 );

            Debug.Assert( gs1_4.Planets.Mine.Count( ) == 1 );
            myPlanet = gs1_4.Planets.Mine.Single( );
            Debug.Assert( myPlanet.ShipCount == 125 );
            Debug.Assert( myPlanet.GrowthRate == 5 );
            Debug.Assert( myPlanet.X == 2 );
            Debug.Assert( myPlanet.Y == 3 );

            Debug.Assert( gs1_4.Planets.Yours.Count( ) == 1 );
            yourPlanet = gs1_4.Planets.Yours.Single( );
            Debug.Assert( yourPlanet.ShipCount == 125 );
            Debug.Assert( yourPlanet.GrowthRate == 5 );
            Debug.Assert( yourPlanet.X == 17 );
            Debug.Assert( yourPlanet.Y == 18 );

            Debug.Assert( gs1_4.Fleets.Count == 0 );

            Debug.Assert( gs1_4.EndState == false );
            Debug.Assert( gs1_4.Winner == null );

            return true;
        }

        /* The fleet is not sufficient to overpower the forces on the planet.
         * This test also does more general stuff like composition of futures and proper movement. */
        static private bool TestOneFleetInsufficient()
        {
            GameState gs = new GameState(TestStrings.GameStateOneFleetInsufficient, 0 ),
                      gs1,
                      gs22,
                      gs1_4;

            Planet neutralPlanet, myPlanet, yourPlanet;
            Fleet myFleet;

            Debug.Assert(gs.Planets.Count == 3);

            Debug.Assert(gs.Planets.Neutral.Count() == 1);
            neutralPlanet = gs.Planets.Neutral.Single();
            Debug.Assert(neutralPlanet.ShipCount == 40);
            Debug.Assert(neutralPlanet.GrowthRate == 4);
            Debug.Assert(neutralPlanet.X == 10);
            Debug.Assert(neutralPlanet.Y == 10);

            Debug.Assert(gs.Planets.Mine.Count() == 1);
            myPlanet = gs.Planets.Mine.Single();
            Debug.Assert(myPlanet.ShipCount == 100);
            Debug.Assert(myPlanet.GrowthRate == 5);
            Debug.Assert(myPlanet.X == 2);
            Debug.Assert(myPlanet.Y == 3);

            Debug.Assert(gs.Planets.Yours.Count() == 1);
            yourPlanet = gs.Planets.Yours.Single();
            Debug.Assert(yourPlanet.ShipCount == 100);
            Debug.Assert(yourPlanet.GrowthRate == 5);
            Debug.Assert(yourPlanet.X == 17);
            Debug.Assert(yourPlanet.Y == 18);

            Debug.Assert(gs.Fleets.Count == 1);
            myFleet = gs.Fleets.Mine.Single( );
            Debug.Assert( myFleet.Owner == Players.Singleton.Me );
            Debug.Assert( myFleet.ShipCount == 50 );
            Debug.Assert( myFleet.SourcePlanet == myPlanet );
            Debug.Assert( myFleet.DestinationPlanet == yourPlanet );
            Debug.Assert( myFleet.TotalTripLength == 22 );
            Debug.Assert( myFleet.TurnsRemaining == 22 );



            gs1 = gs.Futures[ 1 ];

            Debug.Assert(gs1.Planets.Count == 3);

            Debug.Assert(gs1.Planets.Neutral.Count() == 1);
            neutralPlanet = gs1.Planets.Neutral.Single();
            Debug.Assert(neutralPlanet.ShipCount == 40);
            Debug.Assert(neutralPlanet.GrowthRate == 4);
            Debug.Assert(neutralPlanet.X == 10);
            Debug.Assert(neutralPlanet.Y == 10);

            Debug.Assert(gs1.Planets.Mine.Count() == 1);
            myPlanet = gs1.Planets.Mine.Single();
            Debug.Assert(myPlanet.ShipCount == 105);
            Debug.Assert(myPlanet.GrowthRate == 5);
            Debug.Assert(myPlanet.X == 2);
            Debug.Assert(myPlanet.Y == 3);

            Debug.Assert(gs1.Planets.Yours.Count() == 1);
            yourPlanet = gs1.Planets.Yours.Single();
            Debug.Assert(yourPlanet.ShipCount == 105);
            Debug.Assert(yourPlanet.GrowthRate == 5);
            Debug.Assert(yourPlanet.X == 17);
            Debug.Assert(yourPlanet.Y == 18);

            Debug.Assert(gs1.Fleets.Count == 1);
            myFleet = gs1.Fleets.Mine.Single();
            Debug.Assert(myFleet.Owner == Players.Singleton.Me);
            Debug.Assert(myFleet.ShipCount == 50);
            Debug.Assert(myFleet.SourcePlanet == myPlanet);
            Debug.Assert(myFleet.DestinationPlanet == yourPlanet);
            Debug.Assert(myFleet.TotalTripLength == 22);
            Debug.Assert(myFleet.TurnsRemaining == 21);


            gs22 = gs.Futures[ 22 ];

            Debug.Assert(gs22.Planets.Count == 3);

            Debug.Assert(gs22.Planets.Neutral.Count() == 1);
            neutralPlanet = gs22.Planets.Neutral.Single();
            Debug.Assert(neutralPlanet.ShipCount == 40);
            Debug.Assert(neutralPlanet.GrowthRate == 4);
            Debug.Assert(neutralPlanet.X == 10);
            Debug.Assert(neutralPlanet.Y == 10);

            Debug.Assert(gs22.Planets.Mine.Count() == 1);
            myPlanet = gs22.Planets.Mine.Single();
            Debug.Assert(myPlanet.ShipCount == 210);
            Debug.Assert(myPlanet.GrowthRate == 5);
            Debug.Assert(myPlanet.X == 2);
            Debug.Assert(myPlanet.Y == 3);

            Debug.Assert(gs22.Planets.Yours.Count() == 1);
            yourPlanet = gs22.Planets.Yours.Single();
            Debug.Assert(yourPlanet.ShipCount == 160);
            Debug.Assert(yourPlanet.GrowthRate == 5);
            Debug.Assert(yourPlanet.X == 17);
            Debug.Assert(yourPlanet.Y == 18);

            Debug.Assert(gs22.Fleets.Count == 0);

            Debug.Assert(gs22.EndState == false);
            Debug.Assert(gs22.Winner == null);


            gs1_4 = gs1.Futures[ 4 ];

            Debug.Assert(gs1_4.Planets.Count == 3);

            Debug.Assert(gs1_4.Planets.Neutral.Count() == 1);
            neutralPlanet = gs1_4.Planets.Neutral.Single();
            Debug.Assert(neutralPlanet.ShipCount == 40);
            Debug.Assert(neutralPlanet.GrowthRate == 4);
            Debug.Assert(neutralPlanet.X == 10);
            Debug.Assert(neutralPlanet.Y == 10);

            Debug.Assert(gs1_4.Planets.Mine.Count() == 1);
            myPlanet = gs1_4.Planets.Mine.Single();
            Debug.Assert(myPlanet.ShipCount == 125);
            Debug.Assert(myPlanet.GrowthRate == 5);
            Debug.Assert(myPlanet.X == 2);
            Debug.Assert(myPlanet.Y == 3);

            Debug.Assert(gs1_4.Planets.Yours.Count() == 1);
            yourPlanet = gs1_4.Planets.Yours.Single();
            Debug.Assert(yourPlanet.ShipCount == 125);
            Debug.Assert(yourPlanet.GrowthRate == 5);
            Debug.Assert(yourPlanet.X == 17);
            Debug.Assert(yourPlanet.Y == 18);

            Debug.Assert(gs1_4.Fleets.Count == 1);
            myFleet = gs1_4.Fleets.Mine.Single();
            Debug.Assert(myFleet.Owner == Players.Singleton.Me);
            Debug.Assert(myFleet.ShipCount == 50);
            Debug.Assert(myFleet.SourcePlanet == myPlanet);
            Debug.Assert(myFleet.DestinationPlanet == yourPlanet);
            Debug.Assert(myFleet.TotalTripLength == 22);
            Debug.Assert(myFleet.TurnsRemaining == 17);


            return true;
        }

        /* The fleet exactly matches the forces on the planet. The planet's ship count hits 0, but the owner doesn't change */
        static private bool TestOneFleetNeutralise()
        {
            GameState gs = new GameState(TestStrings.GameStateOneFleetNeutralise, 0 ),
                      gs22,
                      gs23;

            Planet planet0, planet1, planet2;
            Fleet myFleet;

            Debug.Assert(gs.Planets.Count == 3);

            planet2 = gs.Planets.Single( x => x.Id == 2 );
            Debug.Assert( planet2.Owner == Players.Singleton.Neutral );
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs.Planets.Single( x => x.Id == 0 );
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 100);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.You);
            Debug.Assert(planet1.ShipCount == 100);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs.Fleets.Count == 1);
            myFleet = gs.Fleets.Mine.Single();
            Debug.Assert(myFleet.Owner == Players.Singleton.Me);
            Debug.Assert(myFleet.ShipCount == 210);
            Debug.Assert(myFleet.SourcePlanet == planet0);
            Debug.Assert(myFleet.DestinationPlanet == planet1);
            Debug.Assert(myFleet.TotalTripLength == 22);
            Debug.Assert(myFleet.TurnsRemaining == 22);


            gs22 = gs.Futures[ 22 ];

            Debug.Assert(gs22.Planets.Count == 3);

            planet2 = gs22.Planets.Single(x => x.Id == 2);
            Debug.Assert(planet2.Owner == Players.Singleton.Neutral);
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs22.Planets.Single(x => x.Id == 0);
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 210);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs22.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.You);
            Debug.Assert(planet1.ShipCount == 0);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs22.Fleets.Count == 0);


            gs23 = gs.Futures[ 23 ];

            Debug.Assert(gs23.Planets.Count == 3);

            planet2 = gs23.Planets.Single(x => x.Id == 2);
            Debug.Assert(planet2.Owner == Players.Singleton.Neutral);
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs23.Planets.Single(x => x.Id == 0);
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 215);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs23.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.You);
            Debug.Assert(planet1.ShipCount == 5);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs23.Fleets.Count == 0);

            Debug.Assert(gs23.EndState == false);
            Debug.Assert(gs23.Winner == null);

            return true;
        }

        /* The fleet overpowers the forces on the planet, thus the planet's owner changes. */
        static private bool TestOneFleetOverpower()
        {
            GameState gs = new GameState(TestStrings.GameStateOneFleetOverpower, 0 ),
                      gs22,
                      gs23;

            Planet planet0, planet1, planet2;
            Fleet myFleet;

            Debug.Assert(gs.Planets.Count == 3);

            planet2 = gs.Planets.Single(x => x.Id == 2);
            Debug.Assert(planet2.Owner == Players.Singleton.Neutral);
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs.Planets.Single(x => x.Id == 0);
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 100);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.You);
            Debug.Assert(planet1.ShipCount == 100);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs.Fleets.Count == 1);
            myFleet = gs.Fleets.Mine.Single();
            Debug.Assert(myFleet.Owner == Players.Singleton.Me);
            Debug.Assert(myFleet.ShipCount == 300);
            Debug.Assert(myFleet.SourcePlanet == planet0);
            Debug.Assert(myFleet.DestinationPlanet == planet1);
            Debug.Assert(myFleet.TotalTripLength == 22);
            Debug.Assert(myFleet.TurnsRemaining == 22);


            gs22 = gs.Futures[ 22 ];

            Debug.Assert(gs22.Planets.Count == 3);

            planet2 = gs22.Planets.Single(x => x.Id == 2);
            Debug.Assert(planet2.Owner == Players.Singleton.Neutral);
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs22.Planets.Single(x => x.Id == 0);
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 210);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs22.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.Me);
            Debug.Assert(planet1.ShipCount == 90);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs22.Fleets.Count == 0);


            gs23 = gs.Futures[ 23 ];

            Debug.Assert(gs23.Planets.Count == 3);

            planet2 = gs23.Planets.Single(x => x.Id == 2);
            Debug.Assert(planet2.Owner == Players.Singleton.Neutral);
            Debug.Assert(planet2.ShipCount == 40);
            Debug.Assert(planet2.GrowthRate == 4);
            Debug.Assert(planet2.X == 10);
            Debug.Assert(planet2.Y == 10);

            planet0 = gs23.Planets.Single(x => x.Id == 0);
            Debug.Assert(planet0.Owner == Players.Singleton.Me);
            Debug.Assert(planet0.ShipCount == 215);
            Debug.Assert(planet0.GrowthRate == 5);
            Debug.Assert(planet0.X == 2);
            Debug.Assert(planet0.Y == 3);

            planet1 = gs23.Planets.Single(x => x.Id == 1);
            Debug.Assert(planet1.Owner == Players.Singleton.Me);
            Debug.Assert(planet1.ShipCount == 95);
            Debug.Assert(planet1.GrowthRate == 5);
            Debug.Assert(planet1.X == 17);
            Debug.Assert(planet1.Y == 18);

            Debug.Assert(gs23.Fleets.Count == 0);

            Debug.Assert( gs23.EndState == true );
            Debug.Assert( gs23.Winner == Players.Singleton.Me );

            return true;
        }

        static private bool EndStates( )
        {
            GameState gs = new GameState( TestStrings.GameStateEndState1, 0 );

            Debug.Assert( gs.EndState == false );
            Debug.Assert( gs.Winner == null );

            return true;
        }
    }
}