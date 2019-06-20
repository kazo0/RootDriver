namespace DriverReports.Commands
{
	public interface ICommand
	{
		void Run(params string[] args);
		bool CanHandle(string commandName);
	}
}