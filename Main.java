package Phase01.src;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        FileReader fileReader = new FileReader("Phase01/data/EnglishData");
        HashMap<String, String> fileContents = fileReader.readContent();
        InvertedIndexSearch invertedIndexSearch = new InvertedIndexSearch(fileContents);

        Scanner scanner = new Scanner(System.in);
        String inputStr = "";
        while (true) {
            System.out.println("Enter search word or enter 'exit()' to end: ");
            inputStr = scanner.nextLine().toLowerCase();
            if (inputStr.equals("exit()"))
                break;
            System.out.println(invertedIndexSearch.search(inputStr));

        }
    }

}
