using System.Collections.Generic;
using System.IO;
using library;

namespace console_app
{
    class Program
    {
        private const string DATA_PATH = "data//EnglishData";
        private const string WELCOME_MASSAGE = "Enter search word or enter 'exit()' to end: ";
        private const string EXIT_COMMAND = "exit()";
        static void Main(string[] args)
        {
            var fileReader = new FileReader(DATA_PATH);
            var fileContents = fileReader.ReadContent();
            var invertedIndexSearch = new InvertedIndexSearch(fileContents);
            string inputString = "";
            while (true)
            {
                System.Console.WriteLine((WELCOME_MASSAGE));
                inputString = System.Console.ReadLine();
                if (inputString.Equals(EXIT_COMMAND))
                    break;
                System.Console.WriteLine(invertedIndexSearch.Search(inputString));
            }
        }
    }
}

