namespace CarCoordinatesProcessor.mqtt
{
	public interface Imqtt
	{
		void PublishMessage<TT>(string topic, TT informationToPublish);
	}
}
