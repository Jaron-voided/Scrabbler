namespace Scrabbler.Interfaces;

public interface IWordSearch
{
    bool ContainsLetter(char letter, string word);

    bool ContainsLetters(List<char> letters, string word, bool hasWildcard);
}