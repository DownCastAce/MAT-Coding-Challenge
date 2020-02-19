using System;
using System.Collections.Generic;
using System.Text;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CarCoordinatesProcessor
{
	public class MyApp
	{
		private readonly CarCoordinatesHandler _engine;
		private Imqtt Client { get; }

		public MyApp()
		{
			Client = new MqttAdapter(client_MqttMsgPublishReceived);
			_engine = new CarCoordinatesHandler(Client, new Dictionary<int, CarDetails>());
		}

		public void Run()
		{
			Event test = new Event
			{
				TimeStamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond,
				Message = "The Race Begins"
			};

			Client.PublishMessage("events", test);
		}

		void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			_engine.Process(Encoding.Default.GetString(e.Message));
		}
	}
}