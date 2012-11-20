using System;
using System.Collections.Generic;

namespace mtBot
{
    static class ObjectFactory
    {
        private static readonly IDictionary<Type, object> s_Singletons = new Dictionary<Type, object>();

        public static T Get<T>( )
        {
            return (T) Get( typeof( T ) );
        }

        static private object Get( Type t )
        {
            object ret = null;

            if( t == typeof( ISourceSink ) )
            {
                return SingletonBehaviour(
                    t, 
                    delegate
                    {
                        if( ObjectFactory.Get<Settings>( ).TcpIo )
                        {
                            return new TcpSink( "localhost", 1337 );
                        }
                        else
                        {
                            return new ConsoleSink( );
                        }
                    }
                );
            }
            else if (t == typeof( Settings )
                  || t == typeof( PlanetGeometryCache ) )
            {
                return SingletonBehaviour( t );
            }

            return ret;
        }

        static private object SingletonBehaviour( Type t )
        {
            return SingletonBehaviour( t, (Func<object>)(() => t.GetConstructor( new Type[0] ).Invoke( new object[0] )) );
        }
        static private object SingletonBehaviour( Type t, Func<object> factory )
        {
            if (!s_Singletons.ContainsKey(t))
            {
                s_Singletons[t] = factory();
            }

            return s_Singletons[t];            
        }
    }
}
