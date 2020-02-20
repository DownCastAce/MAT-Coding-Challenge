using CarCoordinatesProcessor.Logic;
using CarCoordinatesProcessor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarCoordinatesProcessorTests.Logic
{
	[TestClass]
	public class CarLocationProcessorShould
	{

		private static readonly Location StartLine = new Location { Latitude = 52.069342, Longitude = -1.022140 };

		[TestMethod]
		public void Be_True_When_Car_Finishes_Lap()
		{
			CarDetails carDetails = new CarDetails
			{
				CarLocationData = new CarCoordinates
				{
					Location = StartLine,
					TimeStamp = 500000
				},
				PreviousLapTime = 100000
			};

			Assert.IsTrue(CarLocationProcessor.FinishedLap(carDetails));
		}


		[TestMethod]
		public void Be_False_When_Car_Finishes_Lap_Second_Recording()
		{
			CarDetails carDetails = new CarDetails
			{
				CarLocationData = new CarCoordinates
				{
					Location = StartLine,
					TimeStamp = 2000
				},
				PreviousLapTime = 1000
			};

			Assert.IsFalse(CarLocationProcessor.FinishedLap(carDetails));
		}
	}
}
