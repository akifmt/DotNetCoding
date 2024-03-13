using Microser.Core.Models;

namespace Microser.API.Weather;

public class Data
{
    public static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static List<WeatherForecast>? weatherForecasts;

    public static List<WeatherForecast> WeatherForecasts
    {
        get
        {
            if (weatherForecasts == null)
                weatherForecasts = GetWeatherForecasts();
            return weatherForecasts;
        }
    }

    private static List<WeatherForecast> GetWeatherForecasts()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Id = index,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();
    }
}