using System.Collections.Generic;
using Autofac;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.Mqtt;
using uPLibrary.Networking.M2Mqtt;

namespace CarCoordinatesProcessor
{
	public class Program
	{
		public static IContainer Container;
		
		static void Main(string[] args)
		{
			Container = CreateContainer();

			IMqttAdapter iMqttAdapterBrokerAdapter = Container.Resolve<IMqttAdapter>();

			iMqttAdapterBrokerAdapter.Connect();
		}

		/// <summary>
		/// Setups Dependency Injection Using AutoFac
		/// </summary>
		/// <returns></returns>
		private static IContainer CreateContainer()
		{
			ContainerBuilder builder = new ContainerBuilder();

			//Setup Mqtt Dependencies
			builder.RegisterType<MqttClient>().WithParameter("brokerHostName", GetBrokerHostName()).SingleInstance();
			builder.RegisterType<MqttAdapter>().As<IMqttAdapter>().SingleInstance();
			builder.RegisterType<MqttPublisher>().As<IMqttPublisher>().SingleInstance();

			//Setup Car CoordinatesHandler
			builder.RegisterType<CarCoordinatesHandler>().SingleInstance();
			builder.RegisterType<Dictionary<int, CarDetails>>().SingleInstance();

			return builder.Build();
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
	}
}