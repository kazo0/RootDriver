using System;

public static class ValidationExtensions
{
	public static void ValidateNotNull<T>(this T value, string parameterName) where T : class
	{
		if (value == null)
		{
			throw new ArgumentNullException($"{parameterName} cannot be null");
		}
	}

	public static void ValidateString(this string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException($"{parameterName} must have a value");
		}
	}
}