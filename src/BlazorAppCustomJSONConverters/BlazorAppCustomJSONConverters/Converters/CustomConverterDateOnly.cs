using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAppCustomJSONConverters.Converters;

public class CustomConverterDateOnly : JsonConverter<DateOnly>
{
    private readonly static JsonConverter<DateOnly> s_defaultConverter = (JsonConverter<DateOnly>)JsonSerializerOptions.Default.GetConverter(typeof(DateOnly));

    // Custom serialization logic
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        string customDateFormat = value.ToString("dd--MM--yyyy"); // 01--01--2000
        writer.WriteStringValue(customDateFormat);
    }

    // Custom deserialization logic
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                DateTime parsedDate = DateTime.MinValue;
                string pattern = "dd--MM--yyyy";
                DateTime.TryParseExact(reader.GetString(), pattern, null, DateTimeStyles.None, out parsedDate);
                return DateOnly.FromDateTime(parsedDate);
            default:
                return s_defaultConverter.Read(ref reader, typeToConvert, options); // Fall back to default deserialization logic
        }
    }
}
