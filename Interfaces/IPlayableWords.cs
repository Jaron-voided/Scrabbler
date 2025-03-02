namespace Scrabbler.Interfaces;

public interface IPlayableWords
{
    Dictionary<string, int> PlayableWordsDictionary(List<char> letters, bool hasWildcard);
    
    // Check if a specific word can be played using letters and a sequence on the board
    bool PlayableWithSequence(List<char> letters, string word, string sequence, bool hasWildcard);
    
    // Get words that can be played using single letters on the board
    AllDictionaries PlayableWordsSingleLetters(List<char> letters, List<char> boardLetters, bool hasWildcard);
    
    // Get words that can be played using multiple letter sequences on the board
    AllDictionaries PlayableWordsMultipleLetters(List<char> letters, List<string> multipleBoardLetters, bool hasWildcard);
    
    // Get words that can be played using both single letters and multiple letter sequences
    AllDictionaries PlayableWordsSingleAndMultipleLetters(
        List<char> letters, List<char> boardLetters, List<string> multipleBoardLetters, bool hasWildcard);
}
