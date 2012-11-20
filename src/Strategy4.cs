using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace mtBot
{
    static class Strategy4
    {
        static private double Weight(Planet p)
        {
            return
                ((double)p.GrowthRate / (double)p.ShipCount) *
                ((p.Owner == Players.Singleton.You) ? 2 : 1);
        }

        /* Dispatch to single planet (no futuring):
         * Bully: 95
         * Dual: 86
         * Prospector: 93
         * Rage: 96
         * Random: 100
         * 
         * Dispatch to multiple planets (no futuring):
         * Bully: 100
         * Dual: 98
         * Prospector: 100
         * Rage: 70
         * Random: 100
         */
        static public void Turn(GameState gs)
        {
            /* Current strongest planet - this should really be futured for the time it will take the ships to get there, but we don't yet know where they're coming from.
             * So for now, we work on most desirable now (may not be desirable in future if a massive enemy fleet is en route).
             * This is different to the problem of the desired planet's shipcount increasing, which we can know once we've decided which one it is (below). */
            /* OH HAI. Desirability is NOT based on current ship count, but how many ships we will have to throw to take it.
             * The planet with the lowest ship count might take a load of ships to take, if it's far away and so a lot have to be thrown across the map, to account for its growth in the meantime.
             * TODO: we need to run this algorithm (futured closet-first ship throwing) for all planets, then at the end the most desirable is the most growth we get for the fewest ships we'd actually need to send */
            /* TODO: need to make sure that we still get the "auto defense" from us targeting our own planets with just enough ships when they're unable to defend themselves. Not sure how to do this yet */
            IOrderedEnumerable<Planet> desirablePlanets =
                gs.Planets.NotMine.
                Where(p => !gs.Fleets.Where(f => f.DestinationPlanet == p).Any()).
                OrderByDescending(p => Weight(p));

            foreach (Planet desired in desirablePlanets)
            {
                /* GameState turning *must* render our orders into the mix too */
                int shipsSent = 0;
                IList<Order> potentialOrders = new List<Order>();

                IOrderedEnumerable<Planet> closetPlanets =
                    gs.Planets.Mine.OrderBy( p => p.DistanceTo( desired ) );

                foreach (Planet closePlanet in closetPlanets)
                {
                    Planet futureDesired = gs.Futures[closePlanet.DistanceTo( desired )].Planets[desired.Id];

                    if (futureDesired.Owner == Players.Singleton.Me) break;

                    /* must send one more than the planet's shipcount, otherwise it doesn't change hands. */
                    int shipsNeeded = futureDesired.ShipCount + 1;

                    int shipsToSend = Math.Min(
                        shipsNeeded - shipsSent,
                        closePlanet.FuturedAvailableShips - gs.Orders.Where(o => o.SourcePlanet == closePlanet).Select(o => o.ShipCount).Sum() //TODO: nice method in gs to account for this?
                    );

                    /* FuturedAvailableShips will return 0 if we're under attack and will need every last ship to defend. */
                    if (shipsToSend > 0)
                    {
                        potentialOrders.Add(new Order(closePlanet, desired, shipsToSend));
                        shipsSent += shipsToSend;
                        Debug.Assert(shipsToSend <= shipsNeeded);
                    }

                    if (shipsSent == shipsNeeded)
                    {
                        /* Only issue order if we mustered enough ships to take the planet. */
                        gs.Orders.AddRange(potentialOrders);
                        break;
                    }
                }
            }

            gs.Orders.Done();
        }
    }
}