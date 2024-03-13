using Microser.Core.Models;

namespace Microser.BlazorAppClient.Services;

public interface IWeatherForecastApiService
{
    Task<WeatherForecast?> GetByIdAsync(int id);

    Task<WeatherForecast[]?> GetAllAsync();

    Task<WeatherForecast?> AddAsync(WeatherForecast weatherForecast);

    Task<bool> UpdateAsync(int id, WeatherForecast weatherForecast);

    Task<bool> DeleteByIdAsync(int id);
}