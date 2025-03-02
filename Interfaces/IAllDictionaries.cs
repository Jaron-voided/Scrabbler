namespace Scrabbler.Interfaces;

public interface IAllDictionaries
{
    Dictionary<string, WordDictionary> WordDictionaries { get; set; }
    void AddDictionary(WordDictionary dictionary);
    bool ContainsDictionary(string label);
    void SortAllDictionariesByScore();
    void PrintTopValueForEach();
    void PrintTopFewValues(int count);
    bool ContainsWord(string word);
}