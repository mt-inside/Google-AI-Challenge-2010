using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mtBot
{
    static class Strategy3
    {
        /* Strategy 3: Strategy 2 with multi-planet dispatch.
         * - STAGE 1: Dispatch from our strongest planets, in order, accounting for own actions to avoid treading on own toes.
         * - If there were any of our planets left that haven't attacked, it's because they weren't strong enough.
         * - STAGE 2: Take their most desirable planets, in order, and dispatch from as many of our remaining planets as needed to take them.
         * 
         * STRAT 3: ** need a way of accounting for this turn's orders, both in terms of our planets' available ships, and their planets' futures
         * Stage 2:
         * - take their strongest or strongest n (desc) and throw as much at them as needed from multiple planets (need to account for different fleet travel times).
         */
        /* STAGE 1 ONLY
         * DistanceOnlyWeight: Dual ?, Rage ?
         * Poly:
         *   0, 0, 2: 98 / 71
         *   0, 1, 0: 99 / 80
         *   0, 2, 0: 99 / 80
         *   0, 5, 0: 99 / 80
         *   0, 50, 0: 99 / 80
         *   0, 500, 0: 99 / 80
         *   2, 0, 0: 99 / 73
         *   
         * STAGE 1 & 2
         * Poly 0, 2, 0: 99 / 78 (?? presumably fewer planets able to defend themselves against rage - same happens in strat 4)
         */
        static private double PolynomialDistanceWeight(Planet p, Planet reference, Polynomial poly)
        {
            return
                ((double)p.GrowthRate /
                  ((double)p.ShipCount * /* Addition vs multiplication seems to make very little difference here */
                    (poly * p.DistanceTo(reference))
                  )
                ) *
                ((p.Owner == Players.Singleton.You) ? 2 : 1);
        }

        static private double NoDistanceWeight(Planet p)
        {
            return
                ((double)p.GrowthRate / (double)p.ShipCount) *
                ((p.Owner == Players.Singleton.You) ? 2 : 1);
        }

        static public void Turn( GameState gs )
        {
            IOrderedEnumerable<Planet> desirablePlanets;

            /* STAGE 1 */

            /* Current strongest planet */
            IOrderedEnumerable<Planet> strongestPlanets = gs.Planets.Mine.OrderByDescending( p => p.FuturedAvailableShips );
            Set<Planet> attackedPlanets = new Set<Planet>();

            foreach( Planet strongestPlanet in strongestPlanets )
            {
                int shipsRemain = strongestPlanet.FuturedAvailableShips;
                Debug.Assert( strongestPlanet.FuturedAvailableShips <= strongestPlanet.ShipCount );

                /* All planets that won't be ours when any fleet send from here would reach them */
                desirablePlanets =
                    gs.Planets.FuturedPredicate( gs, strongestPlanet, x => x.Owner != Players.Singleton.Me ).
                    OrderByDescending( p => PolynomialDistanceWeight( p, strongestPlanet, new Polynomial( 0, 2, 0 ) ) );

                /* Go through what all the planets' ratings are predicted to be when a fleet would reach them */
                foreach (Planet futurePlanet in desirablePlanets)
                {
                    /* Don't attack any planets that we're currently attacking, or will attack this turn.
                     * The will-attack-this-turn seems fair enough, as we always send sufficient fleets.
                     * The currently-attacking is a bit of a hack to avoid the strongest-getting-closer problem, and should be solved by strat 4 */
                    if (attackedPlanets.Contains(futurePlanet)) continue;
                    if (gs.Fleets.Mine.Where(f => f.DestinationPlanet == futurePlanet).Any()) continue;

                    /* must send one more than the planet's shipcount, otherwise it doesn't change hands. */
                    if (futurePlanet.ShipCount < shipsRemain)
                    {
                        gs.Orders.Add(new Order(strongestPlanet, futurePlanet, futurePlanet.ShipCount + 1));
                        attackedPlanets.Add(futurePlanet);

                        shipsRemain -= futurePlanet.ShipCount + 1;
                    }
                }
            }


            /* STAGE 2 */
            desirablePlanets =
                gs.Planets.Where( p => p.Owner != Players.Singleton.Me ).
                Where( p => !gs.Orders.Where( o => o.DestinationPlanet == p ).Any() ).
                OrderByDescending( p => NoDistanceWeight( p ) );

            foreach (Planet desirablePlanet in desirablePlanets)
            {
                int shipsNeeded = desirablePlanet.ShipCount + 1; //FIXME needs futuring
                IList<Order> potentialOrders = new List<Order>();

                IOrderedEnumerable<Planet> closetPlanets =
                    gs.Planets.Mine.OrderBy(p => p.DistanceTo(desirablePlanet));

                foreach (Planet closePlanet in closetPlanets)
                {
                    int shipsToSend = Math.Min(
                        shipsNeeded,
                        closePlanet.FuturedAvailableShips - gs.Orders.Where(o => o.SourcePlanet == closePlanet).Select(o => o.ShipCount).Sum() //TODO: nice method in gs to account for this?
                    );

                    /* FuturedAvailableShips will return 0 if we're under attack and will need every last ship to defend. */
                    if (shipsToSend > 0)
                    {
                        potentialOrders.Add(new Order(closePlanet, desirablePlanet, shipsToSend));
                        shipsNeeded -= shipsToSend;
                        Debug.Assert(shipsNeeded >= 0);
                    }

                    if (shipsNeeded == 0)
                    {
                        /* Only issue order if we mustered enough ships to take the planet. */
                        gs.Orders.AddRange( potentialOrders );
                        break;
                    }
                }
            }    


            /* ATTACK! */

            gs.Orders.Done();
        }
    }
}
