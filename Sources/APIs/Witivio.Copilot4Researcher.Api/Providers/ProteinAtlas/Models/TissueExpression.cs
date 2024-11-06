using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class TissueExpression
    {
        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; }

        [XmlAttribute(AttributeName = "technology")]
        public string Technology { get; set; }

        [XmlAttribute(AttributeName = "assayType")]
        public string AssayType { get; set; }

        [XmlElement(ElementName = "summary")]
        public Summary Summary { get; set; }

        [XmlElement(ElementName = "verification")]
        public Verification Verification { get; set; }

        [XmlElement("image")]
        public List<Image> Images { get; set; }

        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }
    }


}
