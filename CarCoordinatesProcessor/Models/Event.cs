using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
    public class Event
    {
	    /// <summary>
	    /// Timestamp for current location
	    /// </summary>
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// Message to Display to Fans
        /// </summary>
        [JsonProperty("text")]
        public string Message { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}