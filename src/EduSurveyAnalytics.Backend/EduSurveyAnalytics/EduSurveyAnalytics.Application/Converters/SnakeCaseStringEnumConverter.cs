using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EduSurveyAnalytics.Application.Converters;

// Enum value converter to snake_case_string
// Used for converting int value in response to snake_case_string (example: errorCode not 0, but username_not_found)
// Injecting in Program.cs in AddController().AddJsonOptions...
public class SnakeCaseStringEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<TEnum>(reader.GetString(), ignoreCase: true);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var snakeCaseValue = ToSnakeCase(value.ToString());
        writer.WriteStringValue(snakeCaseValue);
    }

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var result = new StringBuilder();
        for (var i = 0; i < value.Length; i++)
        {
            if (i > 0 && char.IsUpper(value[i]))
            {
                result.Append('_');
            }
            result.Append(char.ToLower(value[i]));
        }
        return result.ToString();
    }
}