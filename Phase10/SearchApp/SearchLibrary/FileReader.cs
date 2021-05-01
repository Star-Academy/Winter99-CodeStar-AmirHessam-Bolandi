using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace SearchLibrary
{
    public class FileReader
    {
        public string path { get; set; }
        public FileReader(string path)
        {
            this.path = path;
        }

        public List<Document> ReadContent()
        {
            var filesContent = new List<Document>();
            System.Array.ForEach(this.GetFiles(), file => filesContent.Add(new Document(file.Split("\\").Last(), File.ReadAllText(file).Trim())));
            return filesContent;
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(path);
        }
    }
}