package Phase01.src;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.HashMap;
import java.util.Scanner;

public class FileReader {
    private String path;

    public FileReader(String path) {
        this.path = path;
    }

    public HashMap<String, String> readContent() {
        var filesContents = new HashMap<String, String>();
        for (File file : this.getFiles()) {
            try {
                var scanner = new Scanner(file);
                var content = new StringBuilder();
                while (scanner.hasNextLine()) {
                    content.append(" ").append(scanner.nextLine());
                }
                filesContents.put(file.getName(), content.toString());
                scanner.close();
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
        }
        return filesContents;
    }

    public File[] getFiles() {
        return new File(this.path).listFiles();
    }
}
