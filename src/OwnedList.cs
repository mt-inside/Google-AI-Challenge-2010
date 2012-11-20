using System.Collections.Generic;
using System.Linq;

namespace mtBot
{
    abstract class OwnedList<T> : List<T>
        where T : IOwned
    {
        public IEnumerable<T> Neutral
        {
            get { return this.Where( x => x.Owner == Players.Singleton.Neutral ); }
        }

        public IEnumerable<T> Mine
        {
            get { return this.Where( x => x.Owner == Players.Singleton.Me ); }
        }

        public IEnumerable<T> NotMine
        {
            get { return this.Where( x => x.Owner != Players.Singleton.Me ); }
        }

        public IEnumerable<T> Yours
        {
            get { return this.Where( x => x.Owner == Players.Singleton.You ); }
        }
    }
}
