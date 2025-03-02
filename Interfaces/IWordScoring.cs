namespace Scrabbler.Interfaces;

public interface IWordScoring
{
    Dictionary<char, int> LetterValues { get; }    
    int CalculateScore(string word);
    WordDictionary AddScoresToWords(WordDictionary wordDictionary);
    Dictionary<string, int> SortByScore(Dictionary<string, int> wordList);
    KeyValuePair<string, int>? GetHighestScore(WordDictionary wordList);
}