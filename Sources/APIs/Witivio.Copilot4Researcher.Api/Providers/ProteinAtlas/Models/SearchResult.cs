using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{

    public class SearchResult
    {
        [JsonPropertyName("Gene")]
        public string Gene { get; set; }

        [JsonPropertyName("Gene synonym")]
        public List<string> GeneSynonym { get; set; }

        [JsonPropertyName("Ensembl")]
        public string Ensembl { get; set; }

        [JsonPropertyName("Uniprot")]
        public List<string> Uniprot { get; set; }

        [JsonPropertyName("Tissue expression cluster")]
        public string TissueExpressionCluster { get; set; }
    }

    public class Xref
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "db")]
        public string Db { get; set; }
    }

    public class Image
    {
        [XmlAttribute(AttributeName = "imageType")]
        public string ImageType { get; set; }

        [XmlElement(ElementName = "tissue")]
        public Tissue Tissue { get; set; }

        [XmlElement(ElementName = "imageUrl")]
        public string ImageUrl { get; set; }
    }

    public class Tissue
    {
        [XmlAttribute(AttributeName = "organ")]
        public string Organ { get; set; }

        [XmlAttribute(AttributeName = "ontologyTerms")]
        public string OntologyTerms { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

    public class Data
    {
        [XmlElement(ElementName = "tissue")]
        public Tissue Tissue { get; set; }

        [XmlElement(ElementName = "level")]
        public Level Level { get; set; }

        [XmlElement(ElementName = "tissueCell")]
        public TissueCell TissueCell { get; set; }
    }

    public class Level
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Expression { get; set; }
    }

    public class TissueCell
    {
        [XmlElement(ElementName = "cellType")]
        public string CellType { get; set; }

        [XmlElement(ElementName = "level")]
        public Level Level { get; set; }
    }


}
