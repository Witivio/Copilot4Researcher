using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Witivio.Copilot4Researcher.Models
{
    public class PatentSearchQuery
    {


        public string UserInput { get; set; }



        public string UserIntent { get; set; }



        public string[] Keywords { get; set; }

        public string[] Countries { get; set; }
        public string BeforePriorityDate { get; internal set; }
        public string BeforeFillingDate { get; internal set; }
        public string BeforePublicationDate { get; internal set; }
        public string AfterPriorityDate { get; internal set; }
        public string AfterFillingDate { get; internal set; }
        public string AfterPublicationDate { get; internal set; }
        public string Language { get; internal set; }
        public string Litigation { get; internal set; }
        public string Assignees { get; internal set; }
        public string Inventors { get; internal set; }
        public string Type { get; internal set; }
        public string Status { get; internal set; }
    }

    public class PatentSearchInputQuery
    {
        [Required]
        [SwaggerSchema(Description = "User input as a natural language query, e.g., 'Show latest articles about BRCA1.'")]
        public string UserInput { get; set; }

        [Required]
        [SwaggerSchema(Description = "User's intent derived from the natural language input, e.g., 'search for recent studies.'")]
        public string UserIntent { get; set; }

        [Required]
        [SwaggerSchema(Description = "Keywords or topics to focus the search on, extracted from the user input, separated by semicolons.")]
        public string Keywords { get; set; }

        [SwaggerSchema(Description = "ISO 2 letters name of one or many countries separated by semicolons. Default is empty")]
        public string Countries { get; set; }

        [SwaggerSchema(Description = "Language of the patent. Supported value: ENGLISH, GERMAN, CHINESE, FRENCH, SPANISH, ARABIC, JAPANESE, KOREAN, PORTUGUESE, RUSSIAN, ITALIAN, DUTCH, SWEDISH, FINNISH, NORWEGIAN, DANISH. Default is ENGLISH")]
        public string Language { get; set; }

        [SwaggerSchema(Description = "Optional. Filter to get patent priority before a date. The format is YYYYMMDD")]
        public string BeforePriorityDate { get; set; }
        [SwaggerSchema(Description = "Optional. Filter to get patent filling before a date. The format is YYYYMMDD")]
        public string BeforeFillingDate { get; set; }
        [SwaggerSchema(Description = "Optional. Filter to get patent publication before a date. The format is YYYYMMDD")]
        public string BeforePublicationDate { get; set; }

        [SwaggerSchema(Description = "Optional. Filter to get patent priority after a date. The format is YYYYMMDD")]
        public string AfterPriorityDate { get; set; }
        [SwaggerSchema(Description = "Optional. Filter to get patent filling after a date. The format is YYYYMMDD")]
        public string AfterFillingDate { get; set; }
        [SwaggerSchema(Description = "Optional. Filter to get patent publication after a date. The format is YYYYMMDD")]
        public string AfterPublicationDate { get; set; }

        [SwaggerSchema(Description = "Optional. Inventors of the patents. Separated by comma.")]
        public string Inventors { get; set; }

        [SwaggerSchema(Description = "Optional. Assignees  of the patents. Separated by comma.")]
        public string Assignees { get; set; }

        [SwaggerSchema(Description = "Optional. Filter patent results by status. Supported value: APPLICATION, GRANT")]
        public string Status { get; set; }

        [SwaggerSchema(Description = "Optional. Filter patent results by type. Supported value: PATENT, DESIGN")]
        public string Type { get; set; }

        [SwaggerSchema(Description = "Optional. Filter patent results by litigation status. Supported value: YES, NO")]
        public string Litigation { get; set; }

        public PatentSearchQuery ToQuery()
        {
            var result = new PatentSearchQuery
            {
                UserInput = this.UserInput,
                UserIntent = this.UserIntent,
                Keywords = this.Keywords?.Split(";"),
                Countries = this.Countries?.Split(";"),
                Language = this.Language,
                BeforePriorityDate = this.BeforePriorityDate,
                BeforeFillingDate = this.BeforeFillingDate,
                BeforePublicationDate = this.BeforePublicationDate,
                AfterPriorityDate = this.AfterPriorityDate,
                AfterFillingDate = this.AfterFillingDate,
                AfterPublicationDate = this.AfterPublicationDate,
                Status = this.Status,
                Type = this.Type,
                Litigation = this.Litigation,
                Assignees = this.Assignees,
                Inventors = this.Inventors,
                
            };
            return result;
        }

    }
}