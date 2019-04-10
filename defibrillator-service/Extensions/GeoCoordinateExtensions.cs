using GeoCoordinatePortable;
using System;

namespace DefibrillatorService.Extensions
{
    public static class GeoCoordinateExtensions
    {
        public static GeoCoordinate OffsetBy(this GeoCoordinate center, double dx, double dy)
        {
            double pi = Math.PI;
            double lat = center.Latitude + (180 / pi) * (dy / 6378137);
            double lon = center.Longitude + (180 / pi) * (dx / 6378137) / Math.Cos(Math.PI / 180.0 * center.Latitude);

            return new GeoCoordinate(lat, lon);
        }
    }
}
