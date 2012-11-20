using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mtBot;

namespace ai2010visualiser
{
    /*
     * replay grammar:

        <planet spec>:<planet spec>:...
        |<planet state>,<planet state>,...,<fleet>,<fleet>,...
        :<planet state>,<planet state>,...,<fleet>,<fleet>,...

        <planet spec> ::= x,y,owner,ships,growth
        <planet state> ::= owner.ships
        <fleet> ::= owner.ships.source.dest.total.remain
     */
    class GameStates : List<GameState>
    {
        private static GameStates s_Singleton;
        static public GameStates Singleton
        {
            get
            {
                if( s_Singleton == null )
                {
                    s_Singleton = new GameStates( );
                }

                return s_Singleton;
            }
        }


        public void ParseReplay( string replay )
        {    
            string planetsS = replay.Split( '|' )[0],
                   turns    = replay.Split('|')[1];
            IList<Planet> basePlanets = new List<Planet>();
            string[] planetInfo, fleetInfo;
            Planet p, baseP;
            Fleet f;

            /* Planets */
            string[] planetsA = planetsS.Split(':');
            for( int i = 0; i < planetsA.Length; i++ )
            {
                planetInfo = planetsA[i].Split( ',' );

                p = new Planet(
                    i,
                    Players.GetById( int.Parse( planetInfo[2] ) ),
                    int.Parse( planetInfo[3] ),
                    int.Parse( planetInfo[4] ),
                    Double.Parse( planetInfo[0] ),
                    Double.Parse( planetInfo[1] )
                );

                basePlanets.Insert( i, p );
            }

            
            /* Turns */
            string[] turnsA = turns.Split( ':' );
            for( int i = 0; i < turnsA.Length - 1; i++ ) /* Engine seems to put an empty line on the end of the file */
            {
                GameState gs = new GameState( );
                string[] turnParts = turnsA[i].Split( ',' );

                for( int j = 0; j < turnParts.Length; j++ )
                {
                    /* New planet states */
                    if( j < basePlanets.Count )
                    {
                        planetInfo = turnParts[j].Split( '.' );
                        baseP = basePlanets[j];
                        p = new Planet(
                            j,
                            Players.GetById( int.Parse( planetInfo[0] ) ),
                            int.Parse( planetInfo[1] ),
                            baseP.GrowthRate,
                            baseP.X,
                            baseP.Y
                        );

                        gs.Planets.Add( p );
                    }

                    /* Fleets */
                    if ( j >= basePlanets.Count )
                    {
                        fleetInfo = turnParts[j].Split( '.' );
                        f = new Fleet(
                            gs,
                            Players.GetById( int.Parse( fleetInfo[0] ) ),
                            int.Parse( fleetInfo[1] ),
                            int.Parse( fleetInfo[2] ),
                            int.Parse( fleetInfo[3] ),
                            int.Parse( fleetInfo[4] ),
                            int.Parse( fleetInfo[5] )
                        );

                        gs.Fleets.Add( f );
                    }
                }
                
                this.Insert( i, gs );
            }
        }
    }
}