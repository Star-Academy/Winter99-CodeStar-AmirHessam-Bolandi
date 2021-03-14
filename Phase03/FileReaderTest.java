
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.util.HashMap;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeEach;

public class FileReaderTest {
    private FileReader fileReader;
    final String PATH = "Phase01/data/Test-Data";

    @BeforeEach
    public void createObject() throws Exception{
        fileReader = new FileReader(PATH);
    }
    
    @Test
    public void readContentTest(){
        var contents = new HashMap<String, String>();
        contents.put("testData1.txt", "hello world..");
        contents.put("testData2.txt", "foo? bar!!!");
        assertTrue(fileReader.readContent().equals(contents));

    }
    
}
