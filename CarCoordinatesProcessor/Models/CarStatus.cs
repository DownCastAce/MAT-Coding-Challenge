using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
    public class CarStatus
    {
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }
        [JsonProperty("carIndex")]
        public int CarIndex { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}