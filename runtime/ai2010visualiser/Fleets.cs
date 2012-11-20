using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    public class Fleets : List<Fleet>
    {
        public IEnumerable<Fleet> MyFleets
        {
            get { return this.Where(x => x.Owner == Players.Me); }
        }

        public IEnumerable<Fleet> YourFleets
        {
            get { return this.Where(x => x.Owner == Players.You); }
        }
    }
}
