using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace mtBot
{
    class GameState
    {
        private readonly int m_Turn;
        private readonly Planets m_Planets = new Planets();
        private readonly Fleets m_Fleets = new Fleets();
        private readonly Orders m_Orders = new Orders();

        private readonly FuturesMemoiser m_FuturesMemoiser;

        public GameState( string gameStatestring, int turn )
            : this( turn )
        {
            ParseGameState(gameStatestring);
        }

        /* Private ctor to make an empty gs into which to programatically build e.g. the next turn. */
        private GameState( int turn )
        {
            m_Turn = turn;
            m_FuturesMemoiser = new FuturesMemoiser(FutureInternal);
        }

        public Orders Orders
        {
            get { return m_Orders; }
        }

        public Planets Planets
        {
            get { return m_Planets; }
        }
        public Fleets Fleets
        {
            get { return m_Fleets; }
        }

        /* Is the game in an end state (win for either player or draw)? */
        public bool EndState
        {
            get { return GetEndStateInner( ) >= 0; }
        }

        /* Returns the winning player, or null if the game's not over */
        public Player Winner
        {
            get
            {
                int endState = GetEndStateInner( );

                if (endState == -1) return null;
                else return Players.Singleton[ endState ];
            }
        }

        private int GetEndStateInner()
        {
            List<Player> remainingPlayers = new List<Player>();

            remainingPlayers.AddRange( m_Planets.Where( p => p.Owner != Players.Singleton.Neutral && !remainingPlayers.Contains( p.Owner ) ).Select( p => p.Owner ) );
            remainingPlayers.AddRange( m_Fleets.Where( f => !remainingPlayers.Contains( f.Owner ) ).Select( f => f.Owner ) );

            switch (remainingPlayers.Count)
            {
                case 0:
                    return 0;
                case 1:
                    return remainingPlayers[0].Id;
                default:
                    return -1;
            }
        }

        private void ParseGameState(string s)
        {
            m_Planets.Clear();
            m_Fleets.Clear();
            int planetID = 0;
            string[] lines = s.Split('\n');
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];
                int commentBegin = line.IndexOf('#');
                if (commentBegin >= 0)
                {
                    line = line.Substring(0, commentBegin);
                }
                if (line.Trim().Length == 0)
                {
                    continue;
                }

                string[] tokens = line.Split(' ');
                if (tokens.Length == 0)
                {
                    continue;
                }

                switch (tokens[0])
                {
                    case "P":
                    {
                        /* P <x:float> <y:float> <owner:int> <ships:int> <growth:int> */
                        if (tokens.Length != 6)
                        {
                            throw new Exception("Invalid planet clause");
                        }
                        double x = double.Parse(tokens[1]);
                        double y = double.Parse(tokens[2]);
                        int ownerId = int.Parse(tokens[3]);
                        int numShips = int.Parse(tokens[4]);
                        int growthRate = int.Parse(tokens[5]);
                        Planet p = new Planet(
                            planetID++,
                            Players.Singleton[ ownerId ],
                            numShips,
                            growthRate,
                            x, y,
                            this
                            );
                        m_Planets.Add(p);
                        break;
                    }

                    case "F":
                    {
                        /* F <owner:int> <ships:int> <source:int> <destination:int> <total_turns:int> <remaining_turns:int> */
                        if (tokens.Length != 7)
                        {
                            throw new Exception("Invalid fleet clause");
                        }
                        int ownerId = int.Parse(tokens[1]);
                        int numShips = int.Parse(tokens[2]);
                        int source = int.Parse(tokens[3]);
                        int destination = int.Parse(tokens[4]);
                        int totalTripLength = int.Parse(tokens[5]);
                        int turnsRemaining = int.Parse(tokens[6]);
                        Fleet f = new Fleet(
                            Players.Singleton[ ownerId ],
                            numShips,
                            m_Planets[ source ],
                            m_Planets[ destination ],
                            totalTripLength,
                            turnsRemaining,
                            this
                            );
                        m_Fleets.Add(f);
                        break;
                    }

                    default:
                        throw new Exception(String.Format("Unrecognised clause {0}", tokens[0]));
                }
            }
        }

        public FuturesMemoiser Futures
        {
            get { return m_FuturesMemoiser; }
        }

        private GameState FutureInternal( int turns )
        {
            GameState gs = this;

            Debug.Assert( turns >= 0 );

            if (turns == 0) return gs; /* Be sure to return the same object */

            /* If these next two lines were the other way round, i.e. tail-recursive, the memoisation would work couter-intuitively;
             * what you'd get is a string of GameStates each with Future( 1 ) memoised, whereas this gives us one GameState with Future( 1..n ) memoised.
             */
            if (turns > 1)
            {
                gs = gs.Futures[ turns - 1 ];
            }

            return gs.Turn();
        }

        private GameState Turn( )
        {
            GameState newState = new GameState( m_Turn + 1 );

            /* Turn existing objects. Fleets may disappear if they arrive. */
            newState.Planets.AddRange(Planets.Select(p => p.Turn( newState )));
            newState.Fleets.AddRange( Fleets.Select( f => f.Turn( newState )).Where(f => f != null));

            /* Add predicted new fleets - TODO: any idea if this works?? */
            Fleets.Yours.Where( f => f.TurnsTravelled == 1 ).ToList(  ).ForEach( fleet =>
                {
                    if( Fleets.Where( f => f.SourcePlanet == fleet.SourcePlanet && f.ShipCount == fleet.ShipCount && f.TurnsTravelled == 2 ).Any( ) &&
                        Fleets.Where( f => f.SourcePlanet == fleet.SourcePlanet && f.ShipCount == fleet.ShipCount && f.TurnsTravelled == 3 ).Any( ) )
                    {
                        /* looks like a stream is in progress, predict that it will continue. */
                        newState.Fleets.Add(
                            new Fleet(
                                fleet.Owner, fleet.ShipCount, fleet.SourcePlanet, fleet.DestinationPlanet, fleet.TotalTripLength, fleet.TotalTripLength - 1, newState
                            )
                        );
                    }
                }
            );

            /* TODO: add in out predicted fleet movements here? (only turn "0" will have anything in Orders) - maybe rendering these in should happen in a different method, after all, we will want to do it several times. It also means that gs is mutable, which totally hoses the memoiser. */

            return newState;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            m_Planets.Select( p => sb.Append( p ) );
            m_Fleets.Select(  f => sb.Append( f ) );

            return sb.ToString();
        }
    }

    class FuturesMemoiser : Memoiser<int, GameState>, IEnumerable<GameState>
    {
        public FuturesMemoiser( Func<int, GameState> innerFunction )
            : base( innerFunction )
        {
        }

        public IEnumerator<GameState> GetEnumerator()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }
    }
}