using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
	public class CarCoordinates
	{
		public CarCoordinates() { }
		
		[JsonProperty("timestamp")]
		public long TimeStamp { get; set; }
		[JsonProperty("carIndex")]
		public int CarIndex { get; set; }
		[JsonProperty("location")]
		public Location Location { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

	public class Location
	{
		[JsonProperty("lat")]
		public double Latitude { get; set; }
		[JsonProperty("long")]
		public double Longitude { get; set; }
	}
}
