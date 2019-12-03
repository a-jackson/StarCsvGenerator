namespace StarSvgGenerator
{
    public class CsvStar
    {

        public double RightAscension { get; set; }

        public double Declination { get; set; }

        public double Magnitude { get; set; }

        public string Constellation { get; set; }

        public string Name { get; set; }

        public double EquatorialLatitude => CoordinateCalculations.GetEquatorialLatitude(RightAscension, Declination);

        public double EquatorialLongitude => CoordinateCalculations.GetEquatorialLongitude(RightAscension, Declination);
    }
}
