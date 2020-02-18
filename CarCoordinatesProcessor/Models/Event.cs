using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
    public class Event
    {
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        [JsonProperty("text")]
        public string Message { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}