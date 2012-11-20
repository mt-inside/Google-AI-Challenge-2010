using System.Collections.Generic;

namespace mtBot
{
    class Orders : List<Order>
    {
        private ISourceSink m_Sink;

        public Orders( )
        {
            m_Sink = ObjectFactory.Get<ISourceSink>();
        }

        public void Done( )
        {
            foreach (Order order in this)
            {
                m_Sink.Sink( order.ToString(  ) );
            }
            
            m_Sink.Sink( "go" );
        }
    }
}
