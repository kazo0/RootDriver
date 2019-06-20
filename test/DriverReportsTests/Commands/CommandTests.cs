using System;
using System.Collections.Generic;
using DriverReports.Commands;
using DriverReports.Data;
using Moq;
using Xunit;

namespace DriverReportsTests.Commands
{
	public abstract class CommandTests<TCommand> where TCommand : class, ICommand
	{
		protected abstract string CommandName { get; }

		protected abstract TCommand SetupCommand();
		
		protected TCommand Command { get; }

		protected Mock<IDriverRepository> MockRepository { get; }

		public CommandTests()
		{
			MockRepository = new Mock<IDriverRepository>();
			Command = SetupCommand();
		}

		[Fact]
		public void CanHandleShouldReturnTrueForCommandName()
		{
			var canHandle = Command.CanHandle(CommandName);

			Assert.True(canHandle);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void CanHandleShouldReturnFalse(string commandName)
		{
			var canHandle = Command.CanHandle(commandName);

			Assert.False(canHandle);
		}

		[Fact]
		public void RunShouldThrowIfNullArguments()
		{
			Assert.Throws<ArgumentNullException>(() => Command.Run(null));
		}
	}
}