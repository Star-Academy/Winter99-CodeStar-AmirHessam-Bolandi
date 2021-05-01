using System;
using System.Linq;
using System.Text.Json.Serialization;
using Elasticsearch.Net;
using Nest;
using NestHandler;

namespace people
{
    class Program
    {
        static String localHost = "http://localhost:9200";
        private static Uri uri = new Uri(localHost);

        static void Main(string[] args)
        {
            var personsList = FileHandler.JsonToList<Person>("..\\..\\..\\people_details.json");
            var client = MakeClient(uri);

            // var responsePing = client.Ping();
            // Console.WriteLine(responsePing);

            IndexHandler indexHandler = new IndexHandler("my-indexx", MakeClient(uri));
            
            indexHandler.MakeIndex<Person>(
                null,
                map => map
                    .Properties(pr => pr
                        .Text(n => n
                            .Name("email").Analyzer("my-email-analyzer")
                        )
                        .GeoPoint(g => g
                            .Name("location")))
            );

            indexHandler.BulkIndex(personsList);
            
            ISearchResponse<Person> response = null;
/*
            response = indexHandler.GetResponseOfQuery<Person>(IndexHandler.MakeMatchQuery("Hunbe", "name", 3));
            IndexHandler.QueryResponsePrinter("Match", response);

            response = indexHandler.GetResponseOfQuery<Person>(IndexHandler.MakeFuzzyQuery("ros", "name", 3));
            IndexHandler.QueryResponsePrinter("fuzzy ", response);

            response = indexHandler.GetResponseOfQuery<Person>(
                IndexHandler.MakeMultiMatchQuery("culpa", new string[] {"name", "about"}, 3));
            IndexHandler.QueryResponsePrinter("MultiMatch ", response);


            response = indexHandler.GetResponseOfQuery<Person>(IndexHandler.MakeBoolQuery(
                should: new QueryContainer[]
                    {IndexHandler.MakeMatchQuery("berg", "name")},
                mustNot: new QueryContainer[]
                    {IndexHandler.MakeMatchQuery("hunnber", "name")}
            ));
            IndexHandler.QueryResponsePrinter("bool ", response);


            response = indexHandler.GetResponseOfQuery<Person>(IndexHandler.MakeTermQuery("22", "age", 1));
            IndexHandler.QueryResponsePrinter("Term ", response);

            response = indexHandler.GetResponseOfQuery<Person>(
                IndexHandler.MakeTermsQuery(new string[] {"Baxter", "rosario", "huber"}, "name", 1));
            IndexHandler.QueryResponsePrinter("Terms ", response);

            response = indexHandler.GetResponseOfQuery<Person>(
                IndexHandler.MakeRangeQuery("numeric", "20", "30", "age", 1));
            IndexHandler.QueryResponsePrinter("Range ", response);
            
            response = indexHandler.GetResponseOfQuery<Person>(
                IndexHandler.MakeGeoDistanceQuery("100m", 43.457637, 77.937221, Infer.Field<Person>(p => p.Location)));
            IndexHandler.QueryResponsePrinter("GeoDistance ", response);*/


            response = indexHandler.GetResponseOfAggs<Person>(IndexHandler.MakeTermsAggQuery("gender", keyword: true));
            IndexHandler.TermAggResponsePrinter(response, "gender");


            var refreshResponse = indexHandler.Refresh();
            var clusterHealthResponse = indexHandler.GetClusterHealth();
            var catNodesResponse = indexHandler.GetCatNodes();
            var catIndicesResponse = indexHandler.GetCatIndices();


            Console.WriteLine(refreshResponse.ToString());
            Console.WriteLine(clusterHealthResponse.ClusterName + " " + clusterHealthResponse.Status + " pt" +
                              clusterHealthResponse.NumberOfPendingTasks + " n" + clusterHealthResponse.NumberOfNodes);
            Console.WriteLine();
        }

        static ElasticClient MakeClient(Uri uri)
        {
            var connectionSettings = new ConnectionSettings(uri);
            connectionSettings.EnableDebugMode();
            var client = new ElasticClient(connectionSettings);
            return client;
        }
    }
}