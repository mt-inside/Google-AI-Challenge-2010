using System.Globalization;
using System.Windows;
using System.Windows.Media;
using mtBot;

namespace ai2010visualiser
{
    public class FleetRender
    {
        private readonly GeometryGroup m_gg = new GeometryGroup();

        public FleetRender( Fleet fleet, bool? lines )
        {
            LineGeometry line;
            FormattedText text;
            Geometry textGeom;

            double x = fleet.SourcePlanet.X +
                       ((fleet.DestinationPlanet.X - fleet.SourcePlanet.X) *
                        ((double)(fleet.TotalTripLength - fleet.TurnsRemaining) / fleet.TotalTripLength));
            double y = fleet.SourcePlanet.Y +
                       ((fleet.DestinationPlanet.Y - fleet.SourcePlanet.Y) *
                        ((double)(fleet.TotalTripLength - fleet.TurnsRemaining) / fleet.TotalTripLength));

            if (lines ?? false)
            {
                line = new LineGeometry(new Point(x, y), new Point(fleet.DestinationPlanet.X, fleet.DestinationPlanet.Y));
                m_gg.Children.Add(line);                
            }

            text = new FormattedText(
                fleet.ShipCount.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Tahoma"),
                0.8,
                Brushes.Black);
            textGeom = text.BuildGeometry(new Point(x - text.Width / 2, y - text.Height / 2));
            m_gg.Children.Add(textGeom);

            m_gg.Freeze();
        }

        public GeometryGroup Geometry
        {
            get { return m_gg; }
        }
    }
}