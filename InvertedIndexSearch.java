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
    private static final String NORMAL_PATTERN = "\\s(\\w+)";
    private static final String PLUS_PATTERN = "\\+(\\w+)";
    private static final String MINUS_PATTERN = "-(\\w+)";
    private static final String NOT_WORDS_PATTERN = "[^a-z0-9]";
    private static final String SPACE_PATTERN = "\\s+";

    public InvertedIndexSearch(HashMap<String, String> fileContents) {
        for (String id : fileContents.keySet()) {
            addWords(id, fileContents.get(id));
            allDocs.add(new Entry(id, 0));
        }
    }

    public Result search(String input) {
        Boolean multiWords = input.contains(" ") || input.contains("+") || input.contains("-");
        return new Result(this.searchQuery(input), multiWords);
    }

    private HashSet<Entry> searchQuery(String query) {
        var normalSet = new HashSet<Entry>(allDocs);
        var plusSet = new HashSet<Entry>();
        var minusSet = new HashSet<Entry>();
        for (String normalStr : this.seperatQuery(" " + query, NORMAL_PATTERN))
            normalSet.retainAll(searchWord(normalStr));
        for (String plusStr : this.seperatQuery(query, PLUS_PATTERN))
            plusSet.addAll(searchWord(plusStr));
        for (String minusStr : this.seperatQuery(query, MINUS_PATTERN))
            minusSet.addAll(searchWord(minusStr));
        return this.combineResults(normalSet, plusSet, minusSet);
    }

    private HashSet<Entry> searchWord(String word) {
        var result = new HashSet<Entry>();
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

    private HashSet<Entry> combineResults(HashSet<Entry> normalSet, HashSet<Entry> plusSet, HashSet<Entry> minusSet) {
        // add normal set, Intersect plus set, Minus minus set'
        var result = new HashSet<Entry>();
        result.addAll(normalSet);
        result.addAll(plusSet);
        result.removeAll(minusSet);
        return result;
    }

    private ArrayList<String> seperatQuery(String query, String regex) {
        var words = new ArrayList<String>();
        Pattern pattern = Pattern.compile(regex);
        Matcher matcher = pattern.matcher(query);
        while (matcher.find()) {
            words.add(matcher.group(1));
        }
        return words;
    }

    private void addWords(String id, String content) {
        String[] listOfWords = getListOfWords(content);
        for (int i = 0; i < listOfWords.length; i++) {
            String word = listOfWords[i];
            if (StopWordsClass.STOP_WORDS.contains(word))
                continue;
            if (indexMap.containsKey(word)) {
                indexMap.get(word).add(new Entry(id, i + 1));
            } else {
                indexMap.put(word, new ArrayList<Entry>(Arrays.asList(new Entry(id, i + 1))));
            }
        }
    }

    private String[] getListOfWords(String content) {
        content = content.toLowerCase().replaceAll(NOT_WORDS_PATTERN, " ");
        return content.split(SPACE_PATTERN);
    }

}
