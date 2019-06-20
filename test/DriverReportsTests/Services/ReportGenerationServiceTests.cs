using System;
using System.Collections.Generic;
using DriverReports.Data;
using DriverReports.Models;
using DriverReports.Services;
using Moq;
using Xunit;

namespace DriverReportsTests.Services
{
	public class ReportGenerationServiceTests
	{
		private readonly Mock<IDriverRepository> _mockRepository;
		private readonly IReportGenerationService _reportGenerationService;

		public ReportGenerationServiceTests()
		{
			_mockRepository = new Mock<IDriverRepository>();
			_reportGenerationService = new ReportGenerationService(_mockRepository.Object);
		}

		[Fact]
		public void GenerateReportShouldCreateAReportPerDriver()
		{
			var bob = new Driver("Bob");
			var john = new Driver("John");

			_mockRepository.Setup(repo => repo.GetDrivers())
				.Returns(new List<Driver> { bob, john });
			
			var report = _reportGenerationService.GenerateReport();

			_mockRepository.Verify(repo => repo.GetDrivers(), Times.Once);

			Assert.NotNull(report.DriverReports);
			Assert.Collection(report.DriverReports, 
				item => Assert.Equal("Bob", item.DriverName),
				item => Assert.Equal("John", item.DriverName));
		}

		[Fact]
		public void GenerateReportShouldNotIncludeTripSpeedsBelow5Mph()
		{
			var bob = new Driver("Bob");
			bob.Trips.Add(new Trip
			{
				Distance = 4,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1),
			});

			_mockRepository.Setup(repo => repo.GetDrivers())
				.Returns(new List<Driver> { bob });

			var report = _reportGenerationService.GenerateReport();

			_mockRepository.Verify(repo => repo.GetDrivers(), Times.Once);
			Assert.NotNull(report.DriverReports);
			Assert.Single(report.DriverReports);
			Assert.Equal(0, report.DriverReports[0].TotalDistance);
			Assert.Equal("Bob: 0 miles", report.DriverReports[0].ToString());
		}

		[Fact]
		public void GenerateReportShouldNotIncludeTripSpeedsAbove100Mph()
		{
			var bob = new Driver("Bob");
			bob.Trips.Add(new Trip
			{
				Distance = 150,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1),
			});

			_mockRepository.Setup(repo => repo.GetDrivers())
				.Returns(new List<Driver> { bob });

			var report = _reportGenerationService.GenerateReport();

			_mockRepository.Verify(repo => repo.GetDrivers(), Times.Once);
			Assert.NotNull(report.DriverReports);
			Assert.Single(report.DriverReports);
			Assert.Equal(0, report.DriverReports[0].TotalDistance);
			Assert.Equal("Bob: 0 miles", report.DriverReports[0].ToString());
		}

		[Fact]
		public void GenerateReportShouldCalculateAverageSpeed()
		{
			var bob = new Driver("Bob");
			bob.Trips.Add(new Trip
			{
				Distance = 50,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1),
			});
			bob.Trips.Add(new Trip
			{
				Distance = 50,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1),
			});


			_mockRepository.Setup(repo => repo.GetDrivers())
				.Returns(new List<Driver> { bob });

			var report = _reportGenerationService.GenerateReport();

			_mockRepository.Verify(repo => repo.GetDrivers(), Times.Once);
			Assert.NotNull(report.DriverReports);
			Assert.Single(report.DriverReports);
			Assert.Equal(100, report.DriverReports[0].TotalDistance);
			Assert.Equal(2, (int)report.DriverReports[0].TotalTime.TotalHours);
			Assert.Equal("Bob: 100 miles @ 50 mph", report.DriverReports[0].ToString());
		}
	}
}