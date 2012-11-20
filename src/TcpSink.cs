using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace mtBot
{
    class TcpSink : ISourceSink
    {
        private Socket m_Socket;

        public TcpSink( string host, ushort port )
        {
            /* Server (connectee) */
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint listenIEP = new IPEndPoint(IPAddress.Any, 1337);
            sock.Bind(listenIEP);
            sock.Listen( 10 );
            m_Socket = sock.Accept( );
        }

        public int Source( )
        {
            int ret = -1;
            byte[] buf = new byte[1];

            try
            {
                int bytesRead = m_Socket.Receive(buf);
                if (bytesRead > 0)
                {
                    ret = buf[0];    
                }
            }
            catch(Exception)
            {
                
            }

            return ret;
        }

        public void Sink( string message )
        {
            m_Socket.Send( Encoding.UTF8.GetBytes( message + "\n" ) );
        }
    }
}
