namespace CarCoordinatesProcessor.Mqtt
{
	public interface IMqttPublisher
	{
		void PublishMessage<TT>(string topic, TT informationToPublish);
	}
}
