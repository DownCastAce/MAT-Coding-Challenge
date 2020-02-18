using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CarCoordinatesProcessor.mqtt
{

	public class MqttAdapter : Imqtt
	{
		private readonly MqttClient _client;

		public MqttAdapter(MqttClient.MqttMsgPublishEventHandler clientMqttMsgPublishReceived)
		{
			_client = new MqttClient("127.0.0.1");
			_client.MqttMsgPublishReceived += clientMqttMsgPublishReceived;
			
			_client.Subscribe(new string[] { "carCoordinates" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
			_client.Connect(Guid.NewGuid().ToString());
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
