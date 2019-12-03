using static System.Math;

namespace StarSvgGenerator
{
    public static class CoordinateCalculations
    {

        public static double GetEquatorialLatitude(double ra, double dec)
        {
            var raRad = (ra * 15) * Constants.DegToRad;
            var decRad = dec * Constants.DegToRad;

            return Asin((Sin(decRad) * Cos(Constants.EarthObilquityRad)) - (Cos(decRad) * Sin(Constants.EarthObilquityRad) * Sin(raRad))) * Constants.RadToDeg;
        }

        public static double GetEquatorialLongitude(double ra, double dec)
        {
            var raRad = (ra * 15) * Constants.DegToRad;
            var decRad = dec * Constants.DegToRad;

            return Atan2(Sin(raRad) * Cos(Constants.EarthObilquityRad) + Tan(decRad) * Sin(Constants.EarthObilquityRad), Cos(raRad)) * Constants.RadToDeg;
        }
    }
}
