using AdaptiveCards;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using Witivio.Copilot4Researcher.Common;
using Witivio.Copilot4Researcher.Features.Collaboration.Models;

namespace Witivio.Copilot4Researcher.Features.Collaboration.Cards
{
    public class FallbackContactCardRenderer
    {
        public static AdaptiveCard Render(Models.Recipient recipient, Product product)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 5));

            // Create column set for profile
            var columnSet = new AdaptiveColumnSet();


            var imageColumn = new AdaptiveColumn
            {
                Width = "auto",
                Items = new List<AdaptiveElement>
                    {
                        new AdaptiveContainer
                        {
                            BackgroundImage = new AdaptiveBackgroundImage
                            {
                                Url = new Uri(CardIcons.BlueCircle),
                                FillMode = AdaptiveImageFillMode.Cover
                            },
                            Items = new List<AdaptiveElement>
                            {
                                new AdaptiveTextBlock
                                {
                                    Text = GenerateInitals(recipient.Name),
                                    Weight = AdaptiveTextWeight.Bolder,
                                    Color = AdaptiveTextColor.Light
                                }
                            }
                        }
                    }
            };


            // Text column
            var textColumn = new AdaptiveColumn { Width = AdaptiveColumnWidth.Stretch };
            textColumn.Items.Add(new AdaptiveTextBlock
            {
                Text = recipient.Name,
                Weight = AdaptiveTextWeight.Bolder,
                Wrap = true
            });
            if (!string.IsNullOrWhiteSpace(recipient.UnitName))
                textColumn.Items.Add(new AdaptiveTextBlock
                {
                    Text = recipient.UnitName,
                    IsSubtle = true,
                    Spacing = AdaptiveSpacing.None,
                    Wrap = true
                });


            columnSet.Columns.Add(imageColumn);
            columnSet.Columns.Add(textColumn);
            card.Body.Add(columnSet);




            // Add show card action with facts
            var showCardAction = new AdaptiveShowCardAction
            {
                Title = "Product details",
                IconUrl = CardIcons.More,
            };

            var innerCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 5));
            innerCard.Body.Add(new AdaptiveFactSet
            {
                Facts = new List<AdaptiveFact>
                {
                    new AdaptiveFact("Provider:", product.Delivery.ProviderName),
                    new AdaptiveFact("Product name:", product.Name),
                    new AdaptiveFact("Reference:", product.Reference),
                    new AdaptiveFact("Delivered:", product.Delivery.Date.GetValueOrDefault(DateOnly.MinValue).ToString("yyyy-MM-dd"))
                }
            });

            showCardAction.Card = innerCard;
            card.Actions.Add(showCardAction);

            return card;
        }

        private static string GenerateInitals(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            var initials = name.Split(' ')
                                .Where(n => !string.IsNullOrWhiteSpace(n) && char.IsLetter(n[0]))
                                .Select(n => n[0])
                                .Take(2)
                                .ToArray();

            return new string(initials).ToUpper();
        }
    }
}