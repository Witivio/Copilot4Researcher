using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas.Models
{
    public class Identifier
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "db")]
        public string Db { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "assembly")]
        public string Assembly { get; set; }

        [XmlAttribute(AttributeName = "gencodeVersion")]
        public string GencodeVersion { get; set; }

        [XmlElement(ElementName = "xref")]
        public List<Xref> Xrefs { get; set; }
    }


}
