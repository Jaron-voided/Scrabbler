namespace Scrabbler;

public class WordList
{
    public static Dictionary<string, int> GetWordList()
    {
        string filePath = "dictionary.json";

        try
        {
            string fileContent = File.ReadAllText(filePath);

            Dictionary<string, int> wordListAndScores = fileContent.Split(',')
                .Select(word => word.Replace("\"", "").Trim())
                .Where(word => !string.IsNullOrEmpty(word))
                .Distinct()
                .ToDictionary(
                    word => word,
                    word => 0
                );

            return wordListAndScores;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading word list: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }

    public static AllDictionaries GetPrunedWordList(List<char> singleBoardLetter)
    {
        try
        {
            Dictionary<string, int> allWords = GetWordList();

            AllDictionaries prunedWords = new AllDictionaries ();
            foreach (char letter in singleBoardLetter)
            {
                string label = letter.ToString();

                if (!prunedWords.ContainsDictionary(label))
                {
                    prunedWords.AddDictionary(new WordDictionary(letter.ToString()));
                }
            }

            foreach (var entry in allWords)
            {
                string word = entry.Key;
                int score = entry.Value;

                foreach (char letter in singleBoardLetter)
                {
                    if (word.Contains(letter) && !prunedWords.ContainsWord(word))
                    {
                        prunedWords.WordDictionaries[letter.ToString()].Words.Add(word, score);
                        break;
                    }
                }
            }
            
            return prunedWords;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading word list: {ex.Message}");
            return new AllDictionaries();
        }
    }

    public static AllDictionaries GetPrunedWordList(List<string> multipleBoardLetters)
    {
        try
        {
            Dictionary<string, int> allWords = GetWordList();

            AllDictionaries prunedWords = new AllDictionaries();
            
            foreach (string letters in multipleBoardLetters)
            {
                string label = letters;

                if (!prunedWords.ContainsDictionary(label))
                {
                    prunedWords.AddDictionary(new WordDictionary(letters));
                }
            }

            foreach (var entry in allWords)
            {
                string word = entry.Key;
                int score = entry.Value;

                foreach (string letters in multipleBoardLetters)
                {
                    if (word.Contains(letters) && !prunedWords.ContainsWord(word))
                    {
                        prunedWords.WordDictionaries[letters].Words.Add(word, score);
                        break;
                    }
                }
            }

            return prunedWords;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading word list: {ex.Message}");
            return new AllDictionaries();
        }
    }

    public static AllDictionaries GetPrunedWordList(List<char> singleBoardLetter,
        List<string> multipleBoardLetters)
    {
        try
        {
            Dictionary<string, int> allWords = GetWordList();

            AllDictionaries prunedWords = new AllDictionaries();

            foreach (var entry in allWords)
            {
                string word = entry.Key;
                int score = entry.Value;

                // Check multiple letter sequences first
                foreach (string letters in multipleBoardLetters)
                {
                    if (word.Contains(letters) && !prunedWords.ContainsWord(word))
                    {
                        prunedWords.WordDictionaries[letters].Words.Add(word, score);
                        break;
                    }
                }

                // If word wasn't added by multiple letters, check single letters
                if (!prunedWords.ContainsWord(word))
                {
                    foreach (char letter in singleBoardLetter)
                    {
                        if (word.Contains(letter) && !prunedWords.ContainsWord(word))
                        {
                            prunedWords.WordDictionaries[letter.ToString()].Words.Add(word, score);
                            break;
                        }
                    }
                }
            }
            
            return prunedWords;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading word list: {ex.Message}");
            return new AllDictionaries();
        }
    }
}