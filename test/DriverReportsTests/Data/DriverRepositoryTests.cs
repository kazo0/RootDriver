using System;
using System.Collections.Generic;
using System.Linq;
using DriverReports.Commands;
using DriverReports.Data;
using DriverReports.Models;
using DriverReports.Services;
using Moq;
using Xunit;

namespace DriverReportsTests.Data
{
	public class DriverRepositoryTests
	{
		private readonly IDriverRepository _driverRepository;

		public DriverRepositoryTests()
		{
			_driverRepository = new DriverRepository();
		}

		[Fact]
		public void GetDriverShouldReturnNullForMissingDriverName()
		{
			var driver = _driverRepository.GetDriver("Bob");
			Assert.Null(driver);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void GetDriverShouldThrowForInvalidDriverName(string driverName)
		{
			Assert.Throws<ArgumentException>(() => _driverRepository.GetDriver(driverName));
		}

		[Fact]
		public void AddDriverShouldAddValidDriver()
		{
			var driver = new Driver("Bob");
			_driverRepository.AddDriver(driver);

			var verifyDriver = _driverRepository.GetDriver("Bob");
			Assert.NotNull(verifyDriver);
		}

		[Fact]
		public void AddDriverShouldNotAddDriverWithSameNameCaseInsensitive()
		{
			var driver = new Driver("Bob");
			var driver2 = new Driver("bob");
			_driverRepository.AddDriver(driver);
			_driverRepository.AddDriver(driver2);

			Assert.Single(_driverRepository.GetDrivers());
		}

		[Fact]
		public void HasDriverShouldReturnTrue()
		{
			var driver = new Driver("Bob");
			_driverRepository.AddDriver(driver);

			Assert.True(_driverRepository.HasDriver("Bob"));
			Assert.True(_driverRepository.HasDriver("bob"));
			Assert.True(_driverRepository.HasDriver("BOb"));
		}

		[Fact]
		public void HasDriverShouldReturnFalse()
		{
			var driver = new Driver("John");
			_driverRepository.AddDriver(driver);

			Assert.False(_driverRepository.HasDriver("Bob"));
		}

		[Fact]
		public void AddDriverShouldThrowForNullDriver()
		{
			Assert.Throws<ArgumentNullException>(() => _driverRepository.AddDriver(null));
		}
	}
}