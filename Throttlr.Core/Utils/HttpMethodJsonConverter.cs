using System.Text.Json;
using System.Text.Json.Serialization;

namespace Throttlr.Core.Utils;
public class HttpMethodJsonConverter : JsonConverter<HttpMethod>
{
    public override HttpMethod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? methodString = reader.GetString();
        return methodString is null ? throw new JsonException("HttpMethod value is null") : new HttpMethod(methodString);
    }

    public override void Write(Utf8JsonWriter writer, HttpMethod value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Method);
    }
}