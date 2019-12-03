using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CsvHelper;

namespace StarSvgGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: StarSvgGenerator.exe hygdata_v3.csv stars.svg");
                return;
            }

            string input = args[0];
            string output = args[1];

            IEnumerable<CsvStar> stars;
            using (var reader = new StreamReader(@"hygdata_v3.csv"))
            {
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.RegisterClassMap<StarCsvMap>();
                    stars = csv.GetRecords<CsvStar>()
                        .Where(x => 
                            x.EquatorialLatitude > Constants.MinLat && 
                            x.EquatorialLatitude < Constants.MaxLat && 
                            x.Magnitude < Constants.MaxMagnitude && 
                            x.Magnitude > Constants.MinMagnitude)
                        .ToList();
                }
            }

            var svgStars = stars.Select(PlotStar).ToList();

            var svg = MakeSvg(svgStars);
            svg.Save("stars.svg");
        }

        static SvgStar PlotStar(CsvStar star)
        {
            // Map to 270deg to -90 so they are plotted clockwise starting at the top
            var rad = Scale(star.EquatorialLongitude, 0, 360, Math.PI * 1.5, -Math.PI * 0.5);
            var r = Scale(star.EquatorialLatitude, Constants.MinLat, Constants.MaxLat, Constants.InnerRadius, Constants.OuterRadius);
            var size = Scale(star.Magnitude, Constants.MinMagnitude, Constants.MaxMagnitude, Constants.MaxStarRadius, Constants.MinStarRadius);
            var opacity = Scale(star.Magnitude, Constants.MinMagnitude, Constants.MaxMagnitude, Constants.MaxStarOpacity, Constants.MinStarOpacity);

            var x = Math.Cos(rad) * r;
            var y = Math.Sin(rad) * r;

            return new SvgStar
            {
                CenterX = x,
                CenterY = y,
                Radius = size,
                Name = star.Name,
                Opacity = opacity,
            };
        }

        static XDocument MakeSvg(IEnumerable<SvgStar> stars)
        {
            var svg = XNamespace.Get("http://www.w3.org/2000/svg");
            var doc = new XDocument(
                new XDeclaration("1.0", "utf8", "yes"),
                new XElement(svg + "svg",
                    new XAttribute("xlmns", svg),
                    new XAttribute("viewBox", $"-{Constants.OuterRadius} -{Constants.OuterRadius} {Constants.OuterRadius * 2} {Constants.OuterRadius * 2}"),
                    new XElement(svg + "circle",
                        new XAttribute("cx", 0),
                        new XAttribute("cy", 0),
                        new XAttribute("r", Constants.OuterRadius),
                        new XAttribute("fill", "black")),
                    new XElement(svg + "circle",
                        new XAttribute("cx", 0),
                        new XAttribute("cy", 0),
                        new XAttribute("r", Constants.InnerRadius),
                        new XAttribute("fill", "white")),
                    stars.Select(x => new XElement(svg + "circle",
                        new XAttribute("cx", x.CenterX),
                        new XAttribute("cy", x.CenterY),
                        new XAttribute("r", x.Radius),
                        new XAttribute("fill", "white"),
                        new XAttribute("stroke-width", 0),
                        new XAttribute("opacity", x.Opacity))),
                    Constants.IncludeNames ? 
                        stars.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x =>
                            new XElement(svg + "text",
                            new XAttribute("x", x.CenterX),
                            new XAttribute("y", x.CenterY),
                            new XAttribute("style", "font-size: 40px; fill: white"),
                            x.Name)) : 
                        null
                ));
            return doc;
        }

        static double Scale(double value, double rawMin, double rawMax, double min, double max)
        {
            return (((value - rawMin) * (max - min)) / (rawMax - rawMin)) + min;
        }
    }
}
