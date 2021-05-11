export class Document {
  public DocumentId: string;
  public Content: string;

  constructor(documentId: string, content: string) {
    this.DocumentId = documentId;
    this.Content = content;
  }
}
