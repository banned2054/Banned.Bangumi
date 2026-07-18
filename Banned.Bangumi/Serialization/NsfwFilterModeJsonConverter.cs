using Banned.Bangumi.Models.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Serialization;

internal sealed class NsfwFilterModeJsonConverter : JsonConverter<NsfwFilterMode>
{
    public override NsfwFilterMode Read(ref Utf8JsonReader    reader,
                                        Type                  typeToConvert,
                                        JsonSerializerOptions options)
    {
        if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
        {
            throw new JsonException("The NSFW filter must be a Boolean value.");
        }

        return reader.GetBoolean() ? NsfwFilterMode.Only : NsfwFilterMode.Exclude;
    }

    public override void Write(Utf8JsonWriter        writer,
                               NsfwFilterMode        value,
                               JsonSerializerOptions options)
    {
        switch (value)
        {
            case NsfwFilterMode.Exclude :
                writer.WriteBooleanValue(false);
                break;
            case NsfwFilterMode.Only :
                writer.WriteBooleanValue(true);
                break;
            default :
                throw new JsonException($"The NSFW filter mode '{value}' is invalid.");
        }
    }
}
