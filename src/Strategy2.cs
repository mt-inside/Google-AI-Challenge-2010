using System.Diagnostics;
using System.Linq;

namespace mtBot
{
    /* Strategy 2: Maximize newly acquired build rate at all times.
     * - This can't be optimal as we don't know for how long we'll hold a planet, so we can't properly factor in the lost production time in travelling there.
     * - We also have to account for planets being of different cost to take
     * - Instead, we take best heuristic guess.
     * 
     * CAVEATS:
     * - Because this strat only issues orders from 1 planet at a time, the set it can attack after going after a couple is severly limited.
     *   - This is why it looks like the closest / strongest picking isn't working, and why it always sends a few small fleets across the map.
     * 
     * TODO:
     * - Needs a learning over-commit of ships for invasions.
     *   - Start with 0 overcommit and add everytime we loose? In fact, adjust according to how much we won/lost by. Need PID :)
     *   - This is to counter bots that send a constant stream of ships all the time.
     *   - This will only tell us the average size of a stream though - we should be able to recognise is as 3+ contiguous fleets in transit, and predict it forwards.
     *     - Have a generator function that's run on gamestate turn that predicts their movements, e.g. will insert more of a stream if there's currently one in progress.
     * - maybe move ships around between planets that we have no work for so that fleet size is always proportional to growth rate.
     *   - this has the downside of having a lot of ships on the move, not defending (maybe trickle feed them, PID or something :)
     *   - solving this troop re-allocation problem to turn an arbitrary GameState into equally defended planets is some kind of NP-complete monster problem. Must be equivalent to something though.
     * - instead of the last one, maybe just bring all non-strongest planets into defending against enemy fleets.
     *   - I.e. find the miniumum proportion to take equally from all planets in range such that the attack is just held off, or so that local fleet levels are equal.
     */
    /* DistanceOnlyWeight: Dual 97, Rage 66
     * Poly:
     *   0, 0, 2: 97 / 68
     *   0, 1, 0: 99 / 73
     *   0, 2, 0: 99 / 73
     *   0, 3, 0: 99 / 73
     *   2, 0, 0: 98 / 70
     */
    static class Strategy2
    {
        static private double PolynomialDistanceWeight(Planet p, Planet reference, Polynomial poly)
        {
            return
                ( (double)p.GrowthRate /
                  ( (double)p.ShipCount * /* Addition vs multiplication seems to make very little difference here */
                    (poly * p.DistanceTo(reference))
                  )
                ) *
                ( (p.Owner == Players.Singleton.You) ? 2 : 1 );
        }

        static private double DistanceOnlyWeight(Planet p, Planet reference)
        {
            return 1 / (double)p.DistanceTo(reference);
        }

        static public void Turn( GameState gs )
        {
            /* Current strongest planet */
            IOrderedEnumerable<Planet> planetsByStrongest = gs.Planets.Mine.OrderByDescending( p => p.FuturedAvailableShips );
            Planet strongest = planetsByStrongest.Count() > 0 ? planetsByStrongest.First() : null;

            if (strongest != null)
            {
                int shipsRemain = strongest.FuturedAvailableShips;
                Debug.Assert( strongest.FuturedAvailableShips <= strongest.ShipCount );

                /* All planets that won't be ours when any fleet send from here would reach them */
                IOrderedEnumerable<Planet> weightedPlanets =
                    gs.Planets.FuturedPredicate( gs, strongest, x => x.Owner != Players.Singleton.Me ).
                    OrderByDescending( p => PolynomialDistanceWeight( p, strongest, new Polynomial( 0, 2, 0 ) ) );

                /* Go through what all the planets' ratings are predicted to be when a fleet would reach them */
                foreach (Planet futurePlanet in weightedPlanets)
                {
                    /* must send one more than the planet's shipcount, otherwise it doesn't change hands. */
                    if (futurePlanet.ShipCount < shipsRemain)
                    {
                        gs.Orders.Add(new Order(strongest, futurePlanet, futurePlanet.ShipCount + 1));
                        shipsRemain -= futurePlanet.ShipCount + 1;
                    }
                }
            }

            gs.Orders.Done();
        }
    }
}