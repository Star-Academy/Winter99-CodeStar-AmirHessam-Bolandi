using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// this project use HttpClient for querying ElasticSearch 


namespace people
{
    class Program
    {
        static String localHost = "http://localhost:9200";
        private static Uri uri = new Uri(localHost);

        static void Main(string[] args)
        {
            var fileContent = File.ReadAllText("..\\..\\..\\people_details.json");

            var personsList = JsonConvert.DeserializeObject<List<Person>>(fileContent);
            var connectionSettings = new ConnectionSettings(uri);
            connectionSettings.EnableDebugMode();
            var client = new ElasticClient(connectionSettings);
            // var responsePing = client.Ping();
            // Console.WriteLine(responsePing);

            var index = "my-index";
            /*
            MakeIndex(index,client);
            BulkIndex(index,personsList,client);
            */

            // MakeMatchQuery(index, "Huber", "name", client);
            MakeMatchQuery(index, "blue", "eyeColor", client);
            
            var responseClusterHealth = client.Cluster.Health();
            var responseCatNodes = client.Cat.Nodes();
            var responseCatIndices = client.Cat.Indices();

            Console.WriteLine(responseClusterHealth.ClusterName);


        }

        static void MakeMatchQuery(string index, string query, string field, ElasticClient client)
        {
            QueryContainer matchQuery = new MatchQuery
            {
                Query = query,
                Field = field
            };

             var response = client.Search<Dictionary<string, object>>(s => s.Index(index).Query(q => matchQuery));
            Console.WriteLine(response);
            Console.WriteLine(response.Hits.Count);
            // Console.WriteLine(response.);
        }

        static ResponseBase MakeIndex(string index, ElasticClient client)
        {
            var response = client.Indices.Create(index,
                s => s.Settings(settings => settings
                        .Setting("max_ngram_diff", 7)
                        .Analysis(analysis => analysis
                            .TokenFilters(tf => tf
                                .NGram("my-ngram-filter", ng => ng
                                    .MinGram(3)
                                    .MaxGram(10)))
                            .Analyzers(analyzer => analyzer
                                .Custom("my-ngram-analyzer", custom => custom
                                    .Tokenizer("standard")
                                    .Filters("lowercase", "my-ngram-filter")))))
                    .Map<Person>(m => m
                        .Properties(pr => pr
                            .Text(t => t
                                .Name(n => n.About)
                                .Fields(f => f
                                    .Text(ng => ng
                                        .Name("ngram")
                                        .Analyzer("my-ngram-analyzer")))))));
            return response;
        }

        static void BulkIndex(string index, List<Person> personsList, ElasticClient client)
        {
            var bulkDescriptor = new BulkDescriptor();
            foreach (var person in personsList)
            {
                bulkDescriptor.Index<Person>(x => x
                    .Index(index)
                    .Document(person)
                );
            }

            client.Bulk(bulkDescriptor);
        }
    }
}


public class Person
{
    [JsonPropertyName("age")] public int Age { get; set; }

    [JsonPropertyName("eyeColor")] public string EyeColor { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("gender")] public string Gender { get; set; }

    [JsonPropertyName("company")] public string Company { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("phone")] public string Phone { get; set; }

    [JsonPropertyName("address")] public string Address { get; set; }

    [JsonPropertyName("about")] public string About { get; set; }

    [JsonPropertyName("registration_date")]
    public string RegistrationDate { get; set; }

    [Ignore]
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [Ignore]
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    private string location = null;

    public string Location
    {
        get
        {
            if (location is null)
                return $"{Latitude},{Longitude}";
            return location;
        }
        set { location = value; }
    }
}


/*
static async Task Main()
        {

            try
            {
                client.BaseAddress =new Uri(localHost );
                HttpResponseMessage response = await client.GetAsync("");
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            
            try
            {
                HttpResponseMessage response = await client.PutAsync("/people", new StringContent(""));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            
    
            try
            {
                var fileString = File.ReadAllText("..\\..\\..\\new-people.json");
                Console.WriteLine(fileString);

                var result = JsonConvert.DeserializeObject<List<JObject>>(fileString);
                foreach (JObject p in result)
                {
                    HttpResponseMessage response = await client.PostAsync("/people/_doc", new StringContent(p.ToString(),Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
               
                Console.WriteLine("done");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
        */