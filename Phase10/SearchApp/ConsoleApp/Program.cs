using System;
using SearchLibrary;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static SearchEngine searchEngine;

        static void Main(string[] args)
        {
            Console.WriteLine("Is the documents index created? (y/n)");
            var createdIndex = Console.ReadLine().ToLower();
            searchEngine = new SearchEngine(new Uri("http://localhost:9200"), createdIndex[0] == 'y');

            Console.WriteLine("Do you want to post new documents? (path/n) ");
            var newDocumentPath = Console.ReadLine().ToLower();
            //Path = "D:\\temp\\SearchApp\\Data\\EnglishData";
            FileReader fr = new FileReader(newDocumentPath);
            foreach (var x in fr.ReadContent())
            {
                Console.WriteLine(x.DocumentId);
            }

            if (newDocumentPath != "n")
                searchEngine.PostDocuments(newDocumentPath);
        }
    }
}