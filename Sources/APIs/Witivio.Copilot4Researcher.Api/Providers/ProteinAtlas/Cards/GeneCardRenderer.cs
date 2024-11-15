using AdaptiveCards;
using Witivio.Copilot4Researcher.Common;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.ProteinAtlas.Cards
{
    public class GeneCardRenderer
    {
        public static AdaptiveCard Render(Gene gene)
        {
            var card = new AdaptiveCard("1.5")
            {
                Body = new List<AdaptiveElement>
            {
                new AdaptiveTextBlock
                {
                    Text = gene.Name,
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.ExtraLarge,
                    Wrap = true,
                    Style = AdaptiveTextBlockStyle.Heading
                },
                new AdaptiveFactSet
                {
                    Facts = new List<AdaptiveFact>
                    {
                        new AdaptiveFact("Protein:", gene.GeneDescription),
                        new AdaptiveFact("Gene name:", gene.GeneName),
                        new AdaptiveFact("Protein class:", gene.ProteinClass),
                        //new AdaptiveFact("Transcripts:", gene.Transcripts),
                        new AdaptiveFact("Interactions:", $"Interacting with {gene.Interactions} proteins")
                    },
                    Spacing = AdaptiveSpacing.None
                },
                new AdaptiveActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new AdaptiveOpenUrlAction
                        {
                            Title = "ProteinAtlas",
                            IconUrl = CardIcons.ExternalLink,
                            Url = new Uri(gene.ProteinAtlasLink)
                        },
                        new AdaptiveOpenUrlAction
                        {
                            Title = "Uniprot",
                            IconUrl = CardIcons.ExternalLink,
                            Url = new Uri(gene.UniprotLink)
                        }
                    }
                },
                new AdaptiveTextBlock
                {
                    Text = "Protein expression and localization",
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.Large,
                    Wrap = true
                }
            }
            };

            var expressionsList = new List<TissueExpression>(gene.Expressions); // Convert IEnumerable to List for indexing
            for (int i = 0; i < expressionsList.Count; i++)
            {
                var expression = expressionsList[i];

                card.Body.Add(new AdaptiveContainer
                {
                    Items = new List<AdaptiveElement>
                        {
                            new AdaptiveColumnSet
                            {
                                Columns = new List<AdaptiveColumn>
                                {
                                    new AdaptiveColumn
                                    {
                                        Items = new List<AdaptiveElement>
                                        {
                                            new AdaptiveTextBlock
                                            {
                                                Text = expression.Organ,
                                                Wrap = true,
                                                Size = AdaptiveTextSize.Medium,
                                                Weight = AdaptiveTextWeight.Bolder
                                            }
                                        },
                                        Width = "stretch"
                                    },
                                    new AdaptiveColumn
                                    {
                                        Id = $"chevronDown_{i}",
                                        Spacing = AdaptiveSpacing.Small,
                                        VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                        Items = new List<AdaptiveElement>
                                        {
                                            new AdaptiveImage
                                            {
                                                Url = new Uri("https://adaptivecards.io/content/down.png"),
                                                PixelWidth = 20,
                                                AltText = "collapsed"
                                            }
                                        },
                                        Width = "auto"
                                    },
                                    new AdaptiveColumn
                                    {
                                        Id = $"chevronUp_{i}",
                                        Spacing = AdaptiveSpacing.Small,
                                        VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                        Items = new List<AdaptiveElement>
                                        {
                                            new AdaptiveImage
                                            {
                                                Url = new Uri("https://adaptivecards.io/content/up.png"),
                                                PixelWidth = 20,
                                                AltText = "expanded"
                                            }
                                        },
                                        Width = "auto",
                                        IsVisible = false
                                    }
                                },
                                SelectAction = new AdaptiveToggleVisibilityAction
                                {
                                    TargetElements = new List<AdaptiveTargetElement> { $"cardContent_{i}", $"chevronUp_{i}", $"chevronDown_{i}" }
                                }
                            },
                            new AdaptiveContainer
                            {
                                Id = $"cardContent_{i}",
                                IsVisible = false,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new Uri(expression.ImageUrl),
                                        AdditionalProperties = new SerializableDictionary<string, object>
                                        {
                                            { "msTeams",new { allowExpand=true} }
                                        }   
                                    }
                                }
                            }
                        },
                    Separator = true,
                    Spacing = AdaptiveSpacing.Medium
                });
            }

            card.Body.Add(new AdaptiveTextBlock
            {
                Text = "Source: proteinatlas.org",
                Wrap = true,
                Size = AdaptiveTextSize.Small,
                IsSubtle = true
            });

            return card;
        }



    }
}
