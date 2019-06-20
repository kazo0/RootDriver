using System;

namespace DriverReports.Commands
{
	public abstract class Command : ICommand
	{
		public abstract string CommandName { get; }

		public abstract void Run(params string[] args);

		public virtual bool CanHandle(string commandName)
		{
			return !string.IsNullOrEmpty(commandName) && CommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase);
		}

		protected void ValidateArguments(int expectedCount, params string[] args)
		{
			args.ValidateNotNull(nameof(args));
			if (args.Length != expectedCount)
			{
				throw new ArgumentException($"Invalid argument count. Expected: {expectedCount} Actual: {args.Length} Arguments: {args}");
			}
		}
	}
}