using System;
using System.Globalization;
using DriverReports.Data;
using DriverReports.Models;

namespace DriverReports.Commands
{
	public class TripCommand : Command
	{
		private const string TRIP_COMMAND = "TRIP";
		private const int DRIVER_IDX = 0;
		private const int START_TIME_IDX = 1;
		private const int END_TIME_IDX = 2;
		private const int DISTANCE_IDX = 3;
		private const int ARG_COUNT = 4;

		public override string CommandName => TRIP_COMMAND;
		private readonly IDriverRepository _driverRepository;

		public TripCommand(IDriverRepository driverRepository)
		{
			_driverRepository = driverRepository;
		}

		public override void Run(params string[] args)
		{
			ValidateArguments(ARG_COUNT, args);

			var driver = GetDriver(args[DRIVER_IDX]);
			var startTime = ParseTime(args[START_TIME_IDX]);
			var endTime = ParseTime(args[END_TIME_IDX]);
			var distance = GetDistance(args[DISTANCE_IDX]);

			driver.Trips.Add(new Trip
			{
				StartTime = startTime,
				EndTime = endTime,
				Distance = distance,
			});
		}

		private Driver GetDriver(string driverName)
		{
			driverName.ValidateString(nameof(driverName));

			var driver = _driverRepository.GetDriver(driverName);
			if (driver == null)
			{
				throw new InvalidOperationException($"No driver with name '{driverName}' exists");
			}

			return driver;
		}

		private static double GetDistance(string distance)
		{
			distance.ValidateString(nameof(distance));
			if (!double.TryParse(distance, out var value))
			{
				throw new ArgumentException($"Distance parameter is not valid: {distance}");
			}

			return value;
		}

		private static DateTime ParseTime(string time)
		{
			time.ValidateString(nameof(time));
			if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
			{
				throw new ArgumentException($"Timestamp parameter is not valid: {time}");
			}
			return dateTime;
		}
	}
}
