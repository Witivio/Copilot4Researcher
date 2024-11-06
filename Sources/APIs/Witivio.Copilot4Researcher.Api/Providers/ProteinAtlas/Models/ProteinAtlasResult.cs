using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
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
