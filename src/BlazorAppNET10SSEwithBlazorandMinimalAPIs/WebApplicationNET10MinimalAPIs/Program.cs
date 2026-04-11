
using System.Runtime.CompilerServices;

namespace WebApplicationNET10MinimalAPIs;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var AllowAnyOrigins = "_AllowAnyOrigins"; // TEST
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowAnyOrigins, policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            return forecast;
        }).WithName("GetWeatherForecast");


        app.MapGet("/sse-json-weatherforecast", (CancellationToken cancellationToken) =>
        {
            async IAsyncEnumerable<WeatherForecastDTO> GetWeatherForecasts([EnumeratorCancellation] CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecastRecord
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                    var heartRate = Random.Shared.Next(60, 100);

                    yield return WeatherForecastDTO.Create(heartRate, forecast);
                    await Task.Delay(2000, cancellationToken);
                }
            }

            return TypedResults.ServerSentEvents(GetWeatherForecasts(cancellationToken), eventType: "weatherforecasts");
        });


        app.UseCors(AllowAnyOrigins);

        app.Run();
    }
}
