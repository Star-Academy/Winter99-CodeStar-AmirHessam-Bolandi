using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace library
{
    public class InvertedIndexSearch
    {
        public Dictionary<string, List<Entry>> IndexMap { get; set; } = new Dictionary<string, List<Entry>>();
        public HashSet<Entry> allDocs { get; set; } = new HashSet<Entry>();
        public static string SPACE_PATTERN1 => SPACE_PATTERN;
        public const string NORMAL_PATTERN = @"\s(\w+)";
        public const string PLUS_PATTERN = @"\+(\w+)";
        public const string MINUS_PATTERN = @"-(\w+)";
        private const string NOT_WORDS_PATTERN = @"[^a-z0-9]";
        private const string SPACE_PATTERN = @"\s+";

        public InvertedIndexSearch(Dictionary<string, string> fileContents)
        {
            foreach (var (id, contnet) in fileContents)
            {
                AddWords(id, GetListOfWords(contnet));
                allDocs.Add(new Entry(id, 0));
            }
        }

        public HashSet<Entry> CombineResults(HashSet<Entry> normalSet, HashSet<Entry> plusSet, HashSet<Entry> minusSet)
        {
            var result = new HashSet<Entry>();
            result.UnionWith(normalSet);
            result.UnionWith(plusSet);
            result.ExceptWith(minusSet);
            return result;
        }

        public List<string> SeperateQuery(string query, string pattern)
        {
            var words = new List<string>();
            Regex regex = new Regex(pattern);
            MatchCollection mathces = regex.Matches(query);
            foreach (Match match in mathces)
                words.Add(match.Groups[1].Value);
            return words;
        }

        public void AddWords(string id, string[] listOfWords)
        {
            for (int i = 0; i < listOfWords.Length; i++)
            {
                string word = listOfWords[i];
                if (StopWordsClass.STOP_WORDS.Contains(word))
                    continue;
                if (IndexMap.ContainsKey(word))
                    IndexMap[word].Add(new Entry(id, i + 1));
                else
                    IndexMap.Add(word, new List<Entry> { new Entry(id, i + 1) });
            }
        }
        public string[] GetListOfWords(string content)
        {
            content = Regex.Replace(content.ToLower(), NOT_WORDS_PATTERN, " ").Trim();
            return Regex.Split(content, SPACE_PATTERN);
        }
        public HashSet<Entry> SearchWord(string word)
        {
            var result = new HashSet<Entry>();
            string wordComplement;
            if (word.EndsWith("s"))
                wordComplement = word.Remove(word.Length - 1);
            else
                wordComplement = word + "s";
            if (IndexMap.ContainsKey(word))
                result.UnionWith(IndexMap[word]);
            if (IndexMap.ContainsKey(wordComplement))
                result.UnionWith(IndexMap[wordComplement]);
            return result;
        }
        public HashSet<Entry> SearchQuery(string query)
        {
            var normalSet = new HashSet<Entry>(allDocs);
            var plusSet = new HashSet<Entry>();
            var minusSet = new HashSet<Entry>();
            foreach (string normalStr in this.SeperateQuery(" " + query, NORMAL_PATTERN))
            {
                var temp = SearchWord(normalStr);
                temp.IntersectWith(normalSet);
                normalSet = temp;
            }
            foreach (string plusStr in this.SeperateQuery(query, PLUS_PATTERN))
                plusSet.UnionWith(SearchWord(plusStr));
            foreach (string minusStr in this.SeperateQuery(query, MINUS_PATTERN))
                minusSet.UnionWith(SearchWord(minusStr));
            return this.CombineResults(normalSet, plusSet, minusSet);
        }

        public Result Search(string input)
        {
            var multiWords = input.Contains(" ") || input.Contains("+") || input.Contains("-");
            return new Result(this.SearchQuery(input), multiWords);
        }
    }
}