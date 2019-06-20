using DriverReports.Models;

namespace DriverReports.Services
{
	public interface IReportGenerationService
	{
		Report GenerateReport();
	}
}