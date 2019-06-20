
using System;

namespace DriverReports.Models
{
	public class Trip
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public double Distance { get; set; }

		public int CalculateAverageSpeed()
		{
			var elapsedTime = EndTime - StartTime;

			return (int)Math.Round(Distance / elapsedTime.TotalHours);
		}
	}
}