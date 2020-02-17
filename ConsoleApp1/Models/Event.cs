using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Models
{
    public class Event
    {
        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        [JsonPropertyName("text")]
        public string Message { get; set; }
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}