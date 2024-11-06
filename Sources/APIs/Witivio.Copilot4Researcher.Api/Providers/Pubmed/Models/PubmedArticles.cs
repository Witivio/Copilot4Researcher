using System.Security.Cryptography.Xml;
using System.Xml.Serialization;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot("PubmedArticleSet")]
    public class PubmedArticleSet
    {
        [XmlElement("PubmedArticle")]
        public List<PubmedArticle> PubmedArticles { get; set; }
    }

    [XmlRoot("PubmedArticle")]
    public class PubmedArticle
    {
        [XmlElement("MedlineCitation")]
        public MedlineCitation MedlineCitation { get; set; }

        [XmlElement("PubmedData")]
        public PubmedData PubmedData { get; set; }
    }

    [XmlRoot("MedlineCitation")]
    public class MedlineCitation
    {
        [XmlAttribute("Status")]
        public string Status { get; set; }

        [XmlAttribute("Owner")]
        public string Owner { get; set; }

        [XmlElement("PMID")]
        public string Pmid { get; set; }

        [XmlElement("DateRevised")]
        public Date DateRevised { get; set; }

        [XmlElement("Article")]
        public Article Article { get; set; }

        [XmlElement("MedlineJournalInfo")]
        public MedlineJournalInfo MedlineJournalInfo { get; set; }

        [XmlAnyElement("CoiStatement")]
        public XmlElement[] CoiStatement { get; set; }
    }

    [XmlRoot("Date")]
    public class Date
    {
        [XmlElement("Year")]
        public int Year { get; set; }

        [XmlElement("Month")]
        public string Month { get; set; }

        [XmlElement("Day")]
        public int Day { get; set; }
    }

    [XmlRoot("Article")]
    public class Article
    {
        [XmlAttribute("PubModel")]
        public string PubModel { get; set; }

        [XmlElement("Journal")]
        public Journal Journal { get; set; }



        [XmlAnyElement("ArticleTitle")]
        public XmlElement[] ArticleTitle { get; set; }

        [XmlElement("ELocationID")]
        public ELocationID ELocationId { get; set; }

        [XmlElement("Abstract")]
        public Abstract Abstract { get; set; }

        [XmlElement("AuthorList")]
        public AuthorList AuthorList { get; set; }

        [XmlElement("Language")]
        public string Language { get; set; }

        [XmlElement("PublicationTypeList")]
        public PublicationTypeList PublicationTypeList { get; set; }

        [XmlElement("ArticleDate")]
        public Date ArticleDate { get; set; }
    }

    [XmlRoot("Journal")]
    public class Journal
    {
        [XmlElement("ISSN")]
        public Issn Issn { get; set; }

        [XmlElement("JournalIssue")]
        public JournalIssue JournalIssue { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("ISOAbbreviation")]
        public string IsoAbbreviation { get; set; }
    }

    [XmlRoot("ISSN")]
    public class Issn
    {
        [XmlAttribute("IssnType")]
        public string IssnType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("JournalIssue")]
    public class JournalIssue
    {
        [XmlAttribute("CitedMedium")]
        public string CitedMedium { get; set; }

        [XmlElement("PubDate")]
        public Date PubDate { get; set; }
    }

    [XmlRoot("ELocationID")]
    public class ELocationID
    {
        [XmlAttribute("EIdType")]
        public string EIdType { get; set; }

        [XmlAttribute("ValidYN")]
        public string ValidYn { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("Abstract")]
    public class Abstract
    {
        [XmlAnyElement("AbstractText")]
        public XmlElement[] AbstractText { get; set; }

        [XmlElement("CopyrightInformation")]
        public string CopyrightInformation { get; set; }
    }

    public static class AbstractExtension
    {
        public static string GetAbstractTextAsPlainText(this Abstract @abstract)
        {
            if (@abstract.AbstractText == null || @abstract.AbstractText.Length == 0)
            {
                return string.Empty;
            }

            // Combine all the text content of the XmlElements, removing any XML/HTML tags
            var plainText = string.Empty;

            foreach (var element in @abstract.AbstractText)
            {
                plainText += element.InnerText; // This will extract the text, ignoring tags
            }

            return plainText;
        }

        public static string GetArticleTitleTextAsPlainText(this Article article)
        {
            if (article.ArticleTitle == null || article.ArticleTitle.Length == 0)
            {
                return string.Empty;
            }

            // Combine all the text content of the XmlElements, removing any XML/HTML tags
            var plainText = string.Empty;

            foreach (var element in article.ArticleTitle)
            {
                plainText += element.InnerText; // This will extract the text, ignoring tags
            }

            return plainText;
        }

        public static string GetAbstractTextAsHtml(this Abstract @abstract)
        {
            if (@abstract.AbstractText == null || @abstract.AbstractText.Length == 0)
            {
                return string.Empty;
            }

            // Combine the OuterXml of each element to retain the HTML tags
            var htmlContent = string.Empty;

            foreach (var element in @abstract.AbstractText)
            {
                htmlContent += element.OuterXml; // This will retain the tags
            }

            return htmlContent;
        }
    }

    [XmlRoot("AuthorList")]
    public class AuthorList
    {
        [XmlAttribute("CompleteYN")]
        public string CompleteYn { get; set; }

        [XmlElement("Author")]
        public List<Author> Authors { get; set; }
    }

    [XmlRoot("Author")]
    public class Author
    {
        [XmlAttribute("ValidYN")]
        public string ValidYn { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("ForeName")]
        public string ForeName { get; set; }

        [XmlElement("Initials")]
        public string Initials { get; set; }

        [XmlElement("Identifier")]
        public Identifier Identifier { get; set; }

        [XmlElement("AffiliationInfo")]
        public List<AffiliationInfo> AffiliationInfo { get; set; }
    }

    [XmlRoot("Identifier")]
    public class Identifier
    {
        [XmlAttribute("Source")]
        public string Source { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("AffiliationInfo")]
    public class AffiliationInfo
    {
        [XmlElement("Affiliation")]
        public string Affiliation { get; set; }
    }

    [XmlRoot("PublicationTypeList")]
    public class PublicationTypeList
    {
        [XmlElement("PublicationType")]
        public List<PublicationType> PublicationTypes { get; set; }
    }

    [XmlRoot("PublicationType")]
    public class PublicationType
    {
        [XmlAttribute("UI")]
        public string Ui { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("MedlineJournalInfo")]
    public class MedlineJournalInfo
    {
        [XmlElement("Country")]
        public string Country { get; set; }

        [XmlElement("MedlineTA")]
        public string MedlineTa { get; set; }

        [XmlElement("NlmUniqueID")]
        public string NlmUniqueId { get; set; }

        [XmlElement("ISSNLinking")]
        public string IssnLinking { get; set; }
    }

    [XmlRoot("PubmedData")]
    public class PubmedData
    {
        [XmlElement("History")]
        public History History { get; set; }

        [XmlElement("PublicationStatus")]
        public string PublicationStatus { get; set; }

        [XmlElement("ArticleIdList")]
        public ArticleIdList ArticleIdList { get; set; }
    }

    [XmlRoot("History")]
    public class History
    {
        [XmlElement("PubMedPubDate")]
        public List<PubMedPubDate> PubMedPubDates { get; set; }
    }

    [XmlRoot("PubMedPubDate")]
    public class PubMedPubDate
    {
        [XmlAttribute("PubStatus")]
        public string PubStatus { get; set; }

        [XmlElement("Year")]
        public int Year { get; set; }

        [XmlElement("Month")]
        public int Month { get; set; }

        [XmlElement("Day")]
        public int Day { get; set; }

        [XmlElement("Hour")]
        public int Hour { get; set; }

        [XmlElement("Minute")]
        public int Minute { get; set; }
    }

    [XmlRoot("ArticleIdList")]
    public class ArticleIdList
    {
        [XmlElement("ArticleId")]
        public List<ArticleId> ArticleIds { get; set; }
    }

    [XmlRoot("ArticleId")]
    public class ArticleId
    {
        [XmlAttribute("IdType")]
        public string IdType { get; set; }

        [XmlText]
        public string Text { get; set; }
    }




}
