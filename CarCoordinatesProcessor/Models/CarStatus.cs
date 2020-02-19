using Newtonsoft.Json;

namespace CarCoordinatesProcessor.Models
{
    public class CarStatus
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
        /// The Type of Car Status {SPEED|POSITION}
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// Value for the Current Type of Status
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}