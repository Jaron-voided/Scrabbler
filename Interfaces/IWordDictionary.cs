namespace Scrabbler.Interfaces;

public interface IWordDictionary
{
    // Properties
    string Label { get; set; }
    string Header { get; set; }
    Dictionary<string, int> Words { get; set; }
    
    // Methods
    void SortByScore();
    Dictionary<string, int> GetTopWords(int count);
    KeyValuePair<string, int>? GetHighestScoringWord();
}