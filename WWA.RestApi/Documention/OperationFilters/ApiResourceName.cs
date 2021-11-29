using Humanizer;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class ApiResourceName
    {
        public string Names { get; }
        public string Name { get; }
        public string Article { get; }
        public bool IsSingular { get; } = false;

        public ApiResourceName(string path)
        {
            IsSingular = path.EndsWith('}');
            var pathValues = path.Split('/');
            var namesPositionFromEnd = IsSingular ? 2 : 1;
            Names =  pathValues[^namesPositionFromEnd];
            Names = Names.Humanize(LetterCasing.Title);
            Name = Names.Singularize();
            Article = Name.GetIndefiniteArticle();
        }

    }

    public static class StringExtensions
    {
        private static readonly HashSet<char> SoundsLikeVowel = new HashSet<char>("AEDHILMNORSX");
        private static readonly HashSet<char> IsVowel = new HashSet<char>("AEIOU");

        // https://stackoverflow.com/questions/4558437/programmatically-determine-whether-to-describe-an-object-with-a-or-an/8044744#8044744
        // This doesn't care about ordinals, so don't use it for that.
        public static string GetIndefiniteArticle(this string phrase)
        {
            // Tokenize all words, if there are no words return 'an'
            var hasWordsMatch = Regex.Match(phrase, @"\w+");
            if (!hasWordsMatch.Success) return "an";

            // Get the first alpha numeric word
            var originalWord = hasWordsMatch.Groups[0].Value;
            var uWord = originalWord.ToUpper();

            // 1 letter words
            if (uWord.Length == 1) return SoundsLikeVowel.Contains(uWord[0]) ? "an" : "a";

            // Consonant starts that require 'an'
            if (new string[] { "EULER", "HEIR", "HONEST", "HONO" }.Any(v => uWord.StartsWith(v)) ||
               (uWord.StartsWith("HOUR") && !uWord.StartsWith("HOURI")))
                return "an";

            // Vowel starts that require 'a'
            if (new string[] { "^E[UW]", "^ONC?E\b", "^UNI([^NMD]|MO)", "^U[BCFHJKQRST][AEIOU]", }.Any(r => Regex.IsMatch(uWord, r))) return "a";

            // Capital starts that require 'an'
            if (Regex.IsMatch(originalWord, "(?!FJO|[HLMNS]Y.|RY[EO]|SQU|(F[LR]?|[HL]|MN?|N|RH?|S[CHKLMNPTVW]?|X(YL)?)[AEIOU])[FHLMNRSX][A-Z]")) return "an";

            // Capital starts that require 'a'
            if (Regex.IsMatch(originalWord, "^U[NK][AIEO]")) return "a";

            // Capital words
            if (originalWord == uWord) return SoundsLikeVowel.Contains(uWord[0]) ? "an" : "a";

            // Starts with a vowel
            if (IsVowel.Contains(uWord[0])) return "an";

            // Y starts that require an 'an'
            if (Regex.IsMatch(uWord, "^Y(B[LOR]|CL[EA]|FERE|GG|P[IOS]|ROU|TT)")) return "an";

            // All other options exausted, guess 'a'
            return "a";
        }
    }
}
