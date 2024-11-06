using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class ProteinClass
    {
        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "parent_id")]
        public string ParentId { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }


}
