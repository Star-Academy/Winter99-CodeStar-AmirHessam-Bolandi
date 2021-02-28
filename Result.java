package Phase01.src;

import java.util.HashSet;

public class Result {
    private HashSet<Entry> resultSet = new HashSet<Entry>();
    private boolean isQuery;
    
    public Result(HashSet<Entry> resultSet, boolean isQuery) {
        this.resultSet = resultSet;
        this.isQuery = isQuery;
    }
    
    @Override
    public String toString() {
        var resultStr = new StringBuilder("");
        if (this.resultSet.isEmpty()) {
            resultStr.append("No result found");
        } else {
            if (this.isQuery) {
                for(Entry entry : resultSet){
                    resultStr.append(" " + entry.getDocName());
                }
            } else {
                resultStr.append(this.resultSet.toString());
            }
        }
        return resultStr.toString();
    }
    
    public HashSet<Entry> getResultSet(){
        return this.resultSet;
    }
}
