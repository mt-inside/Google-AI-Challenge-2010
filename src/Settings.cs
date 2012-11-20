namespace mtBot
{
    class Settings
    {
        private bool m_TcpIo = false;

        public bool TcpIo
        {
            get { return m_TcpIo; }
            set { m_TcpIo = value; }
        }
    }
}
