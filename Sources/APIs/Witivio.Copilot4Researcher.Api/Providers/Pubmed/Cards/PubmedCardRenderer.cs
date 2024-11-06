using AdaptiveCards;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.Pubmed.Cards
{
    public class PubmedCardRenderer
    {
        public static AdaptiveCard Render(Publication publication)
        {
            // Create a new adaptive card
            AdaptiveCard card = new AdaptiveCard("1.5");

            // Add title text block
            card.Body.Add(new AdaptiveTextBlock
            {
                Text = publication.Title,
                Size = AdaptiveTextSize.Medium,
                Weight = AdaptiveTextWeight.Bolder,
                Wrap = true
            });

            // Add fact set
            AdaptiveFactSet factSet = new AdaptiveFactSet
            {
                Facts = new List<AdaptiveFact>
        {
            new AdaptiveFact { Title = "Authors", Value = $"{publication.Authors.First}, ..., {publication.Authors.Last}" },
            new AdaptiveFact { Title = "Date", Value = publication.Date.ToString() },
            new AdaptiveFact { Title = "Citations", Value = publication.Citations },
            new AdaptiveFact { Title = "Journal", Value = publication.JournalName },
            new AdaptiveFact { Title = "Impact factor", Value = "9" }
        }
            };
            card.Body.Add(factSet);

            // Add ActionSet with buttons
            var actionSet = new AdaptiveActionSet
            {
                Actions = new List<AdaptiveAction>
        {
            new AdaptiveShowCardAction
            {
                Title = "Abstract",
                IconUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAFYAAABWAHfbyrhAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAAGxQTFRF////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfHZfwQAAACN0Uk5TAAQGBwkKERUXIS0wOTo/SU50fZCWmLvBxNna4uXv8vP1/f44GNoMAAAA+klEQVQ4y4WTyZKCQBBEU0FE2WSz2aSB9///OAcnHMclKi8d0bVXZUoPBEXrBu8H1xaB3hE3Mw/MTfxi3lf+blqW++ur/b/wDlhdmURSlJRuBbqnJOcR6NO/j7QHxvMjfoStDp9ThvUG42+OfQdT/tp0PkF376OCLX8fK9+gkqTYQ/1hbtXgY0kN9OEnh7CHRgpm1lQfka7MgQpw+gIHhVoovzmU0MpBIul02UmSskyStLucJCXgNEAkVSvTUdIVrpKOE2slRTDIs0iHG1BKGUAmlcDtIC1428EsYTZpjmkuyly1eSz73CZhbMqZpLVpbwvHlp4t3u/y/wEDNSwdtLEsJwAAAABJRU5ErkJggg==",
                Card = new AdaptiveCard("1.5")
                {
                    Body = new List<AdaptiveElement>
                    {
                        new AdaptiveTextBlock
                        {
                            Text = publication.Abstract,
                            Wrap = true
                        }
                    }
                }
            },
           new AdaptiveOpenUrlAction
            {
                Title = "See Pubmed",
                Url = new Uri(publication.Link),
                IconUrl = "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIFVwbG9hZGVkIHRvOiBTVkcgUmVwbywgd3d3LnN2Z3JlcG8uY29tLCBHZW5lcmF0b3I6IFNWRyBSZXBvIE1peGVyIFRvb2xzIC0tPg0KPHN2ZyB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4NCjxnIGlkPSJJbnRlcmZhY2UgLyBFeHRlcm5hbF9MaW5rIj4NCjxwYXRoIGlkPSJWZWN0b3IiIGQ9Ik0xMC4wMDAyIDVIOC4yMDAyQzcuMDgwMDkgNSA2LjUxOTYyIDUgNi4wOTE4IDUuMjE3OTlDNS43MTU0NyA1LjQwOTczIDUuNDA5NzMgNS43MTU0NyA1LjIxNzk5IDYuMDkxOEM1IDYuNTE5NjIgNSA3LjA4MDA5IDUgOC4yMDAyVjE1LjgwMDJDNSAxNi45MjAzIDUgMTcuNDgwMSA1LjIxNzk5IDE3LjkwNzlDNS40MDk3MyAxOC4yODQyIDUuNzE1NDcgMTguNTkwNSA2LjA5MTggMTguNzgyMkM2LjUxOTIgMTkgNy4wNzg5OSAxOSA4LjE5NjkxIDE5SDE1LjgwMzFDMTYuOTIxIDE5IDE3LjQ4IDE5IDE3LjkwNzQgMTguNzgyMkMxOC4yODM3IDE4LjU5MDUgMTguNTkwNSAxOC4yODM5IDE4Ljc4MjIgMTcuOTA3NkMxOSAxNy40ODAyIDE5IDE2LjkyMSAxOSAxNS44MDMxVjE0TTIwIDlWNE0yMCA0SDE1TTIwIDRMMTMgMTEiIHN0cm9rZT0iIzAwMDAwMCIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz4NCjwvZz4NCjwvc3ZnPg=="
            },
            new AdaptiveOpenUrlAction
            {
                Title = "Cite",
                IconUrl = "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIFVwbG9hZGVkIHRvOiBTVkcgUmVwbywgd3d3LnN2Z3JlcG8uY29tLCBHZW5lcmF0b3I6IFNWRyBSZXBvIE1peGVyIFRvb2xzIC0tPg0KPHN2ZyB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4NCjxnIGlkPSJJbnRlcmZhY2UgLyBFeHRlcm5hbF9MaW5rIj4NCjxwYXRoIGlkPSJWZWN0b3IiIGQ9Ik0xMC4wMDAyIDVIOC4yMDAyQzcuMDgwMDkgNSA2LjUxOTYyIDUgNi4wOTE4IDUuMjE3OTlDNS43MTU0NyA1LjQwOTczIDUuNDA5NzMgNS43MTU0NyA1LjIxNzk5IDYuMDkxOEM1IDYuNTE5NjIgNSA3LjA4MDA5IDUgOC4yMDAyVjE1LjgwMDJDNSAxNi45MjAzIDUgMTcuNDgwMSA1LjIxNzk5IDE3LjkwNzlDNS40MDk3MyAxOC4yODQyIDUuNzE1NDcgMTguNTkwNSA2LjA5MTggMTguNzgyMkM2LjUxOTIgMTkgNy4wNzg5OSAxOSA4LjE5NjkxIDE5SDE1LjgwMzFDMTYuOTIxIDE5IDE3LjQ4IDE5IDE3LjkwNzQgMTguNzgyMkMxOC4yODM3IDE4LjU5MDUgMTguNTkwNSAxOC4yODM5IDE4Ljc4MjIgMTcuOTA3NkMxOSAxNy40ODAyIDE5IDE2LjkyMSAxOSAxNS44MDMxVjE0TTIwIDlWNE0yMCA0SDE1TTIwIDRMMTMgMTEiIHN0cm9rZT0iIzAwMDAwMCIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz4NCjwvZz4NCjwvc3ZnPg=="
            }
        }
            };

            card.Body.Add(actionSet);

            return card;
        }
    }
}
