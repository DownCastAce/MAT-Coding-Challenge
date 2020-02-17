using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.mqtt
{
	interface Imqtt
	{
		void PublishMessage<t>(string topic, t informationToPublish);
	}
}
