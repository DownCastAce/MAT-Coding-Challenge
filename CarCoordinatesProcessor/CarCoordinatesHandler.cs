using System;
using System.Collections.Generic;
using System.Linq;
using CarCoordinatesProcessor.Extension;
using CarCoordinatesProcessor.Logic;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.mqtt;
using Newtonsoft.Json;

namespace CarCoordinatesProcessor
{
	public class CarCoordinatesHandler
	{
		private readonly Location _startLine = new Location {Latitude = 52.069342, Longitude = -1.022140};
		private Dictionary<int, CarDetails> _allCarDetailsCache;
		private Imqtt Client { get; }

		public CarCoordinatesHandler(Imqtt client, Dictionary<int, CarDetails> carDetails)
		{
			Client = client;
			_allCarDetailsCache = carDetails;
		}

		/// <summary>
		/// Process the Car Coordinates and update cache and publish relevant Status and Events
		/// </summary>
		/// <param name="carCoordinatesPayload"></param>
		public void Process(string carCoordinatesPayload)
		{
			CarCoordinates currentCarDetails = JsonConvert.DeserializeObject<CarCoordinates>(carCoordinatesPayload);
			if (_allCarDetailsCache.ContainsKey(currentCarDetails.CarIndex))
			{
				#region Update Car Details

				CarDetails currentCarInformation = _allCarDetailsCache[currentCarDetails.CarIndex];
				currentCarInformation.UpdateCarCoordinates(currentCarDetails);
				currentCarInformation.UpdateCarSpeed();
				currentCarInformation.UpdateRank(ReorderAllCarsAndGetCurrentCarsPosition(currentCarInformation));

				#endregion

				#region Publish Car Speed

				Client.PublishMessage("carStatus",
					currentCarInformation.GenerateCarStatusPayload(StatusPayloadType.Speed));

				#endregion

				#region Update Position Of Car if Changed

				if (currentCarInformation.Rank != currentCarInformation.PreviousRank)
				{
					Client.PublishMessage("carStatus",
						currentCarInformation.GenerateCarStatusPayload(StatusPayloadType.Position));
				}

				#endregion

				#region Update Lap

				if (CalculateCarDistance.Distance(_startLine, currentCarInformation.CarLocationData.Location) < 50.00 &&
				    (Math.Abs(currentCarInformation.PreviousLapTime - currentCarDetails.TimeStamp) / 1000.00) > 30.00)
				{
					Client.PublishMessage("events", currentCarInformation.GenerateCarEvent(EventType.Lapcomplete));

					currentCarInformation.PreviousLapTime = currentCarDetails.TimeStamp;
					currentCarInformation.LapNumber++;
				}

				#endregion
			}
			else
			{
				CarDetails carDetails = new CarDetails
				{
					CarLocationData = currentCarDetails,
					Rank = -1,
					CurrentSpeed = 0,
					CarIndex = currentCarDetails.CarIndex,
					DistancedTraveled = 0,
					PreviousRank = 0,
					AverageSpeedPerLap = new Dictionary<int, double>() {{1, 0.0}},
					LapNumber = 1,
					PreviousLapTime = currentCarDetails.TimeStamp
				};

				_allCarDetailsCache.Add(currentCarDetails.CarIndex, carDetails);
			}
		}

		/// <summary>
		/// Orders all cars positions and returns the current cars position
		/// </summary>
		/// <param name="currentCarInformation"></param>
		/// <returns></returns>
		private int ReorderAllCarsAndGetCurrentCarsPosition(CarDetails currentCarInformation)
		{
			return _allCarDetailsCache.Values.OrderByDescending(x => x.LapNumber)
				.ThenByDescending(x => x.DistancedTraveled).Select(x => x.CarIndex).ToList()
				.IndexOf(currentCarInformation.CarIndex) + 1;
		}
	}
}