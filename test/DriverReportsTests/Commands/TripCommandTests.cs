using System;
using System.Collections.Generic;
using System.Linq;
using DriverReports.Commands;
using DriverReports.Models;
using Moq;
using Xunit;

namespace DriverReportsTests.Commands
{
	public class TripCommandTests : CommandTests<TripCommand>
	{
		public static IEnumerable<object[]> InvalidArguments => 
			new List<object[]>
			{
				new object[] { null },
				new object[] { string.Empty },
			};

		public static IEnumerable<object[]> InvalidTimes => 
			new List<object[]>
			{
				new object[] { "Seven Am" },
				new object[] { "7:00" },
				new object[] { "7.00" },
				new object[] { "7am" },
				new object[] { "7" },
			};

		protected override string CommandName => "TRIP";

		private Driver BobTheDriver = new Driver("Bob");

		protected override TripCommand SetupCommand()
		{
			return new TripCommand(MockRepository.Object);
		}

		[Theory]
		[InlineData("Trip")]
		[InlineData("trip")]
		[InlineData("TrIp")]
		public void CanHandleShouldIgnoreCasing(string commandName)
		{
			var canHandle = Command.CanHandle(commandName);
			Assert.True(canHandle);
		}

		[Fact]
		public void RunShouldAddTripToDriver()
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);
			
			Command.Run("Bob", "07:00", "07:20", "22");

			MockRepository.Verify(repo => repo.GetDriver(It.Is<string>(x => x.Equals("Bob"))));
			Assert.NotEmpty(BobTheDriver.Trips);
			
			var trip = BobTheDriver.Trips.First();
			Assert.Equal(22d, trip.Distance);
			Assert.Equal(7, trip.StartTime.Hour);
			Assert.Equal(0, trip.StartTime.Minute);
			Assert.Equal(7, trip.EndTime.Hour);
			Assert.Equal(20, trip.EndTime.Minute);
		}

		[Fact]
		public void RunShouldThrowIfWrongNumberOfArguments()
		{
			Assert.Throws<ArgumentException>(() => Command.Run("Bob"));
		}

		[Fact]
		public void RunShouldThrowIfDriverDoesNotExist()
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(default(Driver));

			var exception = Assert.Throws<InvalidOperationException>(() => Command.Run("Bob", "07:00", "07:20", "22"));
			Assert.Equal(@"No driver with name 'Bob' exists", exception.Message);
		}

		
		[Theory]
		[MemberData(nameof(InvalidTimes), MemberType = typeof(TripCommandTests))]
		public void RunShouldThrowIfStartTimeIsInvalid(string startTime)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			var exception = Assert.Throws<ArgumentException>(() => Command.Run("Bob", startTime, "07:20", "22"));
			Assert.Equal($"Timestamp parameter is not valid: {startTime}", exception.Message);
		}

		[Theory]
		[MemberData(nameof(InvalidTimes), MemberType = typeof(TripCommandTests))]
		public void RunShouldThrowIfEndTimeIsInvalid(string endTime)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			var exception = Assert.Throws<ArgumentException>(() => Command.Run("Bob", "06:50", endTime, "22"));
			Assert.Equal($"Timestamp parameter is not valid: {endTime}", exception.Message);
		}

		
		[Theory]
		[InlineData("70 miles")]
		[InlineData("seventy")]
		public void RunShouldThrowIfDistanceIsInvalid(string distance)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			var exception = Assert.Throws<ArgumentException>(() => Command.Run("Bob", "06:50", "07:20", distance));
			Assert.Equal($"Distance parameter is not valid: {distance}", exception.Message);
		}

		[Theory]
		[MemberData(nameof(InvalidArguments), MemberType = typeof(TripCommandTests))]
		public void RunShouldFailDriverValidation(string driverName)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);
				
			Assert.Throws<ArgumentException>(() => Command.Run(driverName, "07:00", "07:20", "22"));
		}

		[Theory]
		[MemberData(nameof(InvalidArguments), MemberType = typeof(TripCommandTests))]
		public void RunShouldFailStartTimeValidation(string startTime)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			Assert.Throws<ArgumentException>(() => Command.Run("Bob", startTime, "07:20", "22"));
		}

		[Theory]
		[MemberData(nameof(InvalidArguments), MemberType = typeof(TripCommandTests))]
		public void RunShouldFailEndTimeValidation(string endTime)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			Assert.Throws<ArgumentException>(() => Command.Run("Bob", "07:00", endTime, "22"));
		}

		[Theory]
		[MemberData(nameof(InvalidArguments), MemberType = typeof(TripCommandTests))]
		public void RunShouldFailDistanceValidation(string distance)
		{
			MockRepository.Setup(repo => repo.GetDriver(It.IsAny<string>()))
				.Returns(BobTheDriver);

			Assert.Throws<ArgumentException>(() => Command.Run("Bob", "07:00", "07:20", distance));
		}
	}
}