using System.Collections.Generic;

namespace ConsoleApp1.Models
{
	public class CarDetails
	{
		public CarCoordinates CarLocationData { get; set; }
		public CarCoordinates PreviousCarCoordinates { get; set; }
		public double DistancedTraveled { get; set; }
		public double CurrentSpeed { get; set; }
		public int Rank { get; set; }
		public int PreviousRank { get; set; }
		public int CarIndex { get; set; }
		public int LapNumber { get; set; }
		public Dictionary<int, double> AverageSpeedPerLap { get; set; }
		public long PreviousLapTime { get; set; }
	}
}
