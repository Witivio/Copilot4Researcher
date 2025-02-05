using CsvHelper.Configuration;
using System.Globalization;

namespace Witivio.Copilot4Researcher.Features.Literature.Scimago
{
    public class JournalData
    {
        public int Rank { get; set; }
        public string SourceId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Issn { get; set; }
        public decimal? SJR { get; set; }
        public string SJRBestQuartile { get; set; }
        public int HIndex { get; set; }
        public int TotalDocs2023 { get; set; }
        public int TotalDocs3Years { get; set; }
        public int TotalRefs { get; set; }
        public int TotalCites3Years { get; set; }
        public int CitableDocs3Years { get; set; }
        public decimal CitesPerDoc2Years { get; set; }
        public decimal RefPerDoc { get; set; }
        public decimal FemalePercentage { get; set; }
        public int Overton { get; set; }
        public int SDG { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Publisher { get; set; }
        public string Coverage { get; set; }
        public string Categories { get; set; }
        public string Areas { get; set; }
    }

    public sealed class JournalDataMap : ClassMap<JournalData>
    {
        public JournalDataMap()
        {
            Map(m => m.Rank).Name("Rank");
            Map(m => m.SourceId).Name("Sourceid");
            Map(m => m.Title).Name("Title");
            Map(m => m.Type).Name("Type");
            Map(m => m.Issn).Name("Issn");
            Map(m => m.SJR).Name("SJR");
            Map(m => m.SJRBestQuartile).Name("SJR Best Quartile");
            Map(m => m.HIndex).Name("H index");
            Map(m => m.TotalDocs2023).Name("Total Docs. (2023)");
            Map(m => m.TotalDocs3Years).Name("Total Docs. (3years)");
            Map(m => m.TotalRefs).Name("Total Refs.");
            Map(m => m.TotalCites3Years).Name("Total Cites (3years)");
            Map(m => m.CitableDocs3Years).Name("Citable Docs. (3years)");
            Map(m => m.CitesPerDoc2Years).Name("Cites / Doc. (2years)").TypeConverter<CommaDecimalConverter>();
            Map(m => m.RefPerDoc).Name("Ref. / Doc.").TypeConverter<CommaDecimalConverter>();
            Map(m => m.FemalePercentage).Name("%Female").TypeConverter<CommaDecimalConverter>();
            Map(m => m.Overton).Name("Overton");
            Map(m => m.SDG).Name("SDG");
            Map(m => m.Country).Name("Country");
            Map(m => m.Region).Name("Region");
            Map(m => m.Publisher).Name("Publisher");
            Map(m => m.Coverage).Name("Coverage");
            Map(m => m.Categories).Name("Categories");
            Map(m => m.Areas).Name("Areas");
        }
    }
}
