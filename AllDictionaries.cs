namespace Scrabbler;

public class AllDictionaries
{
    public Dictionary<string, WordDictionary> WordDictionaries = 
        new Dictionary<string, WordDictionary>();

    public void AddDictionary(WordDictionary dictionary)
    {
        WordDictionaries[dictionary.Label] = dictionary;   
    }
    public bool ContainsDictionary(string label)
    {
        return WordDictionaries.ContainsKey(label);
    }

    public void SortAllDictionariesByScore()
    {
        foreach (var wordDict in WordDictionaries.Values)
        {
            wordDict.SortByScore();
        }
    }

    public void PrintTopValueForEach()
    {
        foreach (var wordDict in WordDictionaries.Values)
        {
            Console.WriteLine($"Dictionary for {wordDict.Label}:");
            if (wordDict.Words.Count > 0)
            {
                var highestWord = wordDict.GetHighestScoringWord();
                Console.WriteLine($"  Top word: {highestWord.Value.Key} ({highestWord.Value.Value} points)");
            }
            else
            {
                Console.WriteLine("  No words in this dictionary.");
            }
        }
    }

    public void PrintTopFewValues(int count)
    {
        foreach (var wordDict in WordDictionaries.Values)
        {
            Console.WriteLine($"Dictionary for {wordDict.Label}:");
            Dictionary<string, int> values = wordDict.GetTopWords(count);
            foreach (var (word, score) in values)
            {
                Console.WriteLine($"{word}: {score}");
            }
        }
    }

    public bool ContainsWord(string word)
    {
        foreach (var wordDict in WordDictionaries.Values)
        {
            if (wordDict.Words.ContainsKey(word)) return true;
        }
        return false;
    }
    
}