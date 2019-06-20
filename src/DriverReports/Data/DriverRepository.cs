using System;
using System.Collections.Generic;
using DriverReports.Models;

namespace DriverReports.Data
{
	public class DriverRepository : IDriverRepository
	{
		private readonly Dictionary<string, Driver> _drivers = new Dictionary<string, Driver>(StringComparer.OrdinalIgnoreCase);

		public Driver GetDriver(string driverName)
		{
			driverName.ValidateString(nameof(driverName));

			return _drivers.TryGetValue(driverName, out var driver) ? driver : null;
		}

		public bool AddDriver(Driver driver)
		{
			driver.ValidateNotNull(nameof(driver));

			return _drivers.TryAdd(driver.Name, driver);
		}

		public bool HasDriver(string driverName)
		{
			return _drivers.ContainsKey(driverName);
		}

		public IEnumerable<Driver> GetDrivers()
		{
			foreach (var entry in _drivers)
			{
				yield return entry.Value;
			}
		}
	}
}