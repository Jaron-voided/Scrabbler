using Scrabbler.Interfaces;

namespace Scrabbler;

public class WordScoring : IWordScoring
{
    public Dictionary<char, int> LetterValues => _letterValues;
    
    private readonly Dictionary<char, int> _letterValues = new Dictionary<char, int>
    {
        {'a', 1}, {'b', 3}, {'c', 3}, {'d', 2}, {'e', 1}, {'f', 4}, {'g', 2}, {'h', 4},
        {'i', 1}, {'j', 8}, {'k', 5}, {'l', 1}, {'m', 3}, {'n', 1}, {'o', 1}, {'p', 3},
        {'q', 10}, {'r', 1}, {'s', 1}, {'t', 1}, {'u', 1}, {'v', 4}, {'w', 4}, {'x', 8},
        {'y', 4}, {'z', 10}
    };

    public int CalculateScore(string word)
    {
        int score = 0;
        foreach (char c in word.ToLower())
        {
            if (_letterValues.ContainsKey(c)) score += _letterValues[c];
        }

        return score;
    }

    public WordDictionary AddScoresToWords(WordDictionary wordList)
    {
        WordDictionary wordsWithScore = new WordDictionary();
        
        foreach (var (word, _) in wordList.Words)
        {
            var score = CalculateScore(word);
            wordsWithScore.Words.Add(word, score);
        }
        return wordsWithScore;
    }

    public Dictionary<string, int> SortByScore(Dictionary<string, int> wordList)
    {
        Dictionary<string, int> wordsSortedByScore = wordList
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
        
        return wordsSortedByScore;
    }

    public KeyValuePair<string, int>? GetHighestScore(WordDictionary wordList)
    {
        WordDictionary playableWords = wordList;

        KeyValuePair<string, int>? highestScore = playableWords.GetHighestScoringWord();
        
        return highestScore;
    }
}