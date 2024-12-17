namespace Witivio.Copilot4Researcher.Core
{
    using System.Text;

    /// <summary>
    /// Provides methods for phonetic string matching using the Soundex algorithm
    /// </summary>
    public static class Soundex
    {
        private static readonly Dictionary<char, char> SoundexCodes = new()
        {
            {'B', '1'}, {'F', '1'}, {'P', '1'}, {'V', '1'},
            {'C', '2'}, {'G', '2'}, {'J', '2'}, {'K', '2'}, {'Q', '2'}, {'S', '2'}, {'X', '2'}, {'Z', '2'},
            {'D', '3'}, {'T', '3'},
            {'L', '4'},
            {'M', '5'}, {'N', '5'},
            {'R', '6'}
        };

        /// <summary>
        /// Generates a Soundex code for the given input string
        /// </summary>
        /// <param name="input">The input string to generate a Soundex code for</param>
        /// <returns>A four-character Soundex code, or empty string if input is null or empty</returns>
        public static string Get(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var soundex = new StringBuilder();
            soundex.Append(char.ToUpper(input[0]));

            var previous = '0';
            for (int i = 1; i < input.Length && soundex.Length < 4; i++)
            {
                var current = char.ToUpper(input[i]);
                if (SoundexCodes.TryGetValue(current, out var code) && code != previous)
                {
                    soundex.Append(code);
                    previous = code;
                }
            }

            while (soundex.Length < 4)
                soundex.Append('0');

            return soundex.ToString();
        }

        /// <summary>
        /// Checks if two strings are phonetically similar using Soundex algorithm
        /// </summary>
        /// <param name="str1">First string to compare</param>
        /// <param name="str2">Second string to compare</param>
        /// <returns>True if the strings are phonetically similar, false otherwise</returns>
        public static bool AreSimilar(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                return false;

            return Get(str1) == Get(str2);
        }
    }
}
