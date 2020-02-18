using CarCoordinatesProcessor.Models;
using GeoCoordinatePortable;

namespace CarCoordinatesProcessor.Logic
{
    public static class CalculateCarDistance
    {
        public static double Distance(Location location1, Location location2)
        {
            GeoCoordinate coordinate1 = new GeoCoordinate(location1.Latitude, location1.Longitude);
            GeoCoordinate coordinate2 = new GeoCoordinate(location2.Latitude, location2.Longitude);
            
            return coordinate1.GetDistanceTo(coordinate2);
            
        }
    }
}