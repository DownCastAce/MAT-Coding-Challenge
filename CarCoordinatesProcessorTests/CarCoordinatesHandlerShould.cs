using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CarCoordinatesProcessor;
using CarCoordinatesProcessor.Models;
using CarCoordinatesProcessor.Mqtt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CarCoordinatesProcessorTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CarCoordinatesHandlerShould
    {
        private readonly Mock<Imqtt> _fakeMqttClient = new Mock<Imqtt>();
        
        private const string ExamplePayload = "{\"CarLocationData\":{\"timestamp\":1582061329767,\"carIndex\":1,\"location\":{\"lat\":52.06905479305401,\"long\":-1.0225002257078422}},\"PreviousCarCoordinates\":{\"timestamp\":1582061329566,\"carIndex\":1,\"location\":{\"lat\":52.068903120291246,\"long\":-1.0226858502212455}},\"DistancedTraveled\":4393.099131115184,\"CurrentSpeed\":235.08107800782315,\"Rank\":1,\"PreviousRank\":1,\"CarIndex\":1,\"LapNumber\":1,\"AverageSpeedPerLap\":{\"1\":220.27818243236658},\"PreviousLapTime\":1582061322767}";

        private const string FinishLapExamplePayload = "{\"CarLocationData\":{\"timestamp\":1582061329767,\"carIndex\":1,\"location\":{\"lat\":52.06905479305401,\"long\":-1.0225002257078422}},\"PreviousCarCoordinates\":{\"timestamp\":1582061329566,\"carIndex\":1,\"location\":{\"lat\":52.068903120291246,\"long\":-1.0226858502212455}},\"DistancedTraveled\":4393.099131115184,\"CurrentSpeed\":235.08107800782315,\"Rank\":1,\"PreviousRank\":1,\"CarIndex\":1,\"LapNumber\":2,\"AverageSpeedPerLap\":{\"1\":220.27818243236658},\"PreviousLapTime\":1582061329767}";

        private const string FirstCarAddedPayload = "{\"CarLocationData\":{\"timestamp\":1582061329767,\"carIndex\":1,\"location\":{\"lat\":52.06905479305401,\"long\":-1.0225002257078422}},\"PreviousCarCoordinates\":null,\"DistancedTraveled\":0.0,\"CurrentSpeed\":0.0,\"Rank\":-1,\"PreviousRank\":0,\"CarIndex\":1,\"LapNumber\":1,\"AverageSpeedPerLap\":{\"1\":0.0},\"PreviousLapTime\":1582061329767}";

	    private const string ExampleCarCoordinates = "{\"timestamp\":1582061329767,\"carIndex\":1,\"location\":{\"lat\":52.06905479305401,\"long\":-1.0225002257078422}}";
	    private const int CarIndex = 1;
	    private const int PositionTwo = 2;
	    private const int PositionOne = 1;

	    [TestInitialize]
        public void Setup()
        {
            _fakeMqttClient.Setup(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<object>()));
        }
        
		[TestMethod]
        public void Ensure_Car_Is_Successfully_Updated()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(ExamplePayload);
            Dictionary<int, CarDetails> allCars = new Dictionary<int, CarDetails>{{CarIndex, carDetails}};

            #region Fix data

            carDetails.CarLocationData = carDetails.PreviousCarCoordinates;//Reset the Current Car Coordinates to previous one to simulate past
            carDetails.DistancedTraveled -= 21.1232196676363;
            carDetails.AverageSpeedPerLap[1] = 205.47528685691001;

            #endregion

            CarCoordinatesHandler testEngine = new CarCoordinatesHandler(_fakeMqttClient.Object, allCars);
            
            testEngine.Process(ExampleCarCoordinates);

            Assert.AreEqual(ExampleCarCoordinates, allCars[CarIndex].CarLocationData.ToString());
        }

        [TestMethod]
        public void Ensure_Car_Is_Successfully_Added_To_Cache()
        {
            Dictionary<int, CarDetails> allCars = new Dictionary<int, CarDetails>();
            
            CarCoordinatesHandler testEngine = new CarCoordinatesHandler(_fakeMqttClient.Object, allCars);
            
            testEngine.Process(ExampleCarCoordinates);

            Assert.AreEqual(FirstCarAddedPayload, JsonConvert.SerializeObject(allCars[CarIndex]));
        }
        
        [TestMethod]
        public void Update_Lap_Once_Passing_Finishing_Line()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(ExamplePayload);
            Dictionary<int, CarDetails> allCars = new Dictionary<int, CarDetails>{{CarIndex, carDetails}};
            
            #region Fix data

            carDetails.CarLocationData = carDetails.PreviousCarCoordinates;//Reset the Current Car Coordinates to previous one to simulate past
            carDetails.DistancedTraveled -= 21.1232196676363;
            carDetails.AverageSpeedPerLap[1] = 205.47528685691001;
            carDetails.PreviousLapTime -= 80000;
            
            #endregion
            
            CarCoordinatesHandler testEngine = new CarCoordinatesHandler(_fakeMqttClient.Object, allCars);

            testEngine.Process(ExampleCarCoordinates);

            Assert.AreEqual(FinishLapExamplePayload, JsonConvert.SerializeObject(allCars[CarIndex]));
        }

        [TestMethod]
        public void Update_Rank()
        {
            CarDetails carDetails = JsonConvert.DeserializeObject<CarDetails>(ExamplePayload);
            Dictionary<int, CarDetails> allCars = new Dictionary<int, CarDetails>{{CarIndex, carDetails}};

            carDetails.Rank = PositionTwo;
            
            CarCoordinatesHandler testEngine = new CarCoordinatesHandler(_fakeMqttClient.Object, allCars);

            testEngine.Process(ExampleCarCoordinates);

            Assert.AreEqual(PositionTwo, allCars[CarIndex].PreviousRank);
            Assert.AreEqual(PositionOne, allCars[CarIndex].Rank);
        }
    }
}