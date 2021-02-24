package Phase01.src;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.HashMap;
import java.util.Scanner;

public class FileReader {
    private HashMap<String, String> filesContents = new HashMap<String, String>();
    private String path;

    public FileReader(String path) {
        this.path = path;
    }

    public HashMap<String, String> readContent() {
        for (File file : this.getFiles()) {
            try {
                Scanner scanner = new Scanner(file);
                String content = "";
                while (scanner.hasNextLine()) {
                    content = content + " " + scanner.nextLine();
                }
                filesContents.put(file.getName(), content);
                scanner.close();
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
        }
        return this.filesContents;
    }

    public File[] getFiles() {
        File directory = new File(this.path);
        return directory.listFiles();
    }
}
