using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SearchLibrary
{
    public class Query
    {
        public const string NormalPattern = @"\s(\w+)";
        public const string PlusPattern = @"\+(\w+)";
        public const string MinusPattern = @"-(\w+)";
        

        public List<string> Normals { get; }
        public List<string> Pluses { get; }
        public List<string> Minuses { get; }
        public Query(string query){ 
            Normals = FindPattern(" "+query , NormalPattern);
            Minuses = FindPattern(query , MinusPattern);
            Pluses = FindPattern(query , PlusPattern);   
        }
        public Query(string normals ,string pluses,string minuses)
        {
            Normals = normals.Split(" ").ToList();
            Minuses = minuses.Split(" ").ToList();
            Pluses = pluses.Split(" ").ToList();   
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