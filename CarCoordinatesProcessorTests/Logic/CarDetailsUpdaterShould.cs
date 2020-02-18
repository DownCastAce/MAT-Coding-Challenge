using System.Collections.Generic;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarCoordinatesProcessorTests.Logic
{
    [TestClass]
    public class CarDetailsUpdaterShould
    {
        [TestMethod]
        public void Properly_Update_Current_And_Previous_Car_Coordinates()
        {
            Location startingLocation = new Location{ Latitude = 52.069342, Longitude = -1.022140};
            Location updatedLocation = new Location{ Latitude = 52.070342, Longitude = -1.022140};

            #region Setup Car Coordinates

            CarCoordinates starting = new CarCoordinates
            {
                CarIndex = 0,
                Location = startingLocation,
                TimeStamp = 1
            };
            
            CarCoordinates updated = new CarCoordinates
            {
                CarIndex = 0,
                Location = updatedLocation,
                TimeStamp = 1
            };

            #endregion
            
            CarDetails testData = new CarDetails
            {
                CarLocationData = starting
            };
            
            Assert.IsNull(testData.PreviousCarCoordinates);
            Assert.AreEqual(startingLocation.Latitude, testData.CarLocationData.Location.Latitude);
            Assert.AreEqual(startingLocation.Longitude, testData.CarLocationData.Location.Longitude);
            
            testData.UpdateCarCoordinates(updated);
            
            Assert.IsNotNull(testData.PreviousCarCoordinates);
            Assert.AreEqual(updatedLocation.Latitude, testData.CarLocationData.Location.Latitude);
            Assert.AreEqual(updatedLocation.Longitude, testData.CarLocationData.Location.Longitude);
            Assert.AreEqual(startingLocation.Latitude, testData.PreviousCarCoordinates.Location.Latitude);
            Assert.AreEqual(startingLocation.Longitude, testData.PreviousCarCoordinates.Location.Longitude);
        }

        [TestMethod]
        public void Calculate_Car_Speed_And_Distance_Traveled()
        {
            #region Setup Car Coordinates

            CarCoordinates starting = new CarCoordinates
            {
                CarIndex = 0,
                Location = new Location{ Latitude = 52.06792886633373, Longitude = -1.0238781510887842},
                TimeStamp = 1581977379142
            };
            
            CarCoordinates updated = new CarCoordinates
            {
                CarIndex = 0,
                Location = new Location{ Latitude = 52.06786406260594, Longitude = -1.0239574561135762},
                TimeStamp = 1581977378941
            };

            #endregion
            
            CarDetails testData = new CarDetails
            {
                CarLocationData = starting,
                PreviousCarCoordinates = updated,
                AverageSpeedPerLap = new Dictionary<int, double>(),
                LapNumber = 1
            };
            
            testData.UpdateCarSpeed();
            
            Assert.AreEqual(100, (int)testData.CurrentSpeed);
            Assert.IsTrue(testData.AverageSpeedPerLap.ContainsKey(1));
        }

        [TestMethod]
        public void Update_Rank_And_Previous_Rank()
        {
            CarDetails testData = new CarDetails
            {
                Rank = 2,
                PreviousRank = 3
            };
            
            Assert.AreEqual(2, testData.Rank);
            Assert.AreEqual(3, testData.PreviousRank);
            
            testData.UpdateRank(1);
            
            
            Assert.AreEqual(1, testData.Rank);
            Assert.AreEqual(2, testData.PreviousRank);


        }
    }
}