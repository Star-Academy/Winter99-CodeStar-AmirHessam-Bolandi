
import java.util.Arrays;
import java.util.HashSet;

import static org.junit.Assert.assertTrue;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

public class ResultTest {
    private Result query, word ,empty;

    @BeforeEach
    public void createObject() throws Exception {
        Entry[] entryList = { new Entry("test1", 11), new Entry("test2", 14) };
        query = new Result(new HashSet<Entry>(Arrays.asList(entryList)), true);
        word = new Result(new HashSet<Entry>(Arrays.asList(entryList)), false);
        empty = new Result(new HashSet<Entry>(), false);
    }

    @Test
    public void toStringTest() {
        assertTrue(query.toString().equals(" test1 test2") || query.toString().equals(" test2 test1"));
        assertTrue(word.toString().equals("[Document Id: test1, Index: 11, Document Id: test2, Index: 14]")
                || word.toString().equals("[Document Id: test2, Index: 14, Document Id: test1, Index: 11]"));
        assertTrue(empty.toString().equals("No result found"));

    }

}
