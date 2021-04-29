using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Nest;

namespace SearchLibrary
{
    public class SearchEngine
    {
        public readonly Elastic elastic;
        public string IndexName {get;}
        public SearchEngine(string indexName ,Uri uri, bool indexCreated)
        {
            IndexName = indexName;
            elastic = new Elastic(indexName, uri);
            if (!indexCreated)
                elastic.CreateIndex<Document>(mapSelector: CreateMapping).Validate();
        }

        public BulkResponse PostDocuments(string path)
        {
            
            new FileReader(path).ReadContent().ForEach(x => Console.WriteLine("logggg"+x));
            return elastic.BulkIndex(new FileReader(path).ReadContent(), "DocumentId").Validate();
            
        }

        public static ITypeMapping CreateMapping(TypeMappingDescriptor<Document> mappingDescriptor)
        {
            return mappingDescriptor.Properties(d => d
                                        .Keyword(k => k
                                           .Name(d => d.DocumentId)
                                           .IgnoreAbove(256)
                                            ));
        }

        public List<string> Search(List<string> normals, List<string> pluses, List<string> minuses)
        {
            var mustList = MakeMatchQueryList(normals);
            mustList.Add(Elastic.MakeBoolQuery(
                            should: MakeMatchQueryList(pluses).ToArray()
                        ));
            var queryContainer = Elastic.MakeBoolQuery(
                    must: mustList.ToArray(),
                    mustNot: MakeMatchQueryList(minuses).ToArray()
                );

            var response = elastic.GetResponseOfQuery<Document>(queryContainer).Validate();
            return response.Hits.ToList().Select(x => x.Source.DocumentId).ToList();
        }
        public string GetDocuments(string fileName)
        {
            var queryContainer = Elastic.MakeTermQuery(query:fileName ,field:"documentId" );
            var response = elastic.GetResponseOfQuery<Document>(queryContainer).Validate();
            List<Document> documents = response.Hits.ToList().Select(x => x.Source).ToList();
            return JsonSerializer.Serialize(documents);
        }

        private List<QueryContainer> MakeMatchQueryList(List<string> words)
        {
            var list = new List<QueryContainer>();
            foreach (var word in words)
            {
                list.Add(Elastic.MakeMatchQuery(query: word, field: "content"));
            }
            return list;
        }
    }
}