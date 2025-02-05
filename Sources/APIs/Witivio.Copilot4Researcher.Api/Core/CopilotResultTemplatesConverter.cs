using System.Text.Json;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Core
{
    public class CopilotResultTemplatesConverter : JsonConverter<CopilotResultTemplates>
    {
        public override CopilotResultTemplates Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // For simplicity, assume we are not implementing deserialization
            throw new NotImplementedException("Deserialization is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, CopilotResultTemplates value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Add the dynamic card content
            foreach (var entry in value.DynamicContent)
            {
                //writer.WriteString(entry.Key, entry.Value);
                writer.WritePropertyName(entry.Key);
                entry.Value.WriteTo(writer);
            }

            writer.WriteEndObject();
        }
    }

}
