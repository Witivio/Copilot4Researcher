using AdaptiveCards;
using Witivio.Copilot4Researcher.Common;
using Witivio.Copilot4Researcher.Features.Patents.Models;

namespace Witivio.Copilot4Researcher.Features.Patents.Cards
{
    public class PatentCardRenderer
    {
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
                        IconUrl = CardIcons.More,
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
                        IconUrl = CardIcons.ExternalLink,
                        Url = new Uri(patent.GooglePatentUrl)
                    },
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Patent Scope",
                        IconUrl = CardIcons.ExternalLink,
                        Url = new Uri(patent.PatentScopeUrl)
                    },
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Download PDF",
                        IconUrl = CardIcons.Download,
                        Url = new Uri(patent.PDFUrl)
                    }

                }
            };

            return card;
        }
    }
}
