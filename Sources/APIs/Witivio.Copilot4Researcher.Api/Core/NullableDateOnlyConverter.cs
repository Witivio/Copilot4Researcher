namespace Witivio.Copilot4Researcher.Core
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class NullableDateOnlyConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            try
            {
                return DateOnly.Parse(reader.GetString()!);
            }
            catch (Exception)
            {
                // Return null or default value if parsing fails
                return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd")); // Customize format as needed
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }

}
