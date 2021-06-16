using System;
using System.Collections.Generic;
using System.Linq;
using Nest;


namespace SearchLibrary
{

    public class Elastic
    {
        string IndexName { get; set; }
        public ElasticClient Client { get; set; }

        public Elastic(string indexName, Uri uri)
        {
            IndexName = indexName;
            Client = this.CreateClient(uri);            
        }

        public ElasticClient CreateClient(Uri uri) 
        {
            var connectionSettings = new ConnectionSettings(uri);
            connectionSettings.EnableDebugMode();
            return new ElasticClient(connectionSettings);
        }

        public ISearchResponse<T> GetResponseOfQuery<T>(QueryContainer queryContainer, int size=20) where T : class
        {
            return Client.Search<T>(s => s.Index(IndexName).Query(q => queryContainer).Size(size));
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

        public static QueryContainer MakeMatchQuery(string query, string field, int fuzziness = 0)
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
        public ISearchResponse<T> GetResponseOfAggs<T>(TermsAggregation termsAggregation) where T : class
        {

            return Client.Search<T>(s => s.Index(IndexName).Aggregations(
                termsAggregation));
        }
        public static TermsAggregation MakeTermsAggQuery(string field, string name = "", bool keyword = false)
        {
            if (name == "")
                name = field;
            TermsAggregation termsAggregation = new TermsAggregation(name)
            {
                Field = field + (keyword ? ".keyword" : "")
            };
            return termsAggregation;
        }


        public ResponseBase CreateIndex<T>(Func<IndexSettingsDescriptor, IPromise<IIndexSettings>> settingSelector = null,
            Func<TypeMappingDescriptor<T>, ITypeMapping> mapSelector = null) where T : class
        {
            var response = Client.Indices.Create(IndexName,
                s => s.Settings(settingSelector).Map<T>(mapSelector));
            return response;
        }

        public ResponseBase DeleteIndex(){
            return Client.Indices.Delete(IndexName);
        }

        public BulkResponse BulkIndex<T>(List<T> dataList , string idFieldName) where T : class
        {
            var bulkDescriptor = new BulkDescriptor();
            foreach (var data in dataList)
            {
                bulkDescriptor.Index<T>(x => x
                    .Index(IndexName)
                    .Document(data)
                    .Id((string)data.GetType().GetProperty(idFieldName).GetValue(data))
                );
            }
            var response = Client.Bulk(bulkDescriptor);
            return response;
        }

        public RefreshResponse Refresh()
        {
            return Client.Indices.Refresh(IndexName);
        }

        public CatResponse<CatNodesRecord> GetCatNodes()
        {
            return Client.Cat.Nodes();
        }
        public CatResponse<CatIndicesRecord> GetCatIndices()
        {
            return Client.Cat.Indices();
        }

        public ClusterHealthResponse GetClusterHealth(
            Func<ClusterHealthDescriptor, IClusterHealthRequest> healthSelector = null)
        {
            return Client.Cluster.Health(IndexName, healthSelector);
        }

        public static void QueryResponsePrinter<T>(string queryType, ISearchResponse<T> response) where T : class
        {
            Console.WriteLine(queryType + " query:  ---------------------");
            response.Hits.ToList().ForEach(x => Console.WriteLine(x.Source.ToString()));
        }
        public static void TermAggResponsePrinter<T>(ISearchResponse<T> response, string name) where T : class
        {
            Console.WriteLine(name + " Terms Aggregation:  ---------------------");
            response.Aggregations.Terms(name).Buckets.ToList().ForEach(x => Console.WriteLine(x.Key + " : " + x.DocCount));
        }
    }
}

