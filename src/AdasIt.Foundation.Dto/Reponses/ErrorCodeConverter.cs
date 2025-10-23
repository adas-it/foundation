using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdasIt.Foundation.Dto.Reponses;

/// <summary>
/// Custom JSON converter for serializing and deserializing <see cref="ApplicationErrorCode"/> instances.
/// </summary>
/// <remarks>
/// This converter supports deserialization from both JSON objects with a "value" property and direct numeric values.
/// During serialization, the error code is written as a simple numeric value.
/// </remarks>
public class ErrorCodeConverter : JsonConverter<ApplicationErrorCode>
{
    /// <summary>
    /// Reads and converts JSON to an <see cref="ApplicationErrorCode"/> instance.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>An <see cref="ApplicationErrorCode"/> instance representing the deserialized value.</returns>
    /// <exception cref="JsonException">
    /// Thrown when the JSON token is neither a JSON object with a "value" property nor a numeric value.
    /// </exception>
    public override ApplicationErrorCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);

            if (doc.RootElement.TryGetProperty("value", out JsonElement element))
            {
                element.TryGetInt32(out int value);

                return ApplicationErrorCode.New(value);
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return ApplicationErrorCode.New(reader.GetInt32());
        }

        throw new JsonException();
    }

    /// <summary>
    /// Writes an <see cref="ApplicationErrorCode"/> instance as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="ApplicationErrorCode"/> value to convert.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <remarks>
    /// The error code is serialized as a numeric value using the <see cref="ApplicationErrorCode.Value"/> property.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, ApplicationErrorCode value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
