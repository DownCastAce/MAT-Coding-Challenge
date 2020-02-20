using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace CarCoordinatesProcessor.Mqtt
{
	public class MqttPublisher : IMqttPublisher
	{
		private readonly MqttClient _client;

		public MqttPublisher(MqttClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Publish Messages to the chosen Topic
		/// </summary>
		/// <typeparam name="TT"></typeparam>
		/// <param name="topic"></param>
		/// <param name="informationToPublish"></param>
		public void PublishMessage<TT>(string topic, TT informationToPublish)
		{
			_client.Publish(topic, Encoding.UTF8.GetBytes(informationToPublish.ToString()));
		}
	}
}
