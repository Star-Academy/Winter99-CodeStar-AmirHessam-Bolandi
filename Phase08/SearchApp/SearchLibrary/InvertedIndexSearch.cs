using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SearchLibrary
{
    public class InvertedIndexSearch
    {
        public const string NORMAL_PATTERN = @"\s(\w+)";
        public const string PLUS_PATTERN = @"\+(\w+)";
        public const string MINUS_PATTERN = @"-(\w+)";
        private const string NOT_WORDS_PATTERN = @"[^a-z0-9]";
        private const string SPACE_PATTERN = @"\s+";
        private SearchContext Context { get; }

        public InvertedIndexSearch(SearchContext Context, bool createMap)
        {
            this.Context = Context;
            if (createMap)
                CreateIndexMap();
        }

        public void CreateIndexMap()
        {
            foreach (var document in Context.Documents.ToList())
                AddWords(document.DocumentName, GetListOfWords(document.Content));
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

        public void AddWords(string name, string[] listOfWords)
        {
            for (int i = 0; i < listOfWords.Length; i++)
            {
                string word = listOfWords[i];
                if (StopWordsClass.STOP_WORDS.Contains(word))
                    continue;
                Context.IndexMaps.Add(new IndexMap()
                {
                    Entry = new Entry()
                    {
                        DocumentName = name,
                        Index = i + 1
                    },
                    Word = word
                });
            }
            Context.SaveChanges();
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
            result.UnionWith(GetWordEntries(word));
            result.UnionWith(GetWordEntries(wordComplement));
            return result;
        }

        public List<Entry> GetWordEntries(string word)
        {
            return Context.IndexMaps.Include(x => x.Entry).Where(x => x.Word == word).Select(x => x.Entry).ToList();
        }

        public List<Entry> GetAllDocsName()
        {
            return Context.Entries.GroupBy(x => x.DocumentName).Select(group => new Entry
            {
                DocumentName = group.Key,
                Index = 0
            }).ToList();
        }
        
        public HashSet<Entry> SearchQuery(string query)
        {
            var normalSet = new HashSet<Entry>(GetAllDocsName());
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