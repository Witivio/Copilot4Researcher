using System.Text;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Core
{
    public static class CitationFormatGenerator
    {
        public static string GenerateNbibContent(Publication publication)
        {
            var nbib = new StringBuilder();
            
            // PMID
            nbib.AppendLine($"PMID- {publication.Id}");
            
            // Authors
            nbib.AppendLine($"FAU - {publication.Authors.First}");
            
            // Title
            nbib.AppendLine($"TI  - {publication.Title}");
            
            // Journal info
            nbib.AppendLine($"JT  - {publication.JournalName}");
            
            // Publication date
            if (publication.Date.HasValue)
            {
                nbib.AppendLine($"DP  - {publication.Date.Value:yyyy MMM dd}");
            }
            
            // Abstract
            if (!string.IsNullOrEmpty(publication.Abstract))
            {
                nbib.AppendLine($"AB  - {publication.Abstract}");
            }
            
            // DOI
            if (!string.IsNullOrEmpty(publication.DOI))
            {
                nbib.AppendLine($"LID - {publication.DOI} [doi]");
            }
            
            return nbib.ToString();
        }

        public static string GenerateBibtexContent(Publication publication)
        {
            var bibtex = new StringBuilder();
            
            // Create a citation key using first author's lastname and year
            var firstAuthor = publication.Authors.First?.Split(' ').Last() ?? "Unknown";
            var year = publication.Date?.Year.ToString() ?? "XXXX";
            var citationKey = $"{firstAuthor}{year}";
            
            bibtex.AppendLine($"@article{{{citationKey},");
            bibtex.AppendLine($"  title = {{{publication.Title}}},");
            bibtex.AppendLine($"  author = {{{publication.Authors.First}}},");
            bibtex.AppendLine($"  journal = {{{publication.JournalName}}},");
            
            if (publication.Date.HasValue)
            {
                bibtex.AppendLine($"  year = {{{publication.Date.Value.Year}}},");
                bibtex.AppendLine($"  month = {{{publication.Date.Value.ToString("MMM").ToLower()}}},");
            }
            
            if (!string.IsNullOrEmpty(publication.DOI))
            {
                bibtex.AppendLine($"  doi = {{{publication.DOI}}},");
            }
            
            bibtex.AppendLine($"  pmid = {{{publication.Id}}},");
            
            if (!string.IsNullOrEmpty(publication.Abstract))
            {
                bibtex.AppendLine($"  abstract = {{{publication.Abstract}}},");
            }
            
            // Remove trailing comma from last field and close the entry
            var result = bibtex.ToString().TrimEnd(',', '\n') + "\n}";
            return result;
        }
    }
} 