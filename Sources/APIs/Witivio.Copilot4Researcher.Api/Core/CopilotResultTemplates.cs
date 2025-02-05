using AdaptiveCards;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Core
{
    [JsonConverter(typeof(CopilotResultTemplatesConverter))]
    public class CopilotResultTemplates
    {
        public Dictionary<string, JsonElement> DynamicContent { get; } = new Dictionary<string, JsonElement>();

        public void AddCard(string key, AdaptiveCard card)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(card.ToJson());
            DynamicContent[key] = jsonDocument.RootElement;
        }
    }

}
