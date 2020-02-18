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
		private Dictionary<int, CarDetails> _allCarDetails = new Dictionary<int, CarDetails>();
		internal Imqtt _client { get; set; }
		private readonly Location _startLine = new Location { Latitude = 52.069342, Longitude = -1.022140};

		public MyApp()
		{
			_client = new MqttAdapter(client_MqttMsgPublishReceived);
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
	        CarCoordinatesHandler engine = new CarCoordinatesHandler(_client, _allCarDetails);
	        engine.Process(Encoding.Default.GetString(e.Message));
        }
	}
}
