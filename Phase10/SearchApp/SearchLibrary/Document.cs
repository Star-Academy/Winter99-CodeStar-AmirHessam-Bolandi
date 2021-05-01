namespace SearchLibrary
{
    public class Document
    {
        public string DocumentId { get; set; }
        public string Content { get; set; }

        public Document(string documentId, string content)
        {
            DocumentId = documentId;
            Content = content;
        }
    }
}