using System;
using System.Collections.Generic;
using DriverReports.Commands;
using DriverReports.Models;
using Moq;
using Xunit;

namespace DriverReportsTests.Commands
{
	public class DriverCommandTests : CommandTests<DriverCommand>
	{
		protected override string CommandName => "DRIVER";

		protected override DriverCommand SetupCommand()
		{
			return new DriverCommand(MockRepository.Object);
		}

		[Fact]
		public void RunShouldAddDriver()
		{
			Command.Run("Bob");

			MockRepository.Verify(repo => repo.AddDriver(It.Is<Driver>(d => d.Name == "Bob")));
		}

		[Theory]
		[InlineData("Driver")]
		[InlineData("driver")]
		[InlineData("DrIVeR")]
		public void CanHandleShouldIgnoreCasing(string commandName)
		{
			var canHandle = Command.CanHandle(commandName);
			Assert.True(canHandle);
		}

		[Fact]
		public void RunShouldThrowIfWrongNumberOfArguments()
		{
			Assert.Throws<ArgumentException>(() => Command.Run("Bob", "Bob2"));
		}
				
		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void RunShouldThrowIfDriverNameIsInvalid(string driverName)
		{
			Assert.Throws<ArgumentException>(() => Command.Run(new [] { driverName }));
		}
	}
}