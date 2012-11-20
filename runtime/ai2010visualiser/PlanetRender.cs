using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using mtBot;

namespace ai2010visualiser
{
    public class PlanetRender
    {
        const int c_RadiusScale = 3;

        private readonly GeometryGroup m_gg = new GeometryGroup(  );

        public PlanetRender( Planet planet )
        {
            EllipseGeometry planetGeom;
            FormattedText text;
            Geometry textGeom;

            double radius = Math.Sqrt(planet.GrowthRate) / c_RadiusScale; /* Area proportional to growth rate */

            planetGeom = new EllipseGeometry(new Point(planet.X, planet.Y), radius, radius);
            m_gg.Children.Add(planetGeom);

            text = new FormattedText(
                planet.ShipCount.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Tahoma"),
                radius < 0.6 ? 0.6 : radius,
                Brushes.Black);
            textGeom = text.BuildGeometry(new Point(planet.X - text.Width / 2, planet.Y - text.Height / 2));

            m_gg.Children.Add(textGeom);

            m_gg.Freeze(  );
        }

        public GeometryGroup Geometry
        {
            get { return m_gg; }
        }
    }
}