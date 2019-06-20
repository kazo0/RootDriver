using System;
using System.Collections.Generic;
using System.Linq;
using DriverReports.Commands;

namespace DriverReports.Services
{
	public class CommandService : ICommandService
	{
		private readonly IEnumerable<ICommand> _commands;

		public CommandService(IEnumerable<ICommand> commands)
		{
			_commands = commands;
		}

		public void RunCommand(string command)
		{
			var commandParts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			var commandName = commandParts?.FirstOrDefault();
			var parameters = commandParts?.Skip(1).ToArray();

			var associatedCommand = _commands.FirstOrDefault(c => c.CanHandle(commandName));
			if (associatedCommand == null)
			{
				throw new InvalidOperationException($"Unknown command: {commandName}");
			}

			associatedCommand.Run(parameters);
		}
	}
}