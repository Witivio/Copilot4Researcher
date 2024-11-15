using AdaptiveCards;
using CsvHelper;
using System.Collections.Generic;
using Witivio.Copilot4Researcher.Common;
using Witivio.Copilot4Researcher.Models;

namespace Witivio.Copilot4Researcher.Providers.ClinicalTrials.Cards
{
    public class ClinicalTrialCardRenderer
    {
        private static readonly string[] BackgroundsCircle = { CardIcons.GreyCircle, CardIcons.GreenCircle, CardIcons.BlueCircle };

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
                                                    Text = trial.Status.Replace("_", " "),
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
                            IconUrl = CardIcons.ExternalLink,
                            Url = new Uri(trial.Link)
                        }
                    }
            };

            if (trial.Contacts != null && trial.Contacts.Any())
            {
                card.Actions.Insert(0, new AdaptiveShowCardAction
                {
                    Title = "Contacts",
                    IconUrl = CardIcons.Contact,
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
                case "COMPLETED":
                    return new Uri(CardIcons.BlueBg);
                case "ACTIVE":
                    return new Uri(CardIcons.DarkGreenBg);
                case "NOT_YET_RECRUITING":
                    return new Uri(CardIcons.YellowBg);
                default:
                    return new Uri(CardIcons.BrownishBg);
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
