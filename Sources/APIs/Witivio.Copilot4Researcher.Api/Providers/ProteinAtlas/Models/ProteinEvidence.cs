using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class ProteinEvidence
    {
        [XmlAttribute(AttributeName = "evidence")]
        public string Evidence { get; set; }

        [XmlElement(ElementName = "evidence")]
        public List<Evidence> Evidences { get; set; }
    }


}
