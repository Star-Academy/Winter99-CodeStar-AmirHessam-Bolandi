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
        String resultStr = "";
        if (this.resultSet.isEmpty()) {
            resultStr = "No Result found";
        } else {
            if (this.isQuery) {
                for(Entry entry : resultSet){
                    resultStr += " " + entry.getDOCName();
                }
            } else {
                resultStr = this.resultSet.toString();
            }
        }
        return resultStr;
    }
    
    public HashSet<Entry> getResultSet(){
        return this.resultSet;
    }
}
