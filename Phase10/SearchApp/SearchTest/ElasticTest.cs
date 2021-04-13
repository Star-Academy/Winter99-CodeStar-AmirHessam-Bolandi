using System.Linq;
using System;
using Xunit;
using SearchLibrary;
using Elasticsearch.Net;
using System.Collections.Generic;

namespace SearchTest
{
    public class ElasticTest
    {
        private static string indexName = "test-index";
        private Elastic elastic = new Elastic(indexName, new Uri("http://localhost:9200"));

        public void CreateClientTest()
        {
            var pingResponse = elastic.Client.Ping();
            Assert.True(pingResponse.IsValid);
        }

        [Fact]
        public void IndexTest()
        {
            elastic.CreateIndex<Document>();
            var response = elastic.Client.Indices.Exists(indexName).Validate();
            Assert.True(response.Exists);
            elastic.DeleteIndex();
            response = elastic.Client.Indices.Exists(indexName).Validate();
            Assert.False(response.Exists);
        }

        [Fact]
        public void CreateMatchQueryTest()
        {
            var queryContainer = Elastic.MakeMatchQuery("query-test", "field-test", fuzziness: 1);
            var queryString = elastic.Client.RequestResponseSerializer.SerializeToString(queryContainer);
            var realQuery = "{\"match\":{\"field-test\":{\"fuzziness\":1,\"query\":\"query-test\"}}}";
            Assert.Equal(realQuery, queryString);
        }

        [Fact]
        public void BulkTest()
        {
            try
            {
                var documents = new List<Document> { new Document("1", "one"), new Document("2", "two"), new Document("3", "three") };
                elastic.CreateIndex<Document>();
                elastic.BulkIndex<Document>(documents, "DocumentId");
                var response = elastic.GetResponseOfQuery<Document>(Elastic.MakeMatchQuery("one", "content"));
                Assert.Equal("one", response.Hits.Single().Source.Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                elastic.DeleteIndex();
            }
        }
    }
}
