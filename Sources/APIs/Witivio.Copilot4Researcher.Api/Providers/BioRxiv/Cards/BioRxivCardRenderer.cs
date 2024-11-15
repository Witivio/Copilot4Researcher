using AdaptiveCards;
using Witivio.Copilot4Researcher.Common;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Cards
{
    public class BioRxivCardRenderer
    {
        public static AdaptiveCard Render(Publication publication)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 5));

            // Title
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = publication.Title,
                Size = AdaptiveTextSize.Medium,
                Weight = AdaptiveTextWeight.Bolder,
                Wrap = true
            });

            // Facts
            var factSet = new AdaptiveFactSet();

            // Authors fact
            var authorText = $"{publication.Authors.First}, ..., {publication.Authors.Last}";


            factSet.Facts.Add(new AdaptiveFact("Authors", authorText));
            if (publication.Date.HasValue)
            {
                factSet.Facts.Add(new AdaptiveFact("Date", publication.Date.Value.ToString("yyyy-MM-dd")));
            }
            else
            {
                factSet.Facts.Add(new AdaptiveFact("Date", "N/A"));
            }

            if (publication.Citations.HasValue && publication.Citations.Value > 0)
            {
                factSet.Facts.Add(new AdaptiveFact("Citations", publication.Citations.ToString()));
            }

            factSet.Facts.Add(new AdaptiveFact("Journal", publication.JournalName));

            if (publication.ImpactFactor.HasValue && publication.ImpactFactor.Value > 0)
            {
                factSet.Facts.Add(new AdaptiveFact("Impact factor", publication.ImpactFactor.ToString()));
            }
            else
            {
                factSet.Facts.Add(new AdaptiveFact("Impact factor", "N/A"));
            }

            card.Body.Add(factSet);

            // Action Set
            var actionSet = new AdaptiveActionSet();

            // Abstract action
            var abstractAction = new AdaptiveShowCardAction
            {
                Title = "Abstract",
                IconUrl = CardIcons.Abstract
            };
            abstractAction.Card = new AdaptiveCard("1.6");
            abstractAction.Card.Body.Add(new AdaptiveTextBlock
            {
                Text = publication.Abstract,
                Wrap = true
            });

            // See source action
            var sourceAction = new AdaptiveOpenUrlAction
            {
                Title = $"See {publication.Source}",
                Url = new Uri(publication.Link),
                IconUrl = CardIcons.ExternalLink
            };

            actionSet.Actions.Add(abstractAction);
            actionSet.Actions.Add(sourceAction);

            card.Body.Add(actionSet);

            return card;
        }
    }
}
