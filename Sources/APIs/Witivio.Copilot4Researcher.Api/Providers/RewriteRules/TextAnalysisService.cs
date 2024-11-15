using ClosedXML.Excel;
using Microsoft.Graph;
using System.Text;
using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Api.Providers
{
    public interface IRewriteRulesService
    {
        Task<Rule> GetRulesAsync(string journalName, JournalType journalType);
    }

    public class RewriteRulesSharePointService : IRewriteRulesService
    {
        private readonly GraphServiceClient _graphClient;

        public RewriteRulesSharePointService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

       
        public async Task<Rule> GetRulesAsync(string journalName, JournalType journalType)
        {
            try
            {
                // Get SharePoint list items (now using injected client)
                var listItems = await _graphClient.Sites["{site-id}"].Lists["{list-id}"].Items
                    .GetAsync((requestConfiguration) =>
                   {
                       requestConfiguration.QueryParameters.Expand = new string[] { "fields($select=Name,Color,Quantity)" };
                   });

                if (listItems?.Value == null)
                    return null;

                // Convert journal name to Soundex code for comparison
                var searchSoundex = GetSoundex(journalName);

                // Search through list items
                var bestMatch = listItems.Value
                    .Select(item => new
                    {
                        JournalName = item.Fields.AdditionalData["Title"]?.ToString(),
                        MaxWord = int.Parse(item.Fields.AdditionalData["MaxWords"]?.ToString() ?? "0"),
                        MaxChar = int.Parse(item.Fields.AdditionalData["MaxChars"]?.ToString() ?? "0"),
                        JournalType = item.Fields.AdditionalData["JournalType"]?.ToString()
                    })
                    .Where(item => !string.IsNullOrEmpty(item.JournalName) && 
                                  item.JournalType?.Equals(journalType.ToString(), StringComparison.OrdinalIgnoreCase) == true)
                    .Select(item => new
                    {
                        Item = item,
                        SoundexCode = GetSoundex(item.JournalName),
                        LevenshteinDist = LevenshteinDistance(journalName.ToLower(), item.JournalName.ToLower())
                    })
                    .OrderBy(x => x.LevenshteinDist)
                    .FirstOrDefault();

                if (bestMatch == null)
                    return null;

               return new Rule
               {
                CharacterCount = bestMatch.Item.MaxChar.ToString(),
                WordCount = bestMatch.Item.MaxWord.ToString()
               };
            }
            catch (Exception ex)
            {
                // Log exception
                return null;
            }

        }

        private string GetSoundex(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            var soundex = new StringBuilder();
            soundex.Append(char.ToUpper(str[0]));

            var codes = new Dictionary<char, char>
            {
                {'B', '1'}, {'F', '1'}, {'P', '1'}, {'V', '1'},
                {'C', '2'}, {'G', '2'}, {'J', '2'}, {'K', '2'}, {'Q', '2'}, {'S', '2'}, {'X', '2'}, {'Z', '2'},
                {'D', '3'}, {'T', '3'},
                {'L', '4'},
                {'M', '5'}, {'N', '5'},
                {'R', '6'}
            };

            var previous = '0';
            for (int i = 1; i < str.Length && soundex.Length < 4; i++)
            {
                var current = char.ToUpper(str[i]);
                if (codes.ContainsKey(current) && codes[current] != previous)
                {
                    soundex.Append(codes[current]);
                    previous = codes[current];
                }
            }

            while (soundex.Length < 4) soundex.Append('0');
            return soundex.ToString();
        }

        private int LevenshteinDistance(string s1, string s2)
        {
            var costs = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++) costs[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) costs[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    if (s1[i - 1] == s2[j - 1])
                        costs[i, j] = costs[i - 1, j - 1];
                    else
                        costs[i, j] = Math.Min(
                            Math.Min(costs[i - 1, j], costs[i, j - 1]) + 1,
                            costs[i - 1, j - 1] + 1
                        );
                }
            }

            return costs[s1.Length, s2.Length];
        }
    }

    public class Rule
    {
        public string CharacterCount { get; set; }
        
        public string WordCount { get; set; }
    }

     public enum JournalType
        {
            Article,
            Review
        }


    public class TextAnalysisResponse
    {
        public int CharacterCount { get; set; }
        public int WordCount { get; set; }
        public string OriginalText { get; set; }
        public string JournalName { get; set; }
    }
} 