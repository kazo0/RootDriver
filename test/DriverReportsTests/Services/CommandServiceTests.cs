using System;
using System.Collections.Generic;
using System.Linq;
using DriverReports.Commands;
using DriverReports.Data;
using DriverReports.Services;
using Moq;
using Xunit;

namespace DriverReportsTests.Services
{
	public class CommandServiceTests
	{
		private readonly Mock<ICommand> _mockCommand1;
		private readonly Mock<ICommand> _mockCommand2;
		private readonly ICommandService _commandService;

		public CommandServiceTests()
		{
			_mockCommand1 = new Mock<ICommand>();
			_mockCommand2 = new Mock<ICommand>();
			_commandService = new CommandService(new List<ICommand> { _mockCommand1.Object, _mockCommand2.Object });
		}

		[Fact]
		public void RunCommandShouldCallRunOnAssociatedCommand()
		{
			_mockCommand1.Setup(x => x.CanHandle(It.IsAny<string>()))
				.Returns(true);

			_mockCommand2.Setup(x => x.CanHandle(It.IsAny<string>()))
				.Returns(false);

			_commandService.RunCommand("Driver Bob");

			_mockCommand1.Verify(cmd => cmd.CanHandle(It.Is<string>(x => x == "Driver")), Times.Once);
			_mockCommand1.Verify(cmd => cmd.Run(It.Is<string[]>(x => x.Length == 1 && x[0] == "Bob")), Times.Once);
			_mockCommand2.Verify(cmd => cmd.Run(It.Is<string[]>(x => x.Length == 1 && x[0] == "Bob")), Times.Never);
		}

		[Fact]
		public void RunCommandShouldThrowIfNoCommandsCanHandle()
		{
			_mockCommand1.Setup(x => x.CanHandle(It.IsAny<string>()))
				.Returns(false);

			_mockCommand2.Setup(x => x.CanHandle(It.IsAny<string>()))
				.Returns(false);

			var exception = Assert.Throws<InvalidOperationException>(() => _commandService.RunCommand("Driver Bob"));
			Assert.Equal("Unknown command: Driver", exception.Message);

			_mockCommand1.Verify(cmd => cmd.CanHandle(It.Is<string>(x => x == "Driver")), Times.Once);
			_mockCommand2.Verify(cmd => cmd.CanHandle(It.Is<string>(x => x == "Driver")), Times.Once);

			_mockCommand1.Verify(cmd => cmd.Run(It.Is<string[]>(x => x.Length == 1 && x[0] == "Bob")), Times.Never);
			_mockCommand2.Verify(cmd => cmd.Run(It.Is<string[]>(x => x.Length == 1 && x[0] == "Bob")), Times.Never);
		}
	}
}