using System.Linq;
using System.Text;

using System.Collections.Generic;
namespace library
{
    public class Result
    {
        public readonly HashSet<Entry> ResultSet;
        public readonly bool IsQuery;

        public Result(HashSet<Entry> ResultSet, bool IsQuery)
        {
            this.ResultSet = ResultSet;
            this.IsQuery = IsQuery;
        }

        public override string ToString()
        {
            var resultStr = new StringBuilder("");
            if (this.ResultSet.Count == 0)
                resultStr.Append("No result found");
            else
            {
                if (this.IsQuery)
                {
                    foreach (Entry entry in ResultSet)
                        resultStr.Append(" " + entry.DocName);
                }
                else
                    resultStr.Append(string.Join(", ", this.ResultSet));
            }
            return resultStr.ToString();
        }
    }
}