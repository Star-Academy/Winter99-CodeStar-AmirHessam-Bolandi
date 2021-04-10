using System;
using System.Collections.Generic;
using Nest;

namespace SearchLibrary
{
    public class SearchEngine
    {
        private readonly Elastic elastic;
        private const string INDEX_NAME = "documents";
        public SearchEngine(Uri uri,bool indexCreated){
            elastic = new Elastic(INDEX_NAME, uri);
            if(!indexCreated)
                elastic.CreateIndex<Document>(CreateMapping).Validate();
        }

        public BulkResponse PostDocuments(string path)
        {
            
            return elastic.BulkIndex(new FileReader(path).ReadContent()).Validate();
        }
        
        public static ITypeMapping CreateMapping(TypeMappingDescriptor<Document> mappingDescriptor)
        {
            return mappingDescriptor.Properties(d => d
                                        .Keyword(k => k
                                           .Name(d => d.DocumentId)
                                           .IgnoreAbove(256)
                                            ));
        }
        
    }
}