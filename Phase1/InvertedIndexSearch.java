

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Scanner;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class InvertedIndexSearch {
    final static Set<String> stopWords = new HashSet<>(Arrays.asList("ourselves", "hers", "between", "yourself", "but",
    "again", "there", "about", "once", "during", "out", "very", "having", "with", "they", "own", "an", "be",
    "some", "for", "do", "its", "yours", "such", "into", "of", "most", "itself", "other", "off", "is", "s",
    "am", "or", "who", "as", "from", "him", "each", "the", "themselves", "until", "below", "are", "we", "these",
    "your", "his", "through", "don", "nor", "me", "were", "her", "more", "himself", "this", "down", "should",
    "our", "their", "while", "above", "both", "up", "to", "ours", "had", "she", "all", "no", "when", "at",
    "any", "before", "them", "same", "and", "been", "have", "in", "will", "on", "does", "yourselves", "then",
    "that", "because", "what", "over", "why", "so", "can", "did", "not", "now", "under", "he", "you", "herself",
    "has", "just", "where", "too", "only", "myself", "which", "those", "i", "after", "few", "whom", "t",
    "being", "if", "theirs", "my", "against", "a", "by", "doing", "it", "how", "further", "was", "here",
    "than"));
    public static HashMap<String, ArrayList<Entry>> indexMap = new HashMap<String, ArrayList<Entry>>();
    public static HashSet<Entry> allDocs = new HashSet<Entry>();
    
    public static void main(String[] args) {
        readFiles();
        
        Scanner scanner  = new Scanner(System.in);
        System.out.println("Enter search word or enter 'exit()' to end: ");
        String inputStr = "";
        while (true) {
            inputStr = stemWord(scanner.nextLine().trim());
            if (inputStr.equals("exit()"))
            break;
            if (inputStr.contains(" ") || inputStr.contains("+") || inputStr.contains("-")) {
                printResult(searchQuery(inputStr), true);
            } else {
                printResult(searchWord(inputStr), false);
            }
            
        }
        
    }
    
    public static void printResult(HashSet<Entry> searchResult, boolean isQuery) {
        if (searchResult.isEmpty()) {
            System.out.println("No Result found");
        } else {
            if (isQuery) {
                // for(Entry entry: searchResult){
                    // System.out.println(entry.getDocName()+);
                    // }
                    searchResult.forEach((key) -> System.out.println(key.getDOCName()));
                    
                } else {
                    System.out.println(searchResult.toString());
                }
            }
        }
        
        public static HashSet<Entry> searchQuery(String query) {
            HashSet<Entry> result = new HashSet<Entry>();
            HashSet<Entry> normalSet = new HashSet<Entry>();
            HashSet<Entry> plusSet = new HashSet<Entry>();
            HashSet<Entry> minusSet = new HashSet<Entry>();
            
            ArrayList<String> plus = new ArrayList<String>();
            ArrayList<String> minus = new ArrayList<String>();
            ArrayList<String> normal = new ArrayList<String>();
            
            Pattern pattern = Pattern.compile("\\+(\\w+)");
            Matcher matcher = pattern.matcher(query);
            while (matcher.find()) {
                plus.add(matcher.group(1));
            }
            pattern = Pattern.compile("-(\\w+)");
            matcher = pattern.matcher(query);
            while (matcher.find()) {
                minus.add(matcher.group(1));
            }
            pattern = Pattern.compile("\\s(\\w+)");
            matcher = pattern.matcher(" " + query);
            while (matcher.find()) {
                normal.add(matcher.group(1));
            }
            
            boolean flag = false;
            for (String normalStr : normal) {
                if (!flag) {
                    normalSet = searchWord(normalStr);
                    flag = true;
                } else {
                    normalSet.retainAll(searchWord(normalStr));
                    
                }
            }
            
            for (String plusStr : plus) {
                plusSet.addAll(searchWord(plusStr));
            }
            for (String minusStr : minus) {
                minusSet.addAll(searchWord(minusStr));
            }
            
            // add normal set, Intersect plus set, Minus minus set'
            if (normalSet.isEmpty()) {
                result.addAll(plusSet);
            } else {
                result.addAll(normalSet);
                if (!plusSet.isEmpty()) {
                    result.retainAll(plusSet);
                }
            }
            if (result.isEmpty()) {
                result.addAll(allDocs);
            }
            result.removeAll(minusSet);
            
            return result;
        }
        
        public static HashSet<Entry> searchWord(String word) {
            
            HashSet<Entry> result = new HashSet<Entry>();
            String word_s = "";
            if (word.charAt(word.length() - 1) == 's') {
                word_s = word.substring(0, word.length() - 1);
            } else {
                word_s = word + "s";
            }
            if (indexMap.containsKey(word)) {
                result.addAll(indexMap.get(word));
            }
            if (indexMap.containsKey(word_s)) {
                result.addAll(indexMap.get(word_s));
            }
            return result;
        }
        
        public static String stemWord(String word) {
            String stemmed = word.toLowerCase().replace("'s", "");
            stemmed = stemmed.replace(",", "");
            return stemmed;
        }
        
        public static void readFiles() {
            File directory = new File("EnglishData");
            File fileList[] = directory.listFiles();
            for (File file : fileList) {
                String id = file.getName();
                allDocs.add(new Entry(id, 0));
                try {
                    Scanner scanner = new Scanner(file);
                    while (scanner.hasNextLine()) {
                        String content = scanner.nextLine();
                        addWords(id, content);
                    }
                    scanner.close();
                } catch (FileNotFoundException e) {
                    e.printStackTrace();
                }
            }
        }
        
        public static void addWords(String id, String content) {
            String[] listOfWords = getListOfWords(content);
            for (int i = 0; i < listOfWords.length; i++) {
                String word = stemWord(listOfWords[i]);
                if (stopWords.contains(word))
                continue;
                if (indexMap.containsKey(word)) {
                    indexMap.get(word).add(new Entry(id, i + 1));
                } else {
                    indexMap.put(word, new ArrayList<Entry>(Arrays.asList(new Entry(id, i + 1))));
                }
            }
        }
        
        public static String[] getListOfWords(String content) {
            return content.split("\\s+");
        }
        
    }
    
    class Entry {
        private String DOCName;
        private int index;
        
        Entry(String DOCName, int index) {
            this.DOCName = DOCName;
            this.index = index;
        }
        
        String getDOCName() {
            return this.DOCName;
        }
        
        int getIndex() {
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