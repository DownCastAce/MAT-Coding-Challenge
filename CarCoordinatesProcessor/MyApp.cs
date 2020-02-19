using System;
using System.Collections.Generic;
using System.Text;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CarCoordinatesProcessor
{
	public class MyApp
	{
		private Dictionary<int, CarDetails> _allCarDetailsCache = new Dictionary<int, CarDetails>();
		private readonly CarCoordinatesHandler _engine;
		internal Imqtt _client { get; set; }

		public MyApp()
		{
			_client = new MqttAdapter(client_MqttMsgPublishReceived);
			_engine = new CarCoordinatesHandler(_client, new Dictionary<int, CarDetails>());
		}

		public void Run()
		{
			Event test = new Event
			{
				TimeStamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond,
				Message = "The Race Begins"
			};

			_client.PublishMessage("events", test);
		}

		void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			_engine.Process(Encoding.Default.GetString(e.Message));
		}
	}
}
