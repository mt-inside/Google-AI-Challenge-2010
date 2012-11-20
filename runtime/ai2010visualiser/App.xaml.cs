using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace ai2010visualiser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string[] m_Args;

        void Main(object sender, StartupEventArgs e)
        {
            m_Args = e.Args;
        }

        public static string[] Args
        {
            get { return m_Args; }
        }
    }
}
