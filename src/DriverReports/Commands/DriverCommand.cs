using DriverReports.Data;
using DriverReports.Models;

namespace DriverReports.Commands
{
	public class DriverCommand : Command
	{
		public override string CommandName => DRIVER_COMMAND;
		
		private const string DRIVER_COMMAND = "DRIVER";
		private const int ARG_COUNT = 1;
		private const int DRIVER_IDX = 0;
		private readonly IDriverRepository _driverRepository;

		public DriverCommand(IDriverRepository driverRepository)
		{
			_driverRepository = driverRepository;
		}

		public override void Run(params string[] args)
		{
			ValidateArguments(ARG_COUNT, args);

			var driverName = args[DRIVER_IDX];
			driverName.ValidateString(nameof(driverName));

			_driverRepository.AddDriver(new Driver(driverName));
		}
	}
}
