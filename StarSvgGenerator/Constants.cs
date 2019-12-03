using System;

namespace StarSvgGenerator
{
    public static class Constants
    {
        public const double DegToRad = Math.PI / 180;
        public const double RadToDeg = 1 / DegToRad;

        public const double EarthObilquityDegrees = 23.4367;
        public const double EarthObilquityRad = EarthObilquityDegrees * DegToRad;
     
        public const double LatitudeRange = 30;
        public const double MinLat = -LatitudeRange;
        public const double MaxLat = +LatitudeRange;
        public const double MinMagnitude = -2;
        public const double MaxMagnitude = 5.5;

        public const int InnerRadius = 800;
        public const int OuterRadius = 2000;

        public const double MinStarRadius = 2;
        public const double MaxStarRadius = 10;

        public const double MinStarOpacity = 0.5;
        public const double MaxStarOpacity = 1.0;

        public const bool IncludeNames = true;
    }
}
