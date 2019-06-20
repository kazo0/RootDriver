using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriverReports.Models
{
	public class Report
	{
		public List<DriverReport> DriverReports { get; set; } = new List<DriverReport>();

		public override string ToString()
		{
			if (!DriverReports.Any()) 
			{
				return "No drivers to report";
			}

			var builder = new StringBuilder();
			foreach (var report in DriverReports.OrderByDescending(dr => dr.TotalDistance))
			{
				builder.AppendLine(report.ToString());
			}

			return builder.ToString();
		}
	}
}