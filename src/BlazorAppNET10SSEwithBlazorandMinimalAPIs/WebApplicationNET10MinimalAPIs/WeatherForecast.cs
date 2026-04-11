namespace WebApplicationNET10MinimalAPIs;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

internal record WeatherForecastRecord(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record WeatherForecastDTO(DateTime Timestamp, int HeartRate, WeatherForecastRecord[] WeatherForecasts)
{
    public static WeatherForecastDTO Create(int heartRate, WeatherForecastRecord[] weatherForecasts) => new(DateTime.UtcNow, heartRate, weatherForecasts);
}
