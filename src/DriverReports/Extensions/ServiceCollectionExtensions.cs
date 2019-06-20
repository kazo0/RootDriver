using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DriverReports.Extensions
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds multiple singleton services of the type specified in <typeparamref name="TService"/> with all implementations within the containing Assembly
		/// to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
		/// </summary>
		/// <typeparam name="TService">The interface</typeparam>
		public static IServiceCollection AddMultipleSingleton<TService>(this IServiceCollection serviceCollection) where TService : class
		{
			var assembly = typeof(TService).Assembly;
			var classes = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(tt => tt == typeof(TService)));
			foreach (Type @class in classes)
			{
				var binding = serviceCollection.AddSingleton(typeof(TService), @class);
			}

			return serviceCollection;
		}
	}
}