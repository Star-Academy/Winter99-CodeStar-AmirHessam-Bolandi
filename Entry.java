package Phase01.src;

class Entry {
    private String DOCName;
    private int index;
    
    public Entry(String DOCName, int index) {
        this.DOCName = DOCName;
        this.index = index;
    }
    
    public String getDOCName() {
        return this.DOCName;
    }
    
    public int getIndex() {
        return this.index;
    }
    
    @Override
    public String toString() {
        return "Document Id: " + this.DOCName + ", Index: " + this.index;
    }
    
    @Override
    public boolean equals(Object o) {
        if (!(o instanceof Entry)) {
            return false;
        }
        Entry e = (Entry) o;
        return this.DOCName.equals(e.DOCName);
    }
    
    @Override
    public int hashCode() {
        return this.DOCName.hashCode();
    }
}

