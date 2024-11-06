using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Models
{
    public class Entry
    {
        [XmlAttribute(AttributeName = "version")]
        public int Version { get; set; }

        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "synonym")]
        public List<string> Synonyms { get; set; }

        [XmlElement(ElementName = "identifier")]
        public Identifier Identifier { get; set; }

        [XmlArray(ElementName = "proteinClasses")]
        [XmlArrayItem(ElementName = "proteinClass")]
        public List<ProteinClass> ProteinClasses { get; set; }

        [XmlElement(ElementName = "proteinEvidence")]
        public ProteinEvidence ProteinEvidence { get; set; }

        [XmlElement(ElementName = "predictedLocation")]
        public string PredictedLocation { get; set; }

        [XmlElement(ElementName = "tissueExpression")]
        public TissueExpression TissueExpression { get; set; }
    }


}
