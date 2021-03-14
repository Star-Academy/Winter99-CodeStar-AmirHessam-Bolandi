using System.Linq;
using System.Text;

using System.Collections.Generic;
namespace library
{
    public class Result
    {
        public virtual HashSet<Entry> ResultSet { get; set; }
        public bool isQuery { get; set; }

        public Result(HashSet<Entry> ResultSet, bool isQuery)
        {
            this.ResultSet = ResultSet;
            this.isQuery = isQuery;
        }
        public Result() { }

        public override string ToString()
        {
            var resultStr = new StringBuilder("");
            if (this.ResultSet.Count == 0)
                resultStr.Append("No result found");
            else
            {
                if (this.isQuery)
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