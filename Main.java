package Phase01.src;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Scanner;

public class Main {
    private static final String DATA_PATH = "Phase01/data/EnglishData";
    private static final String WELCOME_MASSAGE = "Enter search word or enter 'exit()' to end: ";
    private static final String EXIT_COMMAND = "exit()";

    public static void main(String[] args) {
        FileReader fileReader = new FileReader(DATA_PATH);
        var fileContents = fileReader.readContent();
        var invertedIndexSearch = new InvertedIndexSearch(fileContents);
        var scanner = new Scanner(System.in);
        String inputStr = "";
        while (true) {
            System.out.println(WELCOME_MASSAGE);
            inputStr = scanner.nextLine().toLowerCase();
            if (inputStr.equals(EXIT_COMMAND))
                break;
            System.out.println(invertedIndexSearch.search(inputStr));
        }
    }

}
