using System;
using Xunit;
using SearchLibrary;
using Nest;

namespace SearchTest
{
    public class ElasticTest
    {
        private static string indexName = "test-index";
        private Elastic elastic = new Elastic(indexName, new Uri("http://localhost:9200"));

        [Fact]
        public void CreateClientTest()
        {
            var pingResponse = elastic.Client.Ping();
            Assert.True(pingResponse.IsValid);
        }

        [Fact]
        public void CreateIndexTest()
        {
        }

    }
}
