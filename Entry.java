package Phase01.src;

class Entry {
    private String docName;
    private int index;
    
    public Entry(String docName, int index) {
        this.docName = docName;
        this.index = index;
    }
    
    public String getDocName() {
        return this.docName;
    }
    
    public int getIndex() {
        return this.index;
    }
    
    @Override
    public String toString() {
        return "Document Id: " + this.docName + ", Index: " + this.index;
    }
    
    @Override
    public boolean equals(Object o) {
        if (!(o instanceof Entry)) {
            return false;
        }
        return this.docName.equals(((Entry) o).docName);
    }
    
    @Override
    public int hashCode() {
        return this.docName.hashCode();
    }
}

