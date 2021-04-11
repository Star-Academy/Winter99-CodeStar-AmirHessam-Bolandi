using System.Collections.Generic;

namespace SearchLibrary
{
    public class Entry
    {
        public int EntryId { get; set; }
        public string DocumentName { get; set; }
        public int Index { get; set; }

        public ICollection<IndexMap> IndexMap { get; set; }

        public override string ToString()
        {
            return "Document Id: " + this.DocumentName + ", Index: " + this.Index;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.DocumentName.Equals(((Entry)obj).DocumentName);
        }

        public override int GetHashCode()
        {
            return this.DocumentName.GetHashCode();
        }
    }
}