using System.Collections.Generic;
using DriverReports.Models;

namespace DriverReports.Data
{
	public interface IDriverRepository
	{
		Driver GetDriver(string driverName);
		bool AddDriver(Driver driver);
		bool HasDriver(string driverName);
		IEnumerable<Driver> GetDrivers();
	}
}