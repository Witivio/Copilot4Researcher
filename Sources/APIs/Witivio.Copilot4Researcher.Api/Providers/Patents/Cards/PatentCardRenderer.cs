using AdaptiveCards;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.Patents.Cards
{
    public class PatentCardRenderer
    {
        public const string OpenExternalIcon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAA7EAAAOxAGVKw4bAAABZ0lEQVRIieXVu0pcURTG8V9kCCKWYhVERMQHCKIg4g0REbS0skjEwrxBXsLCh7CUkC5FakshxEtC1CRFZBoVC43ijMXZJx7F0TWZ6fxgc/Zlrf+3ztmHvcn0Bnu4eqStu6v3+Fsj9hp/sJzDq4H29Z7BeiBn7wV20I9PWEXFw9pCuTBuxyBaCnNzWCmMT+A8uc3UAEc1K/tk1VR0NbH/TU43AJ92W+gXvM4NWh7LCmoKG2jFNiZQSmuVRg0m8SHBdxO8jI60ftmIwXgB/g1jOEprbY0ajOJjAt2Hw8v0vKD+TR7BWcr5jlcPxHTK3m6lXoPhAvwHuiIVRQ0GcZpiD6LwqMFAAf4T3VF4xKAXxynmN3oCzL5UyFrEYLwA7w0WvZjnlJ6KxGcMYd/dwy6kiAFs1gvO1Yyz6BkYVAr9ZnKRbfIv2X/7DpdqX5lRlfA29Q9hSezS/582n7suyw6vZkCvZXfyAtwA7YGix5SE1SIAAAAASUVORK5CYII=";
        public const string DownloadUrlIcon = "data:image/gif;base64,R0lGODlhIAAgAHAAACH5BAEAADgALAAAAAAgACAAhQQEBAEBAVdXV0NDQwAAAAYGBkFBQVZWVmZmZnNzcwMDAwICAiIiIl1dXTQ0NCMjI0JCQiEhIUdHRwsLCygoKA8PDwUFBRISEiAgIIeHhyQkJGFhYR8fH1FRUXZ2dnBwcFRUVHFxcTg4OB4eHl5eXhQUFGJiYnR0dAwMDBcXFwcHBxgYGBYWFhkZGRoaGlVVVR0dHVNTUw4ODhUVFYWFhUREREBAQHh4eAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAb2QJxwSCwaj8ikEBAQKJ/HAYFQgFqHhmn1as1SuV0tGOrdjpPlszKtRrLbxjecKD8finVE4qogLOhiQwxTDVYEAYdYgTiDUw5WD1OJOBCBjZNWEZIBOBJTEzgUm2OaUwAVUxYXo2cYklMLiFOcba6vCoi0ShkaG0gcrwQKf0gUHUJ9ukbArEcFUx44kh9JzARKkiDSUyFKIiNP2dsE3XOwJOPlcMIlJuMnc9IoKYV+KuhziCraACUrLBYCChxIsGCKFi6mvMAxgcOIEcEiSpyiYAQMBn1i4CjgAoOMiSBfsYgw4xANITVsGIBgoKXLlzBdDoB5A0cQADs=";
        public const string MoreIcon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAFYAAABWAHfbyrhAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAAGxQTFRF////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfHZfwQAAACN0Uk5TAAQGBwkKERUXIS0wOTo/SU50fZCWmLvBxNna4uXv8vP1/f44GNoMAAAA+klEQVQ4y4WTyZKCQBBEU0FE2WSz2aSB9///OAcnHMclKi8d0bVXZUoPBEXrBu8H1xaB3hE3Mw/MTfxi3lf+blqW++ur/b/wDlhdmURSlJRuBbqnJOcR6NO/j7QHxvMjfoStDp9ThvUG42+OfQdT/tp0PkF376OCLX8fK9+gkqTYQ/1hbtXgY0kN9OEnh7CHRgpm1lQfka7MgQpw+gIHhVoovzmU0MpBIul02UmSskyStLucJCXgNEAkVSvTUdIVrpKOE2slRTDIs0iHG1BKGUAmlcDtIC1428EsYTZpjmkuyly1eSz73CZhbMqZpLVpbwvHlp4t3u/y/wEDNSwdtLEsJwAAAABJRU5ErkJggg==";
        public static AdaptiveCard Render(Patent patent)
        {

            var card = new AdaptiveCard("1.6")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = patent.Title,
                       Style = AdaptiveTextBlockStyle.Heading,
                        Wrap=true
                    },
                    new AdaptiveFactSet
                    {
                        Facts = new List<AdaptiveFact>
                        {
                            new AdaptiveFact("WIPO:", patent.WIPO),
                            new AdaptiveFact("Inventor:", patent.Inventor),
                            new AdaptiveFact("Assignee:", patent.Assignee ?? "N/A"),
                            new AdaptiveFact("Priority date:", patent.PriorityDate?.ToString("yyyy-MM-dd") ?? "N/A"),
                            new AdaptiveFact("Grant date:", patent.GrantDate?.ToString("yyyy-MM-dd") ?? "N/A"),
                            new AdaptiveFact("Publication date:", patent.PublicationDate?.ToString("yyyy-MM-dd") ?? "N/A")
                        }
                    },
                    new AdaptiveRichTextBlock
                    {
                        Inlines = new List<AdaptiveInline>
                        {
                            new AdaptiveTextRun("Countries:    ")
                            {
                                Weight = AdaptiveTextWeight.Bolder
                            },
                        }
                        .Concat(patent.Countries.Select(country =>
                            new AdaptiveTextRun($"{country.Name}, ")
                            {
                                Strikethrough = country.Status == PatentStatus.NOT_ACTIVE,
                                Weight = country.Status == PatentStatus.ACTIVE ? AdaptiveTextWeight.Bolder : AdaptiveTextWeight.Default
                            }
                        )).ToList()
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveShowCardAction
                    {
                        Title = "Details",
                        IconUrl = MoreIcon,
                        Card = new AdaptiveCard("1.6")
                        {
                            Body = new List<AdaptiveElement>
                            {
                                new AdaptiveTextBlock
                                {
                                    Text = patent.Summary,
                                    Wrap = true
                                }
                            }
                        }
                    },
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Google Patent",
                        IconUrl = OpenExternalIcon,
                        Url = new Uri(patent.GooglePatentUrl)
                    },
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Patent Scope",
                        IconUrl = OpenExternalIcon,
                        Url = new Uri(patent.PatentScopeUrl)
                    },
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Download PDF",
                        IconUrl = DownloadUrlIcon,
                        Url = new Uri(patent.PDFUrl)
                    }
                    
                }
            };

            return card;
        }
    }
}
