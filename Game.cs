using Scrabbler.Interfaces;

namespace Scrabbler;

public class Game
{
    private readonly IPlayableWords _playableWords;
    private readonly IWordList _wordList;
    private readonly IWordScoring _wordScoring;
    private readonly IAllDictionaries _allDictionaries;
    private readonly IWordDictionary _wordDictionary;
    private readonly IWordSearch _wordSearch;
    
    public Game(IPlayableWords playableWords, IWordList wordList, IWordScoring wordScoring, IAllDictionaries allDictionaries, IWordDictionary wordDictionary, IWordSearch wordSearch)
    {
        _playableWords = playableWords;
        _wordList = wordList;
        _wordScoring = wordScoring;
        _allDictionaries = allDictionaries;
        _wordDictionary = wordDictionary;
        _wordSearch = wordSearch;
    }

    public void Run()
    {
        Console.WriteLine("Welcome to Scrabbler!");
        
        // Get user input for letters
        Console.Write("Enter your letters (no spaces): ");
        string lettersInput = Console.ReadLine();
        List<char> letters = lettersInput.ToLower().ToList();
        
        // Get user input for board letters
        Console.Write("Enter board letters (no spaces): ");
        string boardLettersInput = Console.ReadLine();
        List<char> boardLetters = boardLettersInput.ToLower().ToList();
        
        // Get user input for multiple board letters
        Console.Write("Enter multiple board letter sequences (comma separated): ");
        string multipleBoardLettersInput = Console.ReadLine();
        List<string> multipleBoardLetters = multipleBoardLettersInput.Split(',')
            .Select(s => s.Trim().ToLower())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();
        
        // Check for blank tile
        Console.WriteLine("Do you have a blank tile? Enter a y for yes or n for no");
        string yesOrNo = Console.ReadLine();
        bool hasWildCard = yesOrNo == "y";
        
        
        // Display information
        Console.WriteLine("\nYour letters: " + string.Join(", ", letters));
        Console.WriteLine("Board letters: " + string.Join(", ", boardLetters));
        Console.WriteLine("Multiple board letters: " + string.Join(", ", multipleBoardLetters));
        
        // Find playable words
        AllDictionaries words = _playableWords.PlayableWordsSingleAndMultipleLetters(
            letters, boardLetters, multipleBoardLetters, hasWildCard);
        //AllDictionaries words = _playableWords.PlayableWordsSingleLetters(letters, boardLetters, hasWildCard);
        
        // Display results
        foreach (WordDictionary dictionary in words.WordDictionaries.Values)
        {
            Console.WriteLine($"\nDictionary for: {dictionary.Label}");
            foreach (var (word, score) in dictionary.Words)
            {
                Console.Write($"{word}: {score} ||");
            }
        }
        
        Console.WriteLine("\nTop 3 words for each dictionary:");
        words.PrintTopFewValues(3);
        
        Console.WriteLine("\nHighest scoring word for each dictionary:");
        words.PrintTopValueForEach();
    }
}