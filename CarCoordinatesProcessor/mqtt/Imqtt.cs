namespace CarCoordinatesProcessor.Mqtt
{
	public interface Imqtt
	{
		void PublishMessage<TT>(string topic, TT informationToPublish);
	}
}