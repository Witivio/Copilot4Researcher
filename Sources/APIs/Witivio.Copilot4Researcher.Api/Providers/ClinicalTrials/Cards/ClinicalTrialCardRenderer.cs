using AdaptiveCards;
using CsvHelper;
using System.Collections.Generic;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.ClinicalTrials.Cards
{
    public class ClinicalTrialCardRenderer
    {
        private const string GREY_CIRCLE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIgSURBVFhHxZc7LwRRGIbXLUGBRqWwBYkEMZGQoNpmI0RBIaLaf6BEREOzNX9gCwnZQrVbKXQSicgULhENFYmIS+KSUHjemVn2mh3M5UmePWdOZL93jplzztZEXJJIJFppJjCGgxjFNhSPeIXHuI/ZVCr1TFuVqgEo3E2ziPPYpDEXvOE2JglyaY1UoGIACqvYGi5gg8b+wAdu4CpBFKqEsgEo3kWzi/3WwP85wZlys1ESgOIDNHvYbg14xx3GCWHalzYFAZw7P0Cvi+dQiLH8mfgO4PzPD9Graa/EKQ4T4lUXtfpw0APnd3HRi+t215kB7l6vmpL99Wn/LZ/Yxyxc5GZA73lQxUU9LqlTw9230N6i20XGK7QudGgGJjHo4kI1JxVAa3tYxBRAG0tYGAqgXS0sogqQ21LDoE0Bym5IQaEAOkyExaMC6CQTFtcKoGNUWJgKoDNcWOwrQBbLHpd85h0ztexIT3R2rKFgSVP7QTMgkqgtMihUSzUjdfowTfPeMAwtSCO6DoBN7n5LndwMiFXUocRvznHF7uYFcM5o03hvDfiDvnuaWi/2ZeEMKIROq3H0I4S+c5waF/alTUEAwR9oYRrFM2vAGzTtOo4f2Zc/WA9hMc5DmaLbjENYEtQleto3cY7iN9ZIEVV3Qs6MPTTLOIuNGnOBFpk06sep7r4irrdigug1nUId4QzsxPyf59eon11a2jMUfqCtQiTyBSfli6ZocSstAAAAAElFTkSuQmCC";

        private const string GREEN_CRICLE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIgSURBVFhHxZe/S5RxHMdPMchFXZocclAQKpLABhtCBxEPBxsa2psbK8KlFrdIcOoPKByadAqUlkAQcSjjcvG2IEIL/AE19Ho933vqrrvjnvT58YKX3+f5cnef9/P1uef7ua5SQu6/W+9nmMVJvIFDOIByiPu4jRu49vL21A/GjnQMQOERhod4D3udS8AJvsJFguxFM21oG4DCFnuKD/CCc2fgJy7hAkEM1UTLABQfZniD16KJ8/MB77RajaYAFL/O8BYvRRPp8RWnCbETTgMNAWpX/h7TLh5jiFv1K/EnQO1/volpLXs7PuJNQhx70u2fGt5wWReXK/gsHNZWgKv3q2ays97t/8svvMoqVOIV8HueV3HpwUcedHH1fYxfMOlDJi18Lgy6AmXMu7hYs2wAn+1FMWkAN5aiGDOAu1pRDBkg3lKLYMAALTekvDCAzURRHBrATqYoqgawjSqKHQPYwxXFhgHWsGW7lDGnuNrNjvSdg9fRVL6sUPvAFZBFdIvMC2tZMzQkJPnMYPeaF8vU/ORBvAKygDYlWWPhJ+GwLgCJ7NHm8Vs0kQ1+9jy1jsJp4woYwm51GrMI4WfOUKMSTgMNAYQX+GCawN1oIh1cdtvxrXD6l6YAwgu9KcfxOZ7n2+F7X+D4v1ce03EnpGccZXiMd/GicwnwIbOC/jiN7vZ2JN6KCWLfMIe2cGN4Get/nlfRn10+2lcpfMDYgVLpN/TngfyhnJCgAAAAAElFTkSuQmCC";

        private const string BLUE_CRICLE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAITSURBVFhHxZe9LwNhHMdLSFjoYpE0DCQShEgYmCwiXgYGg61/gRGRLiw2ITHfSAwmJoNNIhExeAkWcotEpCXxkjD4fO/ppa226eFePsnn6T2X6O97j+s9v6uKecWyGxnHcBj7sBXjKDJ4h6d4iPuxZOKFz4pUDmDZ7YzzOIv1OuWBd9zCVYLcOmfKUD6AZavYMs5hrU79gU/cwBRBFKqI0gEsu41xF7ud+f85x+lSq1EcwLJ7GA+wyZn7xyOOEOLMTA2FAcyVH6HfxV0UYih/JXIBzP/8GP1a9nJc4AAh3jSp1pBFN1zQxUUnrphDdwXMT03J/nq3/5Yv7GIVrt0V0O88rOKiBhd0UMXVN/D5gF4fMn6h50KzVmAcwy4uVHNCAfRsj4phBdDGEhW9CqBdLSpaFcDdUqMgrgClN6SQUAA1E1GRUQB1MlFxrwBqo6LiTAHUw0XFoQLsY8l2KWA+cK+aHemZg23nVLjsUDutFRCrqC0yLFRLNbMNSTJxw6juNSw2qXmlA3cFRArVlASNCi+Zw/wApkebwidnHgz67ilqvZpp4QoohLrVEQwihL5zlBrXZmooDCCSCT2YBvHSmfuDll3t+ImZ5igOIMxN2Y9r+J9fh/52Hft/XrlL5Z3QsjsYF3EG63TKA3rI7KBeTp27vRzet2LLVt8wiWrherEF81/P71GvXXq071E4zWcFYrFvIsVwMjy5h0IAAAAASUVORK5CYII=";

        private const string BLUE_BG = "data:image/gif;base64,R0lGODlhCAABAHAAACwAAAAACAABAIGZzP8AAAAAAAAAAAACA4RvBQA7";

        private const string GREEN_BG = "data:image/gif;base64,R0lGODdhCAABAIAAAJHOgQAAACwAAAAACAABAAACA4RvBQA7";

        private const string DARK_GREEN_BG = "data:image/gif;base64,R0lGODlhCAABAHAAACwAAAAACAABAIGZzP8AAAAAAAAAAAACA4RvBQA7";

        private const string YELLOW_BG = "data:image/gif;base64,R0lGODlhCAABAHAAACwAAAAACAABAIGZzP8AAAAAAAAAAAACA4RvBQA7";

        private const string BROWNISH_BG = "data:image/gif;base64,R0lGODlhCAABAHAAACwAAAAACAABAIGZzP8AAAAAAAAAAAACA4RvBQA7";

        private const string CONTACTIcon = "data:image/gif;base64,R0lGODlhIwAgAHAAACH5BAEAAIUALAAAAAAjACAAh2xmZU5GRVdQT4iDg1JLSiUcGjszMVtUVFFKSCYdGyYcGkE5NyohH3RvbSceHH14d4yIiI2JiJaSkSsiIHt2dDUsKyQaGCkgHigeHSUaGTcvLSQaGZWRkFtVU1NMSyUbGiMZFyogHndycW1oZyUbGWZfXjgwLi4lI3BqaUhBP0tDQjAmJIB8e3NubDoxMH55eSIXFpGOjCwjIZGOjSQZGE9JRzAnJYV/fnZxcTYtK46KiUtDQ2JcWz83NTUtK2ljYoJ9fC4kIomEg3l0cyUaGCoiIG9paISAf3Jsa2hjYZCMi4F9e19YV4aDgiQZFzoxL4WBf4uGhjUrKiccGiYbGUY+PY6JiYeDgmlkYzIpJ3p1dF9ZWJiUkzgvLVROTDIqKG5oZygeHGBaWCcdG0M7O1ROTU1FRFZPTjQsKkI6OXFraiwiICshH11XVnp2dJCLioaAf4R+fEA5N2RdXCgdG4yIh0xEQ2BZWEU9PGFZWF9WVV9XVl9XVV5WVWBYWEtDQWJbWklBP0lAP01FQ0tCQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAj/AAsJHEiwoMGDCBMqXMiwIIAAAgY0nCiQQAEDBxAkUECR4QIGBRs46JjwAUeDECJIIGlwAgWDFSxcwJBBA8uBGwpysNCBoIcPNwuBKBhChMERJG5aIFjCBMITKFimUDFwBQuELVwofSEQRsIYMm7OoFGjkI0bCHHkCKpjRyEePRD6+MESSBCnAkkIMTiECMsiFowQPEICCcEkIJR0XGKBycEmIJw8eWIhaUcoTqIcRCHFgoUpVJxYqGKFYoYrBrGQyKLF4JYCGLgw7OLF4JelCsGEEbNwjEEMZCYmKJPQzJmCaNJ0tKAGYYGCKBKw9HvQ8kALiklOPQhizQo2bdxMQwjq2+AbOHDiFJIzJyidOgtP3rRzZyEIPHn06Nezh8/+/3rw0Yceffixxh8LARKIIAwyOAghDUYoIYMRBGXhhRheGBAAOw==";

        private static readonly string[] BackgroundsCircle = { GREY_CIRCLE, GREEN_CRICLE, BLUE_CRICLE };

        public static AdaptiveCard Render(ClinicalTrial trial)
        {
            var card = new AdaptiveCard("1.6")
            {
                Body = new List<AdaptiveElement>
            {
                new AdaptiveContainer
                {
                    Style = AdaptiveContainerStyle.Emphasis,
                    Items = new List<AdaptiveElement>
                    {
                        new AdaptiveColumnSet
                        {
                            Columns = new List<AdaptiveColumn>
                            {
                                new AdaptiveColumn
                                {
                                    Width = "stretch",
                                    VerticalContentAlignment = AdaptiveVerticalContentAlignment.Center,
                                    Items = new List<AdaptiveElement>
                                    {
                                        new AdaptiveTextBlock
                                        {
                                            Size = AdaptiveTextSize.Large,
                                            Weight = AdaptiveTextWeight.Bolder,
                                            Text = "STATUS",
                                            Wrap = true,
                                            Style = AdaptiveTextBlockStyle.Heading
                                        }
                                    }
                                },
                                new AdaptiveColumn
                                {
                                    Width = "auto",
                                    Items = new List<AdaptiveElement>
                                    {
                                        new AdaptiveContainer
                                        {
                                            BackgroundImage = new AdaptiveBackgroundImage
                                            {
                                                Url =  GetStatusBackground(trial.Status),
                                                FillMode = AdaptiveImageFillMode.Repeat
                                            },
                                            Items = new List<AdaptiveElement>
                                            {
                                                new AdaptiveTextBlock
                                                {
                                                    Text = trial.Status,
                                                    Wrap = true,
                                                    Style = AdaptiveTextBlockStyle.Heading
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Bleed = true
                },
                new AdaptiveTextBlock
                {
                    Text = trial.Title,
                    Wrap = true,
                    Style = AdaptiveTextBlockStyle.Heading
                },
                new AdaptiveFactSet
                {
                    Facts = GetFacts(trial),
                    Separator = true
                }
            },
                Actions = new List<AdaptiveAction>
                    {
                        new AdaptiveOpenUrlAction
                        {
                            Title = "See detail",
                            IconUrl = "data:image/gif;base64,R0lGODlhIAAfAHAAACH5BAEAAEAALAAAAAAgAB8AhjMzMzAwMDExMTc3NwQEBAAAABgYGGBgYCEhIQ4ODggICAUFBSIiIhUVFQwMDBQUFHJycmlpaWRkZGpqaldXVwMDAw0NDSYmJgEBAVJSUktLS0ZGRnZ2dhoaGkVFRSMjI2FhYUJCQl1dXS8vLz8/Px0dHS4uLjw8PB4eHjk5OSAgIAsLC19fXzQ0NHFxcXh4eCQkJHV1dWdnZ3BwcBcXF21tbScnJwICAllZWQcHBxsbGx8fHw8PD0FBQYSEhBMTEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAf/gECCg4SFhoeIiYcAAQKOjwEDipMEBZaXmAaFBwgJCp8KCwyGDZimBQ6ED6eXEIYREhITFBWYFoMXlxgYmAuTGaa3grwFGpOGG7qWwhyWHceFHpcfIMuCEpYA0IMhl89A1kAiliPbQCSXJYPh4wUm2yeXKITslu/HKZcqhZYrgu33FA0owGtfIRQVWPyzN6kFwQIIjgFU5OLhB2gTE72wBGNbxkQxZJj7aE4RyZKCZhQ6WZJGAV+DWJq7REjmNpoxGaJcZ6mmzp3geg5ikQ2ooFoFCNWwZMMokBsFbvCLahSHpRyFcljCQAOFjq9gw4rdweNSj0I+iLFae8rfoR9qCNmyqhDAaclAADs=",
                            Url = new Uri(trial.Link)
                        }
                    }
            };

            if (trial.Contacts != null && trial.Contacts.Any())
            {
                card.Actions.Insert(0, new AdaptiveShowCardAction
                {
                    Title = "Contacts",
                    IconUrl = CONTACTIcon,
                    Card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 5))
                    {
                        Body = GenerateContactsColumns(trial.Contacts)
                    }
                });
            }

            return card;
        }

        private static List<AdaptiveFact> GetFacts(ClinicalTrial trial)
        {
            var facts = new List<AdaptiveFact>();

            if (trial.Conditions != null && trial.Conditions.Any())
            {
                facts.Add(new AdaptiveFact("Conditions:", string.Join(", ", trial.Conditions)));
            }

            if (!string.IsNullOrEmpty(trial.SponsorAffiliation))
            {
                facts.Add(new AdaptiveFact("Sponsor:", trial.SponsorAffiliation));
            }

            if (!string.IsNullOrEmpty(trial.StartDate))
            {
                facts.Add(new AdaptiveFact("Study start:", trial.StartDate));
            }

            if (!string.IsNullOrEmpty(trial.CompletionDate))
            {
                facts.Add(new AdaptiveFact("Study completion:", trial.CompletionDate));
            }

            if (trial.EnrollmentCount != 0)
            {
                facts.Add(new AdaptiveFact("Enrolment:", trial.EnrollmentCount.ToString()));
            }

            if (trial.Phases != null && trial.Phases.Any())
            {
                facts.Add(new AdaptiveFact("Phases:", string.Join(", ", trial.Phases)));
            }

            if (trial.Interventions != null && trial.Interventions.Any())
            {
                facts.Add(new AdaptiveFact("Interventions:", string.Join(", ", trial.Interventions.Select(c => c.Type + ": " + c.Name))));
            }

            return facts;
        }

        private static List<AdaptiveElement> GenerateContactsColumns(List<Models.ClinicalTrialContact> contacts)
        {
            var contactColumns = new List<AdaptiveElement>();

            foreach (var contact in contacts)
            {
                var contactColumnSet = new AdaptiveColumnSet
                {
                    Columns = new List<AdaptiveColumn>
                {
                    new AdaptiveColumn
                    {
                        Width = "auto",
                        Items = new List<AdaptiveElement>
                        {
                            new AdaptiveContainer
                            {
                                BackgroundImage = new AdaptiveBackgroundImage
                                {
                                    Url = GetRandomPeopleBackground(),
                                    FillMode = AdaptiveImageFillMode.Cover
                                },
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = GenerateInitals(contact.Name),
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Color = AdaptiveTextColor.Light
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveColumn
                    {
                        Width = "stretch",
                        Items = GetContactItems(contact)
                    }
                }
                };

                contactColumns.Add(contactColumnSet);
            }

            return contactColumns;
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
        private static Uri GetStatusBackground(string status)
        {
            switch (status)
            {
                case "RECRUITING":
                    return new Uri(GREEN_BG);
                case "COMPLETED":
                    return new Uri(BLUE_BG);
                case "ACTIVE":
                    return new Uri(DARK_GREEN_BG);
                case "NOT YET RECRUITING":
                    return new Uri(YELLOW_BG);
                default:
                    return new Uri(BROWNISH_BG);
            }
        }
    
    

        private static Uri GetRandomPeopleBackground()
        {
            var random = new Random();
            int index = random.Next(BackgroundsCircle.Length);
            return new Uri(BackgroundsCircle[index]);
        }

        private static List<AdaptiveElement> GetContactItems(ClinicalTrialContact contact)
        {
            var items = new List<AdaptiveElement>();

            if (!string.IsNullOrWhiteSpace(contact.Name))
            {
                items.Add(new AdaptiveTextBlock
                {
                    Text = contact.Name,
                    Weight = AdaptiveTextWeight.Bolder,
                    Wrap = true
                });
            }

            if (!string.IsNullOrWhiteSpace(contact.Email))
            {
                items.Add(new AdaptiveTextBlock
                {
                    Text = contact.Email,
                    IsSubtle = true,
                    Wrap = true
                });
            }

            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                items.Add(new AdaptiveTextBlock
                {
                    Text = contact.Phone,
                    IsSubtle = true,
                    Wrap = true
                });
            }

            var actions = new List<AdaptiveAction>();
            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                actions.Add(new AdaptiveOpenUrlAction
                {
                    Title = "📞",
                    Url = new Uri("tel:" + contact.Phone)
                });
            }

            if (!string.IsNullOrWhiteSpace(contact.Email))
            {
                actions.Add(new AdaptiveOpenUrlAction
                {
                    Title = "📧",
                    Url = new Uri("mailto:" + contact.Email)
                });
            }

            if (actions.Any())
            {
                items.Add(new AdaptiveActionSet
                {
                    Actions = actions
                });
            }

            return items;
        }
    }
}
