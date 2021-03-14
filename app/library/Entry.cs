using System;

namespace library
{
    public class Entry
    {
        public string DocName { get; set; }
        public int Index { get; set; }
        public Entry(string docName, int index)
        {
            DocName = docName;
            Index = index;
        }

        public override string ToString()
        {
            return "Document Id: " + this.DocName + ", Index: " + this.Index;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.DocName.Equals(((Entry)obj).DocName);
        }

        public override int GetHashCode()
        {
            return this.DocName.GetHashCode();
        }
    }
}
