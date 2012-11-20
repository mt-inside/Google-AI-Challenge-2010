using System;
using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    public class Planets : List<Planet>
    {
        public IEnumerable<Planet> NeutralPlanets
        {
            get { return this.Where(x => x.Owner == Players.Neutral); }
        }

        public IEnumerable<Planet> MyPlanets
        {
            get { return this.Where(x => x.Owner == Players.Me); }
        }

        public IEnumerable<Planet> YourPlanets
        {
            get { return this.Where(x => x.Owner == Players.You); }
        }
    }
}
