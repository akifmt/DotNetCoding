using BlazorAppCustomJSONConverters.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAppCustomJSONConverters.Converters;

public class CustomConverterWeatherForecast : JsonConverter<WeatherForecast>
{
    private readonly static JsonConverter<WeatherForecast> s_defaultConverter = (JsonConverter<WeatherForecast>)JsonSerializerOptions.Default.GetConverter(typeof(WeatherForecast));

    // Custom serialization logic
    public override void Write(Utf8JsonWriter writer, WeatherForecast value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("Date", value.Date.ToString("yyyy-MM-dd"));
        writer.WriteNumber("TemperatureC", value.TemperatureC);
        writer.WriteString("Summary", value.Summary);

        writer.WriteEndObject();
    }

    // Custom deserialization logic
    public override WeatherForecast Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        WeatherForecast weatherForecast = new();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return weatherForecast;
            }

            string? propertyName = reader.GetString();
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "Date":
                        string? date = reader.GetString();
                        weatherForecast.Date = DateOnly.Parse(date);
                        break;
                    case "TemperatureC":
                        int temperatureC = reader.GetInt32();
                        weatherForecast.TemperatureC = temperatureC;
                        break;
                    case "Summary":
                        string? summary = reader.GetString();
                        weatherForecast.Summary = summary;
                        break;
                }
            }
        }

        throw new JsonException();
    }
}
