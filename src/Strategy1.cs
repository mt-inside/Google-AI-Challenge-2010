using System;
using System.Linq;

namespace mtBot
{
    /* Default strategy */
    static class Strategy1
    {
        /* Turn entry point */
        public static void Turn(GameState gs)
        {
            // (1) If we currently have a fleet in flight, just do nothing.
            if (gs.Fleets.Mine.Any())
            {
                return;
            }

            // (2) Find my strongest planet.
            Planet source = null;
            double sourceScore = Double.MinValue;
            foreach (Planet p in gs.Planets.Mine)
            {
                double score = (double)p.ShipCount;
                if (score > sourceScore)
                {
                    sourceScore = score;
                    source = p;
                }
            }

            // (3) Find the weakest enemy or neutral planet.
            Planet dest = null;
            double destScore = Double.MinValue;
            foreach (Planet p in gs.Planets.Neutral.Concat(gs.Planets.Yours))
            {
                double score = 1.0 / (1 + p.ShipCount);
                if (score > destScore)
                {
                    destScore = score;
                    dest = p;
                }
            }

            // (4) Send half the ships from my strongest planet to the weakest
            // planet that I do not own.
            if (source != null && dest != null)
            {
                int numShips = source.ShipCount / 2;

                gs.Orders.Add(new Order(source, dest, numShips));
            }

            gs.Orders.Done();
        }
    }
}
