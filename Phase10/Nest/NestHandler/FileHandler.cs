using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NestHandler
{
    public class FileHandler
    {
        public static List<T> JsonToList<T>(string path)
        {
            var fileContent = File.ReadAllText(path);
            var personsList = JsonConvert.DeserializeObject<List<T>>(fileContent);
            return personsList;
        }

    }
}