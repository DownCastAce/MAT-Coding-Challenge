using System;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CarCoordinatesProcessor.Mqtt
{
	public class MqttAdapter : IMqttAdapter
	{
		private readonly CarCoordinatesHandler _engine;
		private readonly MqttClient _client;
		private const int Retries = 10;

		public MqttAdapter(CarCoordinatesHandler engine, MqttClient client)
		{
			_engine = engine;
			_client = client;
		}

		public void Connect()
		{
			_client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

			_client.Subscribe(new[] {"carCoordinates"}, new[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});

			int count = 0;
			while (count < Retries)
			{
				int connectionResult = -1;
				try
				{
					connectionResult = _client.Connect(Guid.NewGuid().ToString(), "guest", "guest");
				}
				catch (Exception e)
				{
					Thread.Sleep(1000);
					Console.WriteLine(
						$"Error occured while trying to connect to MQTT : {e.Message}\nError Message : {e.InnerException?.Message}");
				}

				if (connectionResult == 0)
				{
					Console.WriteLine("Successfully connected to the broker!");
					return;
				}

				Thread.Sleep(1000);

				count++;
			}
		}
		
		/// <summary>
		/// This subsribes to the events published by the Mqtt Broker and handles them
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mqttMsgPublishEventArgs"></param>
		private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs mqttMsgPublishEventArgs)
		{
			_engine.Process(Encoding.Default.GetString(mqttMsgPublishEventArgs.Message));
		}
	}
}