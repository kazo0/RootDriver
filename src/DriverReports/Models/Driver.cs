using System.Collections.Generic;

namespace DriverReports.Models
{
	public class Driver
	{
		public string Name { get; set; }
		public List<Trip> Trips { get; set; } = new List<Trip>();

		public Driver(string name)
		{
			Name = name;
		}
	}
}