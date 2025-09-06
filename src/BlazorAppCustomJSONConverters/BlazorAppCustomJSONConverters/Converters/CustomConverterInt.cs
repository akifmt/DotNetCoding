using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAppCustomJSONConverters.Converters;

public class CustomConverterInt : JsonConverter<int>
{
    private readonly static JsonConverter<int> s_defaultConverter = (JsonConverter<int>)JsonSerializerOptions.Default.GetConverter(typeof(int));

    // Custom serialization logic
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }

    // Custom deserialization logic
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return 0;
            case JsonTokenType.Number:
                return reader.GetInt32();
            default:
                return s_defaultConverter.Read(ref reader, typeToConvert, options); // Fall back to default deserialization logic
        }
    }
}
