using System;
using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    class Planets : OwnedList<Planet>
    {
        /* For a view of what planets will look like after n turns, simply turn your GameState */

        /* Returns a collection of all planets as they will look in n turns where n is the distance from planet.
         * That is, what a planet will be like when reached by a fleet dispatched this turn from planet */
        /* TODO: put these function on ITurnable, and indeed impliement it in generic terms on a base class / extension */
        public IEnumerable<Planet> FuturePredictionTravelTime( GameState startingState, Planet planet )
        {
            int planetsLeft = this.Count,
                distance = 0; /* Both in time and space... oooh */
            GameState gs = startingState;
            IEnumerable<Planet> ps;
            Stack<Planet> ret = new Stack<Planet>(planetsLeft);

            while (planetsLeft > 0)
            {
                /* Advance */
                ps = gs.Planets.Where(x => x.DistanceTo(planet) == distance);  /* Being eagerly executed, not a problem */
                planetsLeft -= ps.Count();
                
                foreach (Planet planet1 in ps)
                {
                    ret.Push(planet1);
                }

                distance++;
                gs = startingState.Futures[ distance ];
            }

            return ret;
        }

        /* Return a collection of the planets that will satisfy pred in n turns where n is the distance from planet.
         * That is, all the planets that are expected still to satisfy pred when reached by a fleet dispatched this turn from planet. */
        public IEnumerable<Planet> FuturedPredicate( GameState gs, Planet planet, Func<Planet, bool> pred )
        {
            return FuturePredictionTravelTime( gs, planet ).Where( pred );
        }
    }

    static class PlanetsExtensions
    {

    }
}
