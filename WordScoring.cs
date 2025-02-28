namespace Scrabbler;

public class WordScoring
{
    public static Dictionary<char, int> letterValues = new Dictionary<char, int>
    {
        {'a', 1}, {'b', 3}, {'c', 3}, {'d', 2}, {'e', 1}, {'f', 4}, {'g', 2}, {'h', 4},
        {'i', 1}, {'j', 8}, {'k', 5}, {'l', 1}, {'m', 3}, {'n', 1}, {'o', 1}, {'p', 3},
        {'q', 10}, {'r', 1}, {'s', 1}, {'t', 1}, {'u', 1}, {'v', 4}, {'w', 4}, {'x', 8},
        {'y', 4}, {'z', 10}
    };

    public static int CalculateScore(string word)
    {
        int score = 0;
        foreach (char c in word.ToLower())
        {
            if (letterValues.ContainsKey(c)) score += letterValues[c];
        }

        return score;
    }

    public static WordDictionary AddScoresToWords(WordDictionary wordList)
    {
        WordDictionary wordsWithScore = new WordDictionary();
        
        foreach (var (word, _) in wordList.Words)
        {
            var score = CalculateScore(word);
            wordsWithScore.Words.Add(word, score);
        }
        return wordsWithScore;
    }

    public static Dictionary<string, int> SortByScore(Dictionary<string, int> wordList)
    {
        Dictionary<string, int> wordsSortedByScore = wordList
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
        
        return wordsSortedByScore;
    }

    public static KeyValuePair<string, int>? GetHighestScore(WordDictionary wordList)
    {
        WordDictionary playableWords = wordList;

        KeyValuePair<string, int>? highestScore = playableWords.GetHighestScoringWord();
        
        return highestScore;
    }
}