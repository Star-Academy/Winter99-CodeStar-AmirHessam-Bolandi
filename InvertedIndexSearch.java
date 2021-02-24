package Phase01.src;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Scanner;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class InvertedIndexSearch {

    private HashMap<String, ArrayList<Entry>> indexMap = new HashMap<String, ArrayList<Entry>>();
    private HashSet<Entry> allDocs = new HashSet<Entry>();
    // private HashMap<String, String> fileContents = new HashMap<String, String>();
    private final Set<String> stopWords = new HashSet<>(Arrays.asList("ourselves", "hers", "between", "yourself", "but",
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

    public InvertedIndexSearch(HashMap<String, String> fileContents) {
        for (String id : fileContents.keySet()) {
            addWords(id, fileContents.get(id));
            allDocs.add(new Entry(id, 0));
        }
    }

    public Result search(String input) {
        if (input.contains(" ") || input.contains("+") || input.contains("-")) {
            return new Result(this.searchQuery(input), true);
        } else {
            return new Result(this.searchWord(input), false);
        }
    }

    private HashSet<Entry> searchQuery(String query) {
        HashSet<Entry> normalSet = new HashSet<Entry>(allDocs), plusSet = new HashSet<Entry>(),
                minusSet = new HashSet<Entry>();
        for (String normalStr : this.seperatQuery(" " + query, "\\s(\\w+)"))
            normalSet.retainAll(searchWord(normalStr));
        for (String plusStr : this.seperatQuery(query, "\\+(\\w+)"))
            plusSet.addAll(searchWord(plusStr));
        for (String minusStr : this.seperatQuery(query, "-(\\w+)"))
            minusSet.addAll(searchWord(minusStr));
        return this.combineResults(normalSet, plusSet, minusSet);
    }

    private HashSet<Entry> combineResults(HashSet<Entry> normalSet, HashSet<Entry> plusSet, HashSet<Entry> minusSet) {
        // add normal set, Intersect plus set, Minus minus set'
        HashSet<Entry> result = new HashSet<Entry>();
        result.addAll(normalSet);
        result.addAll(plusSet);
        result.removeAll(minusSet);
        return result;
    }

    private ArrayList<String> seperatQuery(String query, String regex) {
        ArrayList<String> words = new ArrayList<String>();
        Pattern pattern = Pattern.compile(regex);
        Matcher matcher = pattern.matcher(query);
        while (matcher.find()) {
            words.add(matcher.group(1));
        }
        return words;
    }

    private HashSet<Entry> searchWord(String word) {
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

    private void addWords(String id, String content) {
        String[] listOfWords = getListOfWords(content);
        for (int i = 0; i < listOfWords.length; i++) {
            String word = listOfWords[i];
            if (stopWords.contains(word))
                continue;
            if (indexMap.containsKey(word)) {
                indexMap.get(word).add(new Entry(id, i + 1));
            } else {
                indexMap.put(word, new ArrayList<Entry>(Arrays.asList(new Entry(id, i + 1))));
            }
        }
    }

    private String[] getListOfWords(String content) {
        content = content.toLowerCase().replaceAll("[^a-z0-9]", " ");
        return content.split("\\s+");
    }

}
