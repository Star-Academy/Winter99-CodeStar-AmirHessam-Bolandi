import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.when;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.jupiter.MockitoExtension;

@ExtendWith(MockitoExtension.class)
public class InvertedIndexSearchTest {
    private final HashSet<Entry> QUERY_TEST_RESULT = new HashSet<Entry>(
            Arrays.asList(new Entry("testData1.txt", 1), new Entry("testData2.txt", 2)));
    private final HashSet<Entry> WORD_TEST_RESULT = new HashSet<Entry>(Arrays.asList(new Entry("testData1.txt", 1)));
    @Mock
    Result result1;
    Result result2;
    private InvertedIndexSearch iiSearch;

    @BeforeEach
    public void createObject() throws Exception {
        var contents = new HashMap<String, String>();
        contents.put("testData1.txt", "hello hello world..");
        contents.put("testData2.txt", "foo?  i bars!!! him");
        this.iiSearch = new InvertedIndexSearch(contents);
    }

    @Test
    public void searchTest() {
        result1 = Mockito.mock(Result.class);
        result2 = Mockito.mock(Result.class);
        when(result1.getResultSet()).thenReturn(WORD_TEST_RESULT);
        when(result2.getResultSet()).thenReturn(QUERY_TEST_RESULT);

        assertEquals(iiSearch.search("hello").getResultSet(), (result1.getResultSet()));
        assertEquals(iiSearch.search("hello +bars -doesntExist").getResultSet(), (result2.getResultSet()));
    }

    @Test
    public void searchQueryTest() {
        assertEquals(iiSearch.searchQuery("hello +bar"), QUERY_TEST_RESULT);
    }

    @Test
    public void searchWordTest() {
        assertEquals(iiSearch.searchWord("hello"), WORD_TEST_RESULT);
    }

    @Test
    public void combineResutTest() {
        var normalSet = new HashSet<Entry>(Arrays.asList(new Entry("t1", 1), new Entry("t2", 2)));
        var plusSet = new HashSet<Entry>(Arrays.asList(new Entry("t3", 1), new Entry("t2", 2)));
        var minusSet = new HashSet<Entry>(Arrays.asList(new Entry("t2", 1), new Entry("t5", 2)));
        var resultSet = new HashSet<Entry>(Arrays.asList(new Entry("t1", 1), new Entry("t3", 1)));
        assertEquals(resultSet, iiSearch.combineResults(normalSet, plusSet, minusSet));
    }

    @Test
    public void seperatQueryTest() {
        String inputStr = " hello +ali -go +majid +r";
        assertEquals(iiSearch.seperatQuery(inputStr, "\\+(\\w+)"),
                new ArrayList<String>(Arrays.asList("ali", "majid", "r")));
        assertEquals(iiSearch.seperatQuery(inputStr, "\\-(\\w+)"), new ArrayList<String>(Arrays.asList("go")));
        assertEquals(iiSearch.seperatQuery(inputStr, "\\s(\\w+)"), new ArrayList<String>(Arrays.asList("hello")));
    }

    @Test
    public void addWords() {
        String[] listOfWords = {"hi", "bye", "serious", "hmm"};
        iiSearch.addWords("testId", listOfWords);
        for (String word : listOfWords) {
            assertTrue(iiSearch.getIndexMap().containsKey(word));
        }
    }

    @Test
    public void getListOfWordsTest() {
        assertTrue(Arrays.equals(iiSearch.getListOfWords("$234 hell*garden*33 why %#REZA"),
                new String[]{"234", "hell", "garden", "33", "why", "reza"}));
    }

}
