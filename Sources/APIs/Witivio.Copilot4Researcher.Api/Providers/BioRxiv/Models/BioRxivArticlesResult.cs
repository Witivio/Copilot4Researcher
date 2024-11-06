using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Providers.BioRxiv.Models
{


    public class BioRxivArticlesResult
    {
        [JsonPropertyName("collection")]
        public List<BioRxivArticleDetail> ArticleDetails { get; set; }
    }


}
