using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace NestHandler
{
    public class IndexHandler//<T> where T : class
    {
        string IndexName { get; set; }
        ElasticClient Client { get; set; }


        public IndexHandler(string indexName, ElasticClient elasticClient)
        {
            IndexName = indexName;
            Client = elasticClient;
        }

        public ISearchResponse<T> GetResponseOfQuery<T>(QueryContainer queryContainer) where T : class
        {
            return Client.Search<T>(s => s.Index(IndexName).Query(q => queryContainer));
        }

        public static QueryContainer MakeFuzzyQuery(string query, string field, int fuzziness = -1)
        {
            QueryContainer fuzzyQuery = new FuzzyQuery
            {
                Field = field,
                Value = query,
                Fuzziness = fuzziness == -1 ? Fuzziness.Auto : Fuzziness.EditDistance(fuzziness)
            };
            return fuzzyQuery;
        }

        public static QueryContainer MakeMatchQuery(string query, string field, int fuzziness = 1)
        {
            QueryContainer matchQuery = new MatchQuery
            {
                Query = query,
                Field = field,
                Fuzziness = Fuzziness.EditDistance(fuzziness)
            };
            return matchQuery;
        }

        public static QueryContainer MakeMultiMatchQuery(string query, string[] fields,
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

        public static QueryContainer MakeTermQuery(string query, string field, double boost = 1)
        {
            QueryContainer termQuery = new TermQuery
            {
                Field = field,
                Value = query,
                Boost = boost
            };
            return termQuery;
        }

        public static QueryContainer MakeTermsQuery(string[] queries, string field, double boost = 1)
        {
            QueryContainer termsQuery = new TermsQuery
            {
                Field = field,
                Terms = queries,
                Boost = boost
            };
            return termsQuery;
        }

        public static QueryContainer MakeRangeQuery(string type, string gte, string lte,
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

        public static QueryContainer MakeBoolQuery(QueryContainer[] must = null, QueryContainer[] filter = null,
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

        public static QueryContainer MakeGeoDistanceQuery(string distance, double latitude,
            double longitude, Field distanceField, double boost = 1)
        {
            var geoDistanceQuery = new GeoDistanceQuery
            {
                Field = distanceField,
                DistanceType = GeoDistanceType.Arc,
                Location = new GeoLocation(latitude, longitude),
                Distance = distance,
                Boost = boost
            };
            return geoDistanceQuery;
        }


        public ResponseBase MakeIndex<T>(Func<IndexSettingsDescriptor, IPromise<IIndexSettings>> settingSelector,
            Func<TypeMappingDescriptor<T>, ITypeMapping> mapSelector)where T:class
        {
            var response = Client.Indices.Create(IndexName,
                s => s.Settings(settingSelector).Map<T>(mapSelector));
                /*
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
                );*/
            return response;
        }

        public void BulkIndex<T>( List<T> personsList)where T:class
        {
            var bulkDescriptor = new BulkDescriptor();
            foreach (var person in personsList)
            {
                bulkDescriptor.Index<T>(x => x
                    .Index(IndexName)
                    .Document(person)
                );
            }
            Client.Bulk(bulkDescriptor);
        }

        public RefreshResponse Refresh()
        {
            return Client.Indices.Refresh(IndexName);
        }

        public  CatResponse<CatNodesRecord> GetCatNodes()
        {
            return Client.Cat.Nodes();
        }
        public  CatResponse<CatIndicesRecord> GetCatIndices()
        {
            return Client.Cat.Indices();
        }

        public ClusterHealthResponse GetClusterHealth(
            Func<ClusterHealthDescriptor, IClusterHealthRequest> healthSelector = null)
        {
            return Client.Cluster.Health(IndexName, healthSelector);
        }

        /*static   MakeTermsAggQuery(string index, ElasticClient client, string[] queries, string field, double boost = 1)
        {
            TermsAggregation termsAggregation = new TermsAggregation(field);
            // return null; //client.Search<Person>(s => s.Index(index).Query(q => termsQuery));

        }*/

        public static void QueryResponsePrinter<T>(string queryType, ISearchResponse<T> response) where T:class
        {
            Console.WriteLine(queryType + " query:");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));
        }
    }
}