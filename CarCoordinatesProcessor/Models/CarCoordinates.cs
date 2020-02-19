using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
	public class CarCoordinates
	{
		/// <summary>
		/// Timestamp for current location
		/// </summary>
		[JsonProperty("timestamp")]
		public long TimeStamp { get; set; }

		/// <summary>
		/// Car Identifier
		/// </summary>
		[JsonProperty("carIndex")]
		public int CarIndex { get; set; }

		/// <summary>
		/// GPS Coordinates
		/// </summary>
		[JsonProperty("location")]
		public Location Location { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

	public class Location
	{
		[JsonProperty("lat")] public double Latitude { get; set; }
		[JsonProperty("long")] public double Longitude { get; set; }
	}
}