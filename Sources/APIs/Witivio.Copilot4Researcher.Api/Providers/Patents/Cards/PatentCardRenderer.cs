using AdaptiveCards;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.Patents.Cards
{
    public class PatentCardRenderer
    {
        public const string OpenExternalIcon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAA7EAAAOxAGVKw4bAAABZ0lEQVRIieXVu0pcURTG8V9kCCKWYhVERMQHCKIg4g0REbS0skjEwrxBXsLCh7CUkC5FakshxEtC1CRFZBoVC43ijMXZJx7F0TWZ6fxgc/Zlrf+3ztmHvcn0Bnu4eqStu6v3+Fsj9hp/sJzDq4H29Z7BeiBn7wV20I9PWEXFw9pCuTBuxyBaCnNzWCmMT+A8uc3UAEc1K/tk1VR0NbH/TU43AJ92W+gXvM4NWh7LCmoKG2jFNiZQSmuVRg0m8SHBdxO8jI60ftmIwXgB/g1jOEprbY0ajOJjAt2Hw8v0vKD+TR7BWcr5jlcPxHTK3m6lXoPhAvwHuiIVRQ0GcZpiD6LwqMFAAf4T3VF4xKAXxynmN3oCzL5UyFrEYLwA7w0WvZjnlJ6KxGcMYd/dwy6kiAFs1gvO1Yyz6BkYVAr9ZnKRbfIv2X/7DpdqX5lRlfA29Q9hSezS/582n7suyw6vZkCvZXfyAtwA7YGix5SE1SIAAAAASUVORK5CYII=";

        public static AdaptiveCard Render(Patent patent)
        {

            var card = new AdaptiveCard("1.6")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = patent.Title,
                        Size = AdaptiveTextSize.Medium,
                        Weight = AdaptiveTextWeight.Bolder
                    },
                    new AdaptiveTextBlock
                    {
                        Text = patent.Title,
                        Wrap = true,
                        Size = AdaptiveTextSize.Large,
                        Weight = AdaptiveTextWeight.Bolder
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
                            new AdaptiveTextRun($"{country.Name}{(country.Status == PatentStatus.NOT_ACTIVE ? ", " : " ")}")
                            {
                                Strikethrough = country.Status == PatentStatus.NOT_ACTIVE,
                                Weight = country.Status == PatentStatus.ACTIVE ? AdaptiveTextWeight.Bolder : AdaptiveTextWeight.Default
                            }
                        )).ToList()
                    },
                    new AdaptiveTextBlock
                    {
                        Text = patent.Summary,
                        Wrap = true
                    }
                },
                Actions = new List<AdaptiveAction>
                {
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
                        IconUrl = OpenExternalIcon,
                        Url = new Uri(patent.PDFUrl)
                    },
                    new AdaptiveShowCardAction
                    {
                        Title = "Detail",
                        Card = new AdaptiveCard("1.6")
                        {
                            Body = new List<AdaptiveElement>
                            {
                                new AdaptiveTextBlock
                                {
                                    Text = patent.Summary
                                }
                            }
                        }
                    }
                }
            };

            return card;
        }
    }
}
