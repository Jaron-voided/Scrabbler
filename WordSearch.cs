namespace Scrabbler;

public static class WordSearch
{
    public static bool ContainsLetter(char letter, string word)
    {
        return word.Contains(letter);
    }
    
    public static bool ContainsLetters(List<char> letters, string word, bool hasWildcard)
    {
        bool wildCardUsed = false;
        if (letters.Count < word.Length) return false;
    
        List<char> remainingLetters = new List<char>(letters);
    
        foreach (char c in word)
        {
            if (remainingLetters.Contains(c))
            {
                remainingLetters.Remove(c);
            }
            else if (hasWildcard && !wildCardUsed)
            {
                wildCardUsed = true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}