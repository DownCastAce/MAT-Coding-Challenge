using System.Collections.Generic;

namespace CarCoordinatesProcessor.Models
{
	public class CarDetails
	{
		/// <summary>
		/// Current Car GPS Coordinates of Car
		/// </summary>
		public CarCoordinates CarLocationData { get; set; }

		/// <summary>
		/// Previous GPS Coordinates of Car
		/// </summary>
		public CarCoordinates PreviousCarCoordinates { get; set; }

		/// <summary>
		/// Total Distance Traveled
		/// </summary>
		public double DistancedTraveled { get; set; }

		/// <summary>
		/// Current Speed of Car in mph
		/// </summary>
		public double CurrentSpeed { get; set; }

		/// <summary>
		/// Current Position
		/// </summary>
		public int Rank { get; set; }

		/// <summary>
		/// Previous Position
		/// </summary>
		public int PreviousRank { get; set; }

		/// <summary>
		/// Car Identifier
		/// </summary>
		public int CarIndex { get; set; }

		/// <summary>
		/// Current Lap Number
		/// </summary>
		public int LapNumber { get; set; }

		/// <summary>
		/// Holds Record of each Laps Average Speed
		/// </summary>
		public Dictionary<int, double> AverageSpeedPerLap { get; set; }

		/// <summary>
		/// Timestamp of last completed lap
		/// </summary>
		public long PreviousLapTime { get; set; }
	}
}