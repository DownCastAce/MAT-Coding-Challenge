using System;
using CarCoordinatesProcessor.Models;

namespace CarCoordinatesProcessor.Extension
{
    public static class CarDetailsExtension
    {
        public static CarStatus GenerateCarStatusPayload(this CarDetails carDetails, StatusPayloadType statusPayloadType)
        {
            CarStatus result = new CarStatus
            {
                CarIndex = carDetails.CarIndex,
                TimeStamp = carDetails.CarLocationData.TimeStamp,
                Type = statusPayloadType.ToString()
            };
            
            switch (statusPayloadType)
            {
                case StatusPayloadType.Position:
                    result.Value = carDetails.Rank;
                    break;
                case StatusPayloadType.Speed:
                    result.Value = (int)carDetails.CurrentSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusPayloadType), statusPayloadType, null);
            }

            return result;
        }

        public static Event GenerateCarEvent(this CarDetails carDetails, EventType eventType)
        {
            Event result = new Event()
            {
                TimeStamp = carDetails.CarLocationData.TimeStamp
            };
            
            switch (eventType)
            {
                case EventType.Lapcomplete:
                    result.Message = $"Car {carDetails.CarIndex} | Lap {carDetails.LapNumber} | Average Speed : {Convert.ToInt32(carDetails.AverageSpeedPerLap[carDetails.LapNumber])}mph";
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