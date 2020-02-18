namespace CarCoordinatesProcessor.mqtt
{
	public interface Imqtt
	{
		void PublishMessage<t>(string topic, t informationToPublish);
	}
}
