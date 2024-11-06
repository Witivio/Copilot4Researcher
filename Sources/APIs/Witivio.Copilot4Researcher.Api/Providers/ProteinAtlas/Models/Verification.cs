using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class Verification
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlText]
        public string Status { get; set; }
    }


}
