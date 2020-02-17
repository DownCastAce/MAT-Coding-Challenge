using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Models
{
	public class CarCoordinates
	{
		public CarCoordinates() { }
		
		[JsonPropertyName("timestamp")]
		public long TimeStamp { get; set; }
		[JsonPropertyName("carIndex")]
		public int CarIndex { get; set; }
		[JsonPropertyName("location")]
		public Location Location { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}

	public class Location
	{
		[JsonPropertyName("lat")]
		public double Latitude { get; set; }
		[JsonPropertyName("long")]
		public double Longitude { get; set; }
	}
}
