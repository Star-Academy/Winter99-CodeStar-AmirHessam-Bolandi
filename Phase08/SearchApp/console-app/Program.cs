using System.ComponentModel.Design.Serialization;
using System;
using System.IO;
using System.Linq;
using SearchLibrary;
using System.Collections.Generic;

namespace console_app
{
    class Program
    {
        private const string WELCOME_MASSAGE = "Enter search word or enter 'exit()' to end: ";
        private const string EXIT_COMMAND = "exit()";
        private const string SERVER = "localhost";

        static void Main(string[] args)
        { 
            var invertedIndexSearch = new InvertedIndexSearch(new SearchContext(SERVER), false);
            while (true)
            {
                System.Console.WriteLine((WELCOME_MASSAGE));
                var inputString = System.Console.ReadLine();
                if (inputString.Equals(EXIT_COMMAND))
                    break;
                System.Console.WriteLine(invertedIndexSearch.Search(inputString));
            }
        }
    }
}
