using System;
using System.Diagnostics.CodeAnalysis;
using CarCoordinatesProcessor.Extension;
using CarCoordinatesProcessor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CarCoordinatesProcessorTests.Extension
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CarDetailsExtensionShould
    {
        private const string ExpectedPositionCarStatus = "{\"timestamp\":1582061329767,\"carIndex\":1,\"type\":\"POSITION\",\"value\":1}";
        private const string ExpectedSpeedCarStatus = "{\"timestamp\":1582061329767,\"carIndex\":1,\"type\":\"SPEED\",\"value\":100}";
        private const string ExpectedCarEvent = "{\"timestamp\":1582061329767,\"text\":\"Car 1 | Lap 1 | Average Speed : 100mph\"}";
        
        private const string FirstCarAddedPayload = "{\"CarLocationData\":{\"timestamp\":1582061329767,\"carIndex\":1,\"location\":{\"lat\":52.06905479305401,\"long\":-1.0225002257078422}},\"PreviousCarCoordinates\":null,\"DistancedTraveled\":0.0,\"CurrentSpeed\":100.0,\"Rank\":1,\"PreviousRank\":0,\"CarIndex\":1,\"LapNumber\":1,\"AverageSpeedPerLap\":{\"1\":100.0},\"PreviousLapTime\":1582061329767}";
        
        [TestMethod]
        public void Generate_Position_CarStatus_Payload()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(FirstCarAddedPayload);

            CarStatus actualResult = carDetails.GenerateCarStatusPayload(StatusPayloadType.Position);
            
            Assert.AreEqual(ExpectedPositionCarStatus, actualResult.ToString());
        }
        
        [TestMethod]
        public void Generate_Speed_CarStatus_Payload()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(FirstCarAddedPayload);

            CarStatus actualResult = carDetails.GenerateCarStatusPayload(StatusPayloadType.Speed);
            
            Assert.AreEqual(ExpectedSpeedCarStatus, actualResult.ToString());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Throw_Exception_When_Invalid_StatusPayload_Is_Used()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(FirstCarAddedPayload);

            carDetails.GenerateCarStatusPayload(0);
        }
        
        
        [TestMethod]
        public void Generate_CarEvent_Payload()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(FirstCarAddedPayload);

            Event actualResult = carDetails.GenerateCarEvent(EventType.Lapcomplete);
            
            Assert.AreEqual(ExpectedCarEvent, actualResult.ToString());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Throw_Exception_When_Invalid_EventType_Is_Used()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(FirstCarAddedPayload);

            carDetails.GenerateCarEvent(0);
        }
        
    }
}