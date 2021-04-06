using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Elasticsearch.Net;
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
            ISearchResponse<Person> response = null;
            response = GetResponseOfQuery(MakeMatchQuery("Hunbe", "name", 3), index, client);
            Console.WriteLine("Match query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));

            response = GetResponseOfQuery(MakeFuzzyQuery("ros", "name", 3), index, client);
            Console.WriteLine("fuzzy query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));


            response = GetResponseOfQuery(MakeMultiMatchQuery("culpa", new string[] {"name", "about"}, 3), index,
                client);
            Console.WriteLine("MultiMatch query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));
            
            response = GetResponseOfQuery(MakeBoolQuery(
                should:new QueryContainer[]
                    {MakeMatchQuery("berg","name")},
                mustNot: new QueryContainer[]
                    { MakeMatchQuery("hunnber","name")}
            ), index, client);
            Console.WriteLine("bool query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));


            response = GetResponseOfQuery(MakeTermQuery("22", "age", 1), index, client);
            Console.WriteLine("Term query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));


            response = GetResponseOfQuery(MakeTermsQuery(new string[] {"Baxter", "rosario", "huber"}, "name", 1), index,
                client);
            Console.WriteLine("Terms query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));

            response = GetResponseOfQuery(MakeRangeQuery("numeric", "20", "30", "age", 1), index, client);
            Console.WriteLine("Range query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));


            response = GetResponseOfQuery(MakeGeoDistanceQuery("100m", 43.457637, 77.937221), index, client);
            Console.WriteLine("GeoDistance query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));


            var refreshResponse = client.Indices.Refresh(index);
            var clusterHealthResponse = client.Cluster.Health(index , s=>s.WaitForStatus(WaitForStatus.Green).Timeout(1000));
            var catNodesResponse = client.Cat.Nodes();
            var catIndicesResponse = client.Cat.Indices();

            
            Console.WriteLine(refreshResponse.ToString());
            Console.WriteLine(clusterHealthResponse.ClusterName+" "+clusterHealthResponse.Status+" pt"+clusterHealthResponse.NumberOfPendingTasks+" n"+clusterHealthResponse.NumberOfNodes);
        }


        static ISearchResponse<Person> GetResponseOfQuery(QueryContainer queryContainer, string index,
            ElasticClient client)
        {
            return client.Search<Person>(s => s.Index(index).Query(q => queryContainer));
        }

        static QueryContainer MakeFuzzyQuery(string query, string field, int fuzziness = -1)
        {
            QueryContainer fuzzyQuery = new FuzzyQuery
            {
                Field = field,
                Value = query,
                Fuzziness = fuzziness == -1 ? Fuzziness.Auto : Fuzziness.EditDistance(fuzziness)
            };
            return fuzzyQuery;
        }

        static QueryContainer MakeMatchQuery(string query, string field, int fuzziness = 1)
        {
            QueryContainer matchQuery = new MatchQuery
            {
                Query = query,
                Field = field,
                Fuzziness = Fuzziness.EditDistance(fuzziness)
            };
            return matchQuery;
        }

        static QueryContainer MakeMultiMatchQuery(string query, string[] fields,
            int fuzziness = 1)
        {
            QueryContainer multiMatchQuery = new MultiMatchQuery
            {
                Query = query,
                Fields = fields,
                Fuzziness = Fuzziness.EditDistance(fuzziness)
            };
            return multiMatchQuery;
        }

        static QueryContainer MakeTermQuery(string query, string field, double boost = 1)
        {
            QueryContainer termQuery = new TermQuery
            {
                Field = field,
                Value = query,
                Boost = boost
            };
            return termQuery;
        }

        static QueryContainer MakeTermsQuery(string[] queries, string field, double boost = 1)
        {
            QueryContainer termsQuery = new TermsQuery
            {
                Field = field,
                Terms = queries,
                Boost = boost
            };
            return termsQuery;
        }
        
        static QueryContainer MakeRangeQuery(string type, string gte, string lte,
            string field, double boost = 1)
        {
            QueryContainer rangeQuery = null;
            if (type.ToLower() == "long")
            {
                rangeQuery = new LongRangeQuery()
                {
                    Field = field,
                    LessThan = long.Parse(lte),
                    GreaterThan = long.Parse(gte),
                    Boost = boost
                };
            }
            else if (type.ToLower() == "date")
            {
                rangeQuery = new DateRangeQuery()
                {
                    Field = field,
                    LessThan = DateMath.FromString(lte),
                    GreaterThan = DateMath.FromString(gte),
                    Boost = boost
                };
            }
            else if (type.ToLower() == "term")
            {
                rangeQuery = new TermRangeQuery()
                {
                    Field = field,
                    LessThan = lte,
                    GreaterThan = gte,
                    Boost = boost
                };
            }
            else if (type.ToLower() == "numeric")
            {
                rangeQuery = new TermRangeQuery()
                {
                    Field = field,
                    LessThan = lte,
                    GreaterThan = gte,
                    Boost = boost
                };
            }

            return rangeQuery;
        }

        static QueryContainer MakeBoolQuery(QueryContainer[] must = null, QueryContainer[] filter = null,
            QueryContainer[] should = null, QueryContainer[] mustNot = null, double boost = 1)
        {
            QueryContainer boolQuery = new BoolQuery
            {
                Must = must,
                Should = should,
                Filter = filter,
                MustNot = mustNot,
                Boost = boost
            };
            return boolQuery;
        }

        static QueryContainer MakeGeoDistanceQuery(string distance, double latitude,
            double longitude, double boost = 1)
        {
            var geoDistanceQuery = new GeoDistanceQuery
            {
                Field = Infer.Field<Person>(p => p.Location),
                DistanceType = GeoDistanceType.Arc,
                Location = new GeoLocation(latitude, longitude),
                Distance = distance,
                Boost = boost
            };
            return geoDistanceQuery;
        }

        /*static   MakeTermsAggQuery(string index, ElasticClient client, string[] queries, string field, double boost = 1)
        {
            TermsAggregation termsAggregation = new TermsAggregation(field);
            // return null; //client.Search<Person>(s => s.Index(index).Query(q => termsQuery));

        }*/

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
                                        .Analyzer("my-ngram-analyzer")))
                            )
                        )
                    )
            );
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

    public override string ToString()
    {
        return Name + " " + Age + " " + Email + " p{" + Phone + "} " + Location;
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