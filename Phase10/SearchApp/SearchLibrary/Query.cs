using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SearchLibrary
{
    public class Query
    {
        public const string NORMAL_PATTERN = @"\s(\w+)";
        public const string PLUS_PATTERN = @"\+(\w+)";
        public const string MINUS_PATTERN = @"-(\w+)";
        

        public List<string> Normals { get; }
        public List<string> Pluses { get; }
        public List<string> Minuses { get; }
        public Query(string query){ 
            Normals = FindPattern(" "+query , NORMAL_PATTERN);
            Minuses = FindPattern(query , MINUS_PATTERN);
            Pluses = FindPattern(query , PLUS_PATTERN);   
        }
        
        public static List<string> FindPattern(string query, string pattern)
        {
            var words = new List<string>();
            Regex regex = new Regex(pattern);
            MatchCollection mathces = regex.Matches(query);
            foreach (Match match in mathces)
                words.Add(match.Groups[1].Value);
            return words;
        }
    }
}