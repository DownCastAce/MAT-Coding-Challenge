using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Logic;
using ConsoleApp1.Models;
using ConsoleApp1.mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ConsoleApp1
{
	public class MyApp
	{
		private Dictionary<int, CarDetails> _allCarDetails = new Dictionary<int, CarDetails>();
		private readonly MqttAdapter _client;
		private long _currentTimeStamp = long.MinValue;
		private long _previousTimeStamp = long.MinValue;
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
	        var currentCarDetails = JsonSerializer.Deserialize<CarCoordinates>(Encoding.Default.GetString(e.Message));
	        _currentTimeStamp = currentCarDetails.TimeStamp;

			if (_allCarDetails.ContainsKey(currentCarDetails.CarIndex))
	        {
				#region Update Car Details

				_allCarDetails[currentCarDetails.CarIndex].UpdateCarCoordinates(currentCarDetails);
				_allCarDetails[currentCarDetails.CarIndex].UpdateCarSpeed();

				#endregion

				CarDetails currentCarInformation = _allCarDetails[currentCarDetails.CarIndex];

				#region Publish Car Speed

				CarStatus currentCarStatusSpeed = new CarStatus
				{
					TimeStamp = currentCarInformation.CarLocationData.TimeStamp,
					CarIndex = currentCarDetails.CarIndex,
					Type = "SPEED",
					Value = Convert.ToInt32(currentCarInformation.CurrentSpeed)
				};

				_client.PublishMessage("carStatus", currentCarStatusSpeed);

				#endregion

				#region Update Positions

				if (_currentTimeStamp != _previousTimeStamp)
				{
					var allCars = _allCarDetails.Values.OrderByDescending(x => x.LapNumber).ThenByDescending(x => x.DistancedTraveled);

					int position = 1;
					foreach (var carDetails in allCars)
					{
						carDetails.UpdateRank(position++);
					}
					
					Parallel.ForEach(allCars, (carDetails) =>
					{
						CarStatus currentCarStatusPosition = new CarStatus
						{
							TimeStamp = carDetails.CarLocationData.TimeStamp,
							CarIndex = carDetails.CarLocationData.CarIndex,
							Type = "POSITION",
							Value = carDetails.Rank
						};

						_client.PublishMessage("carStatus", currentCarStatusPosition);
					});
				}

				#endregion

				#region Update Lap

				if (CalculateCarDistance.Distance(_startLine, currentCarInformation.CarLocationData.Location) < 50.00 && (Math.Abs(currentCarInformation.PreviousLapTime - currentCarDetails.TimeStamp ) / 1000.00) > 30.00)
				{
					Event lapCompleteEvent = new Event
					{
						TimeStamp = currentCarDetails.TimeStamp,
						Message = $"Car {currentCarDetails.CarIndex} | Lap {currentCarInformation.LapNumber} | Average Speed : {Convert.ToInt32(currentCarInformation.AverageSpeedPerLap[currentCarInformation.LapNumber])}mph"
					};

					_client.PublishMessage("events", lapCompleteEvent);

					currentCarInformation.PreviousLapTime = currentCarDetails.TimeStamp;
					currentCarInformation.LapNumber += 1;
				}

				#endregion
			}
	        else
	        {
				CarDetails carDetails = new CarDetails
				{
					CarLocationData = currentCarDetails,
					Rank = currentCarDetails.CarIndex,
					CurrentSpeed = 0,
					CarIndex = currentCarDetails.CarIndex,
					DistancedTraveled = 0,
					PreviousRank = 0,
					AverageSpeedPerLap = new Dictionary<int, double>(){ {1, 0.0} },
					LapNumber = 1,
					PreviousLapTime = currentCarDetails.TimeStamp
				};

				_allCarDetails.Add(currentCarDetails.CarIndex, carDetails);

				_previousTimeStamp = currentCarDetails.TimeStamp;
	        }
        }
	}
}
