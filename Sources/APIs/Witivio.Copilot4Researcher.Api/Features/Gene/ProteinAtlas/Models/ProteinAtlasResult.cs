using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas.Models
{
    [XmlRoot(ElementName = "proteinAtlas")]
    public class ProteinAtlasResult
    {
        [XmlAttribute(AttributeName = "schemaVersion")]
        public string SchemaVersion { get; set; }

        [XmlElement(ElementName = "entry")]
        public Entry Entry { get; set; }
    }


}
