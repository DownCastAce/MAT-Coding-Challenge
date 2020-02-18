using System;
using CarCoordinatesProcessor.Models;

namespace CarCoordinatesProcessor.Logic
{
	public static class CarDetailsUpdater
	{
		/// <summary>
		/// Updates the Current and Previous Coordinates of the car
		/// </summary>
		/// <param name="carDetails"></param>
		/// <param name="currentCarCoordinates"></param>
		public static void UpdateCarCoordinates(this CarDetails carDetails, CarCoordinates currentCarCoordinates)
		{
			carDetails.PreviousCarCoordinates = carDetails.CarLocationData;
			carDetails.CarLocationData = currentCarCoordinates;
		}

		/// <summary>
		/// Updates the current cars details for its speed and distances traveled.
		/// Speed is stored in mph and Distance in Meters
		/// </summary>
		/// <param name="carDetails"></param>
		public static void UpdateCarSpeed(this CarDetails carDetails)
		{
			if (carDetails.PreviousCarCoordinates != null)
			{
				double distanceTraveled = CalculateCarDistance.Distance(carDetails.CarLocationData.Location, carDetails.PreviousCarCoordinates.Location);
				double elapsedTime = Math.Abs(carDetails.CarLocationData.TimeStamp - carDetails.PreviousCarCoordinates.TimeStamp) / 1000.00;
				carDetails.DistancedTraveled += distanceTraveled;
				carDetails.CurrentSpeed = CalculateCarSpeed(distanceTraveled, elapsedTime);
				if (!carDetails.AverageSpeedPerLap.ContainsKey(carDetails.LapNumber))
				{
					carDetails.AverageSpeedPerLap.Add(carDetails.LapNumber, carDetails.CurrentSpeed);
				}
				else
				{
					carDetails.AverageSpeedPerLap[carDetails.LapNumber] = (carDetails.AverageSpeedPerLap[carDetails.LapNumber] + carDetails.CurrentSpeed) / 2;
				}
			}
		}

		/// <summary>
		/// Updates the Rank of the Car and its previous rank
		/// </summary>
		/// <param name="carDetails"></param>
		/// <param name="rank"></param>
		public static void UpdateRank(this CarDetails carDetails, int rank)
		{
			carDetails.PreviousRank = carDetails.Rank;
			carDetails.Rank = rank;
		}

		/// <summary>
		/// Calculates Speeds based on Distances/Time and Converts.
		/// Converts Meters into Miles and Seconds into Hours
		/// </summary>
		/// <param name="distanceTraveled"></param>
		/// <param name="elapsedTime"></param>
		/// <returns></returns>
		private static double CalculateCarSpeed(double distanceTraveled, double elapsedTime)
		{
			return ((distanceTraveled / 1609.344) / (elapsedTime / 3600.00));
		}
	}
}
