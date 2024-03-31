using Microser.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microser.API.Weather.Controllers;

[Authorize(Policy = "ClientIdPolicy")]
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecasts")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        return Ok(Data.WeatherForecasts);
    }

    [HttpGet("{id}", Name = "GetWeatherForecast")]
    public ActionResult<WeatherForecast> Get(int id)
    {
        var item = Data.WeatherForecasts.FirstOrDefault(x => x.Id == id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost(Name = "PostWeatherForecast")]
    public ActionResult<WeatherForecast> Post(WeatherForecast weatherForecast)
    {
        var last = Data.WeatherForecasts.LastOrDefault();
        if (last == null)
            weatherForecast.Id = 1;
        else
            weatherForecast.Id = last.Id + 1;

        Data.WeatherForecasts.Add(weatherForecast);

        return new CreatedAtRouteResult("GetWeatherForecast", new { id = weatherForecast.Id }, weatherForecast);
    }

    [HttpPut("{id}", Name = "PutWeatherForecast")]
    public ActionResult<WeatherForecast> Put(int id, WeatherForecast weatherForecast)
    {
        var item = Data.WeatherForecasts.FirstOrDefault(x => x.Id == id);
        if (item == null)
            return NotFound();

        item.Date = weatherForecast.Date;
        item.TemperatureC = weatherForecast.TemperatureC;
        item.Summary = weatherForecast.Summary;

        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteWeatherForecast")]
    public ActionResult<WeatherForecast> Delete(int id)
    {
        var item = Data.WeatherForecasts.FirstOrDefault(x => x.Id == id);
        if (item == null)
            return NotFound();

        Data.WeatherForecasts.Remove(item);

        return Ok();
    }
}