using System;
using System.Linq;
using DriverReports.Data;
using DriverReports.Models;

namespace DriverReports.Services
{
	public class ReportGenerationService : IReportGenerationService
	{
		private readonly IDriverRepository _driverRepository;

		public ReportGenerationService(IDriverRepository driverRepository)
		{
			_driverRepository = driverRepository;
		}

		public Report GenerateReport()
		{
			var report = new Report();
			foreach (var driver in _driverRepository.GetDrivers())
			{
				var driverReport = new DriverReport(driver.Name);

				foreach (var trip in driver.Trips.Where(t => IsValidTrip(t)))
				{
					driverReport.TotalDistance += (int)Math.Round(trip.Distance);
					driverReport.TotalTime += (trip.EndTime - trip.StartTime);
				}
				report.DriverReports.Add(driverReport);
			}

			return report;
		}

		private static bool IsValidTrip(Trip trip)
		{
			var averageSpeed = trip.CalculateAverageSpeed();
			return averageSpeed > 5 && averageSpeed < 100;
		}
	}
}