namespace mtBot
{
    class StrategyScratch
    {
        //RE: start 3
        // FIXME: this work insofar as it avoids spamming the same planet lots of times on the same go.
        // However, it *really* suffers from the strongest-moving-closer problem.
        // NEED to do strat 4, where we swap the loops - foreach of their most desirable planets, find our strongest one that can take it (futured)
        // Do a strat 4, like strat 2 - for their planet, throw our closest planet that can take it, at it, if there is one (remember to use avail ships). That planet's remaining ships then go back in the pool (need way to resolve our current orders)
        // Do a strat 5, like strat 3 - for their planet, find some combo of ours that can take it, probably closest first. This causes problem that our planets most likely to be attacked will be weak (though use avail ships), and the ones furthest from the enemy will get strong.
        // Strat 6: after we've attacked them, add a fleet equalisation / move to front line (i.e. those that have attacking orders) step from the planets that aren't involved in attack


        /*
         *     * STRAT 4:
     * order their planets like they are now, but flat in time
     * start with our closest planet to the first (strongest) target and work out, sending all ships until there are enough
     * - so long as we work outwards, we can work out how many ships will be left when fleet 1 arrives, then find planet 2, then see how many ships its fleet will have to deal with when *it* arrives.
     * Then move onto their next strongest planet.
         * 
         */


        /* Future strats:
         *  * - STRAT2's ever-changing strongest is hurting us:
     *     when a sufficient fleet is sent to a planet, the strongest ususally changes, because the old strongest just emptied most of its ships.
     *     If the new strongest is closer to last turn's optimal planet then it too will send a sufficient fleet because when that fleet arrives the planet will still be neutral...
     *     I think we need to find best target over all at the current time, no matter the strongest planet, then find the closes planet that can take it (accounting for futures).
     *     This is a very similar problem to: planet under attack, find closest planet that can help, adjusted for time.
         *     
         * 
         * consider missing first turn in order to defend against rage bot.
         * Graph clustering for defence
  * - idea of geometry - make a front line with all production feeding the front. Expand front line. Keep front line planets strong enough such that ships+future production(n) could repel n% of an attack from n distance. All other planets to empty all the time because the front line forces are always closer than are the enemy.
  *   - i'm not acutally sure about explicity coding this. Search should be able to do this. I would hope that this would happen given the right weighting for distance.
  *   - it might be interesting to try to identify the "front" (initially the homeworld, then the concave hull) and do two things:
  *     - push all ships from planets not on the front to the front
  *     - take each planet on the front in turn and run strat 2 on it as the reference planet (strongest). It would be really difficult to maintain a "front" shape without really hamering our strategy.
  *   - another take on this: the purpose of the front system is automatically to marshall ships from planets that don't need them to ones that might.
  *     - a planet doesn't need any ships if one of our planets is closer to it than one of the enemy's, because troops can be sent to it in time if it's targeted.
  *     - a planet that has an enemy as its closest inhabited neighbour needs all the ships it can get because we can't see attacks coming in time.
  *       - that is, up to the sum of the futured ships on all enemy planets closer than our nearest, adjusted for what's in flight.
  *     - treat planets closer to the enemy as "attackers", these take it in turn as the reference. They need to know not to send out so many ships that they can't defend themselves, otherwise they spam.
  *     - treat planets closer to us as "factories" and instantly send all their ships to attackers, depending on some metric of need
         *     
         * 
         *      * - Strat ?? Try a proper nearest-only advance.
     *   - Rememeber home planet - always use as reference.
     *   - Wait until have enough forces to take it (as opp to taking closest we can) - look into multi planet attack.
     *     - This is basically strat 4/5, because it's the target, not the attacker, that's the subject
     *   - By re-evaluating from home planet every time, we will maintain the blob shape.
     *   
  */

        /* Given a newly launched fleet of size fleetsize bearing down on planet target over turns turns,
         * dispatch an equal proportion of all local planets' ships to bring garrissons up to an equal ratio to growthrate */
        public static void ShitWereUnderAttackRallyTheTroops( Planet target, int fleetsize, int turns )
        {
            
        }
    }
}
