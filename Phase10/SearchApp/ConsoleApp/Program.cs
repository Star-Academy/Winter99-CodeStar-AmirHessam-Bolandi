using System;
using SearchLibrary;

namespace ConsoleApp
{
    class Program
    {
        private static SearchEngine searchEngine;
        private const string ELASTIC_URI = "http://localhost:9200";
        private const string INDEX_NAME = "documents";

        static void Main(string[] args)
        {
            Initialize();
            PostNewData();
            GetQuery();
        }

        public static void Initialize()
        {
            Console.WriteLine("Is the documents index created? (y/n)");
            var createdIndex = Console.ReadLine().ToLower();
            try
            {
                searchEngine = new SearchEngine(INDEX_NAME, new Uri(ELASTIC_URI), createdIndex[0] == 'y');
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType()} \n {e.Message} \n {e.StackTrace}");
                return;
            }
        }

        public static void PostNewData()
        {
            while (true)
            {
                Console.WriteLine("Do you want to post new documents? (path/n) ");
                var newDocumentPath = Console.ReadLine().ToLower();
                if (newDocumentPath == "n")
                    break;
                try
                {
                    searchEngine.PostDocuments(newDocumentPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.GetType()} \n {e.Message} \n {e.StackTrace}");
                }
            }
        }

        public static void GetQuery()
        {
            while (true)
            {
                Console.WriteLine("Enter your query or exit() to end.");
                var command = Console.ReadLine();
                if(command=="exit()")
                    break;
                var query = new Query(command);
                var responseList = searchEngine.Search(query.Normals, query.Pluses, query.Minuses);
                Console.WriteLine($"Results:\n Docuemtn Ids: \n {string.Join(", ", responseList)}\n ");
            }
        }
    }
}
