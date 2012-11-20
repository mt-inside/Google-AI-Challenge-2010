using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using mtBot;

namespace ai2010visualiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_Playing;
        private DispatcherTimer m_Timer = new DispatcherTimer();
        private int m_Interval = 500;

        public MainWindow( )
        {
            InitializeComponent( );

            if( App.Args.Length >= 2 )
            {
                m_Interval = int.Parse( App.Args[1] );
            }

            if( App.Args.Length >= 1 )
            {
                Load( App.Args[0] );
            }

            m_Timer.Interval = TimeSpan.FromMilliseconds(m_Interval);
            m_Timer.Tick += TimerCallback;
        }

        private void RenderGameState( GameState gs )
        {
            /* Geometry */
            List<GeometryGroup> planetsGeoms = new List<GeometryGroup>
            {
                new GeometryGroup(), new GeometryGroup(  ), new GeometryGroup(  )
            };

            gs.Planets.ForEach( p => planetsGeoms[ p.Owner.Id ].Children.Add( new PlanetRender( p ).Geometry ) );

            /* Rendering */
            List<GeometryDrawing> planetsDrawers = new List<GeometryDrawing>
            {
                new GeometryDrawing( ), new GeometryDrawing( ), new GeometryDrawing( )
            };

            planetsGeoms.ForEach( pg => planetsDrawers[ planetsGeoms.IndexOf( pg ) ].Geometry = pg );
            
            planetsDrawers[ Players.Neutral.Id ].Brush = new SolidColorBrush( Color.FromArgb( 255, 128, 128, 128 ) );
            planetsDrawers[ Players.Me.Id      ].Brush = new SolidColorBrush( Color.FromArgb( 255,   0, 255,   0 ) );
            planetsDrawers[ Players.You.Id     ].Brush = new SolidColorBrush( Color.FromArgb( 255, 255,   0,   0 ) );


            /* Geometry */
            List<GeometryGroup> fleetsGeoms = new List<GeometryGroup>
            {
                new GeometryGroup(), new GeometryGroup(  ), new GeometryGroup(  )
            };

            gs.Fleets.ForEach(f => fleetsGeoms[f.Owner.Id].Children.Add(new FleetRender(f, checkBoxLines.IsChecked).Geometry));

            /* Rendering */
            List<GeometryDrawing> fleetsDrawers = new List<GeometryDrawing>
            {
                new GeometryDrawing( ), new GeometryDrawing( ), new GeometryDrawing( )
            };

            fleetsGeoms.ForEach(fg => fleetsDrawers[fleetsGeoms.IndexOf(fg)].Geometry = fg);

            fleetsDrawers[Players.Me.Id ].Brush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            fleetsDrawers[Players.Me.Id ].Pen = new Pen(new SolidColorBrush(Color.FromArgb(128, 0, 255, 0)), 0.05);
            fleetsDrawers[Players.You.Id].Brush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            fleetsDrawers[Players.You.Id].Pen = new Pen( new SolidColorBrush(Color.FromArgb(128, 255, 0, 0)), 0.05);
 

            DrawingGroup udg = new DrawingGroup(  );

            planetsDrawers.ForEach( udg.Children.Add );
            fleetsDrawers.ForEach( udg.Children.Add );

            m_CanvasUniverse.Background = new DrawingBrush(udg);
        }

        private void RenderGraphs( IEnumerable<int> myShipCounts, IEnumerable<int> yourShipCounts )
        {
            List<GeometryDrawing> graphDrawers = new List<GeometryDrawing>
            {
                new GeometryDrawing( ), new GeometryDrawing( ), new GeometryDrawing()
            };

            graphDrawers[Players.Me.Id].Geometry = RenderGraph(myShipCounts, myShipCounts.Concat( yourShipCounts ).Max());
            graphDrawers[Players.Me.Id].Pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)), 1);
            graphDrawers[Players.Me.Id].Brush = new SolidColorBrush( Color.FromArgb( 128, 0, 255, 0 ) );
            graphDrawers[Players.You.Id].Geometry = RenderGraph(yourShipCounts, myShipCounts.Concat(yourShipCounts).Max());
            graphDrawers[Players.You.Id].Pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)), 1);
            graphDrawers[Players.You.Id].Brush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

            DrawingGroup gdg = new DrawingGroup();

            graphDrawers.ForEach(gdg.Children.Add);

            m_CanvasGraph.Background = new DrawingBrush(gdg);
        }

        private Geometry RenderGraph( IEnumerable<int> ys, int max )
        {
            StreamGeometry sg = new StreamGeometry();
            sg.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext sgc = sg.Open())
            {
                ys = ys.Select(y => max - y).ToList();

                sgc.BeginFigure(new Point(0, ys.First()), true, false);

                int x = 0;
                foreach (int y in ys.Skip(1))
                {
                    x++;
                    sgc.LineTo(new Point(x, y), true, false);
                }

                sgc.LineTo(new Point(x, max), false, false);
                sgc.LineTo(new Point(0, max), false, false);
            }

            sg.Freeze();

            return sg;
        }

        private void sliderTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RenderGameState( GameStates.Singleton[ (int)e.NewValue ] );
            labelTurns.Content = String.Format( "Turn {0}/{1}", ((int)e.NewValue) + 1, GameStates.Singleton.Count );

            if( (int)e.NewValue >= GameStates.Singleton.Count - 1 ) Pause( );
        }

        private void TimerCallback(object sender, EventArgs args)
        {
            sliderTime.Value++;
        }

        private void Load( string path )
        {
            try
            {
                GameStates.Singleton.ParseReplay(File.OpenText(path).ReadToEnd());    
            }
            catch( Exception e )
            {
                
            }

            RenderGraphs(GameStates.Singleton.Select(gs => gs.Planets.MyPlanets.Sum(p => p.ShipCount) + gs.Fleets.MyFleets.Sum( f => f.ShipCount )),
                         GameStates.Singleton.Select(gs => gs.Planets.YourPlanets.Sum(p => p.ShipCount) + gs.Fleets.YourFleets.Sum(f => f.ShipCount)));
            
            RenderGameState(GameStates.Singleton[(int)sliderTime.Minimum]);

            sliderTime.Maximum = GameStates.Singleton.Count - 1;
            sliderTime.Value = 0;
            sliderTime.IsEnabled = true;
            Play();
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Load( dlg.FileName );
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(  );
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            sliderTime.Value = sliderTime.Minimum;
        }

        private void buttonEnd_Click(object sender, RoutedEventArgs e)
        {
            sliderTime.Value = sliderTime.Maximum;
        }

        private void buttonStepBack_Click(object sender, RoutedEventArgs e)
        {
            if( sliderTime.Value > sliderTime.Minimum )
            {
                sliderTime.Value--;    
            }
        }

        private void buttonStepForward_Click(object sender, RoutedEventArgs e)
        {
            if (sliderTime.Value < sliderTime.Maximum)
            {
                sliderTime.Value++;
            }
        }

        private void buttonRewind_Click(object sender, RoutedEventArgs e)
        {
            m_Interval *= 2;
            m_Timer.Interval = TimeSpan.FromMilliseconds(m_Interval);
        }

        private void buttonFastForward_Click(object sender, RoutedEventArgs e)
        {
            m_Interval /= 2;
            m_Timer.Interval = TimeSpan.FromMilliseconds(m_Interval);
        }

        private void buttonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (m_Playing)
            {
                Pause(  );
            }
            else
            {
                if ( sliderTime.Value < GameStates.Singleton.Count - 1 )
                {
                    Play();                    
                }
            }
        }

        private void Play()
        {
            m_Timer.Start();
            buttonPlayPause.Content = "||";

            m_Playing = true;
        }

        private void Pause( )
        {
            m_Timer.Stop();
            buttonPlayPause.Content = ">";

            m_Playing = false;
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show( "ai2010visualiser.\nCopyright 2010 Matthew Turner, all rights reserved.\nNo warranty etc." );
        }
    }
}
