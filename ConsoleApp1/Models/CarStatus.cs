using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Models
{
    public class CarStatus
    {
        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }
        [JsonPropertyName("carIndex")]
        public int CarIndex { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public int Value { get; set; }
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}