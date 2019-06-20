using System;
using System.Text;

namespace DriverReports.Models
{
	public class DriverReport
	{
		public string DriverName { get; set; }
		public int TotalDistance { get; set; }
		public TimeSpan TotalTime { get; set; }

		public DriverReport(string driverName)
		{
			DriverName = driverName;
			TotalDistance = 0;
			TotalTime = TimeSpan.Zero;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append($"{DriverName}: {TotalDistance} miles");

			var totalHours = TotalTime.TotalHours;
			if (TotalDistance != 0 && totalHours != 0)
			{
				var averageTotalSpeed = (int)Math.Round(TotalDistance / totalHours);
				builder.Append($" @ {averageTotalSpeed} mph");
			}

			return builder.ToString();
		}
	}
}