using System;
using CarCoordinatesProcessor.Models;

namespace CarCoordinatesProcessor.Extension
{
	public static class CarDetailsExtension
	{
		/// <summary>
		/// Generates the Car Status Payload based on the CarDetails and Payload required
		/// </summary>
		/// <param name="carDetails"></param>
		/// <param name="statusPayloadType"></param>
		/// <returns></returns>
		public static CarStatus GenerateCarStatusPayload(this CarDetails carDetails,
			StatusPayloadType statusPayloadType)
		{
			CarStatus result = new CarStatus
			{
				CarIndex = carDetails.CarIndex,
				TimeStamp = carDetails.CarLocationData.TimeStamp,
				Type = statusPayloadType.ToString().ToUpperInvariant()
			};

			switch (statusPayloadType)
			{
				case StatusPayloadType.Position:
					result.Value = carDetails.Rank;
					break;
				case StatusPayloadType.Speed:
					result.Value = (int) carDetails.CurrentSpeed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(statusPayloadType), statusPayloadType,
						$"The payload type {statusPayloadType} isn't yet support");
			}

			return result;
		}

		/// <summary>
		/// Generates the Car Event Payload based on the CarDetails and Payload required
		/// </summary>
		/// <param name="carDetails"></param>
		/// <param name="eventType"></param>
		/// <returns></returns>
		public static Event GenerateCarEvent(this CarDetails carDetails, EventType eventType)
		{
			Event result = new Event()
			{
				TimeStamp = carDetails.CarLocationData.TimeStamp
			};

			switch (eventType)
			{
				case EventType.Lapcomplete:
					result.Message =
						$"Car {carDetails.CarIndex} | Lap {carDetails.LapNumber} | Average Speed : {(int) carDetails.AverageSpeedPerLap[carDetails.LapNumber]}mph";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			return result;
		}
	}

	public enum StatusPayloadType
	{
		Position = 1,
		Speed = 2
	}

	public enum EventType
	{
		Lapcomplete = 1
	}
}