using System;
using CarCoordinatesProcessor.Models;
using GeoCoordinatePortable;

namespace CarCoordinatesProcessor.Logic
{
	public static class CarLocationProcessor
	{
		private static readonly Location StartLine = new Location { Latitude = 52.069342, Longitude = -1.022140 };

		/// <summary>
		/// Distance Measured using GeoCoordinates. Distance in Meters.
		/// </summary>
		/// <param name="location1"></param>
		/// <param name="location2"></param>
		/// <returns></returns>
		public static double Distance(Location location1, Location location2)
		{
			GeoCoordinate coordinate1 = new GeoCoordinate(location1.Latitude, location1.Longitude);
			GeoCoordinate coordinate2 = new GeoCoordinate(location2.Latitude, location2.Longitude);

			return coordinate1.GetDistanceTo(coordinate2);
		}

		public static bool FinishedLap(CarDetails currentCarDetails)
		{
			bool reachedFinishedLine = Distance(StartLine, currentCarDetails.CarLocationData.Location) < 50.00;
			bool isFirstRecording = (Math.Abs(currentCarDetails.PreviousLapTime - currentCarDetails.CarLocationData.TimeStamp) / 1000.00) > 30.00;

			return reachedFinishedLine && isFirstRecording;
		}
	}
}