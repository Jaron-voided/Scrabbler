
using Scrabbler;




List<char> letters = new List<char> { 'p', 'r', 'e', 's', 'u', 't', 'l' };
List<char> boardLetters = new List<char> { 'e', 't', 'q', 'a'};
List<string> multipleBoardLetters = new List<string> { "re", "ex", "ty" };
AllDictionaries words = WordsToPlay.PlayableWordsSingleAndMultipleLetters(letters, boardLetters, multipleBoardLetters, true);
foreach (WordDictionary dictionary in words.WordDictionaries.Values)
{
    Console.WriteLine($"Dictionary for: {dictionary.Label}");
    foreach (var (word, score) in dictionary.Words)
    {
        Console.WriteLine(word + " " + score);
    }
}

words.PrintTopFewValues(3);
words.PrintTopValueForEach();
words.SortAllDictionariesByScore();

