using System;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CarCoordinatesProcessor.mqtt
{

	public class MqttAdapter : Imqtt
	{
		private readonly MqttClient _client;
		private int retrys = 10;

		public MqttAdapter(MqttClient.MqttMsgPublishEventHandler clientMqttMsgPublishReceived)
		{
			_client = new MqttClient(GetBrokerHostName());
			_client.MqttMsgPublishReceived += clientMqttMsgPublishReceived;
			
			_client.Subscribe(new string[] { "carCoordinates" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
			bool connected = false;
			int count = 0;
			while (!connected && count < retrys)
			{
				var connectionResult = -1;
				try
				{
					connectionResult = _client.Connect(Guid.NewGuid().ToString(), "guest", "guest");
				}
				catch (Exception e)
				{
					Thread.Sleep(1000);
					Console.WriteLine($"Error occured while trying to connect to MQTT : {e.Message}\nError Message : {e.InnerException?.Message}");
				}

				if (connectionResult == 0)
				{
					connected = true;
					Console.WriteLine("Successfully connected to the broker!");
				}
				else
				{
					Thread.Sleep(1000);
				}

				count++;
			}

		}

		/// <summary>
		/// Retrieve the hostName to use to connect to the Broker
		/// </summary>
		/// <returns></returns>
		private static string GetBrokerHostName()
		{
			#if DEBUG
				return "127.0.0.1";
			#endif
			return "broker";
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
