using System;
using System.IO;
using System.Linq;
using DriverReports.Commands;
using DriverReports.Data;
using DriverReports.Extensions;
using DriverReports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DriverReports
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var serviceProvider = ConfigureServices();

			var filePath = args?.FirstOrDefault();
			if (string.IsNullOrEmpty(filePath))
			{
				Console.Error.WriteLine("Please provide path to input file.");
				Console.Error.WriteLine("Usage: DriverReports.cs <path-to-file>");
				Environment.Exit(1);
			}

			try
			{
				using (var file = File.OpenText(filePath))
				{
					var commandService = serviceProvider.GetService<ICommandService>();

					string line;
					while ((line = file.ReadLine()) != null)
					{
						try
						{
							commandService.RunCommand(line);
						}
						catch (Exception e)
						{
							Console.Error.WriteLine($"An error occurred while running the following command: {line}. Skipping...");
							Console.Error.WriteLine(e.Message);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"An error occurred attempting to read file at: {filePath}");
				Console.Error.WriteLine(e.Message);
				Environment.Exit(1);
			}

			var reportGenerationService = serviceProvider.GetService<IReportGenerationService>();
			var report = reportGenerationService.GenerateReport();
			Console.Write(report.ToString());
			Environment.Exit(0);
		}

		private static ServiceProvider ConfigureServices()
		{
			return new ServiceCollection()
				.AddMultipleSingleton<ICommand>()
				.AddSingleton<IDriverRepository, DriverRepository>()
				.AddSingleton<ICommandService, CommandService>()
				.AddSingleton<IReportGenerationService, ReportGenerationService>()
				.BuildServiceProvider();
		}
	}
}
