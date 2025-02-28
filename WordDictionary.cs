namespace Scrabbler;

public class WordDictionary
{
    private string _label;
    
    public string Label 
    { 
        get => _label; 
        set 
        { 
            _label = value;
            Header = $"Words you can make with the letter {_label}";
        } 
    }    
    
    public string Header = "Words you can make with the letter ${label}";
    public Dictionary<string, int> Words = new Dictionary<string, int>();
    
    // Default constructor
    public WordDictionary()
    {
    }
    
    // Constructor with label parameter
    public WordDictionary(string label)
    {
        Label = label; // This will also set the Header
    }
    
    // Constructor with label and initial words
    public WordDictionary(string label, Dictionary<string, int> initialWords)
    {
        Label = label;
        Words = new Dictionary<string, int>(initialWords); // Create a copy of the dictionary
    }
    
    public void SortByScore()
    {
        // Create a new dictionary sorted by values (scores) in descending order
        Words = Words
            .OrderByDescending(pair => pair.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    
    public Dictionary<string, int> GetTopWords(int count)
    {
        return Words
            .OrderByDescending(pair => pair.Value)
            .Take(count)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    
    public KeyValuePair<string, int>? GetHighestScoringWord()
    {
        if (Words.Count == 0)
        {
            return null;
        }
            
        return Words.OrderByDescending(pair => pair.Value).First();
    }
    
}