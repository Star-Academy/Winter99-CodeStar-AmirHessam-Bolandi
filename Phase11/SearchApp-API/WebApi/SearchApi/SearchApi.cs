using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchApi.Controllers;
using SearchLibrary;

namespace SearchApi
{
    public class SearchApi
    {
        private static SearchEngine searchEngine;
        private const string ElasticUri = "http://localhost:9200";
        private const string IndexName = "documents";


        public static string Initialize(bool isCreated)
        {
            try
            {
                searchEngine = new SearchEngine(IndexName, new Uri(ElasticUri), isCreated);
                return "initialized";
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType()} \n {e.Message} \n {e.StackTrace}");
            }

            return null;
        }

        public static string PostNewData(string newDocumentPath)
        {
            try
            {
                searchEngine.PostDocuments(newDocumentPath);
                return "documents are added";
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType()} \n {e.Message} \n {e.StackTrace}");
            }
            return null;
        }


        public static List<string> GetQuery(string queryPhrase)
        {
            var query = new Query(queryPhrase);
            var responseList = searchEngine.Search(query.Normals, query.Pluses, query.Minuses);
            return responseList;
        }
        
        public static List<string> GetQuery(string normals ,string pluses,string minuses)
        {
            var query = new Query(normals ,pluses,minuses);
            var responseList = searchEngine.Search(query.Normals, query.Pluses, query.Minuses);
            return responseList;
        }

        public static string GetFileContent(string fileName)
        {
            var responseList = searchEngine.GetDocuments(fileName);
            return responseList;
        }
    }
}