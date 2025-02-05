  namespace Witivio.Copilot4Researcher.Api.Core.Options
  {
    public class SharePointOptions
    {
        public const string SectionName = "SharePoint";
        
        public string Url { get; set; }
        public string ListId { get; set; }

        public string LogListId { get; set; }

        public string DeliveryNotesUrl { get; set; }

        public int ScanIntervalMinutes { get; set; }
    }
  }
