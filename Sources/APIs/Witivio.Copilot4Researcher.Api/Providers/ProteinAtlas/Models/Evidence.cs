using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class Evidence
    {
        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; }

        [XmlAttribute(AttributeName = "evidence")]
        public string EvidenceLevel { get; set; }
    }


}
