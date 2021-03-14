using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace library
{
    public class FileReader
    {
        public string path { get; set; }
        public FileReader(string path)
        {
            this.path = path;
        }

        public Dictionary<string, string> ReadContent()
        {
            var filesContent = new Dictionary<string, string>();
            System.Array.ForEach(this.GetFiles(), file => filesContent.Add(file.Split("\\").Last(), File.ReadAllText(file).Trim()));
            return filesContent;
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(path);
        }
    }
}