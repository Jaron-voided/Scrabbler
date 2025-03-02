using Scrabbler.Interfaces;

namespace Scrabbler;

public class PlayableWords : IPlayableWords
{
    private readonly IWordList _wordList;
    private readonly IWordSearch _wordSearch;
    private readonly IWordScoring _wordScoring;

    public PlayableWords(IWordList wordList, IWordSearch wordSearch, IWordScoring wordScoring)
    {
        _wordList = wordList;
        _wordSearch = wordSearch;
        _wordScoring = wordScoring;
    }

    public Dictionary<string, int> PlayableWordsDictionary(List<char> letters, bool hasWildcard)
    {
        Dictionary<string, int> words = _wordList.GetWordList();
        Dictionary<string, int> playableWords = new Dictionary<string, int>();
        
        foreach (var (word, _) in words)
        {
            if (_wordSearch.ContainsLetters(letters, word, hasWildcard))
            {
                int wordScore = _wordScoring.CalculateScore(word);

                if (!playableWords.ContainsKey(word))
                {
                    playableWords.Add(word, wordScore);
                }
            }
        }
        
        return playableWords;
    }
    
    // Sees if you can play a certain word with your characters and multiple letters on board already
    public bool PlayableWithSequence(List<char> letters, string word, string sequence, bool hasWildcard)
    {
        // Check if sequence appears as a continuous substring in the word
        if (!word.Contains(sequence))
        {
            return false;
        }
    
        // Get the index where the sequence appears
        int index = word.IndexOf(sequence);
    
        // Create the remaining word by removing the sequence
        string remainingWord = word.Remove(index, sequence.Length);
    
        // Check if the remaining letters can be formed from your hand
        return _wordSearch.ContainsLetters(letters, remainingWord, hasWildcard);
    }
    /*public bool PlayableWithSequence(List<char> letters, string word, string sequence, bool hasWildcard)
    {
        if (!word.Contains(sequence))
        {
            return false;
        }
        string remainingWord = word.Replace(sequence, "");
        return _wordSearch.ContainsLetters(letters, remainingWord, hasWildcard);
    }*/

    public AllDictionaries PlayableWordsSingleLetters(List<char> letters, List<char> boardLetters, bool hasWildcard)
    {
        AllDictionaries words = _wordList.GetPrunedWordList(boardLetters);
        AllDictionaries playableWords = new AllDictionaries();
    
        foreach (var dictionaryEntry in words.WordDictionaries)
        {
            string label = dictionaryEntry.Key;  // This is the single board letter
            WordDictionary wordDict = dictionaryEntry.Value;
        
            if (!playableWords.ContainsDictionary(label))
            {
                playableWords.AddDictionary(new WordDictionary(label));
            }
        
            foreach (var wordEntry in wordDict.Words)
            {
                string word = wordEntry.Key;
            
                // Create a list with your letters plus JUST THIS ONE board letter
                List<char> combinedLetters = new List<char>(letters);
                combinedLetters.Add(label[0]);  // Add only the single board letter
            
                // Check if the word can be formed with your hand plus this one board letter
                if (_wordSearch.ContainsLetters(combinedLetters, word, hasWildcard))
                {
                    int wordScore = _wordScoring.CalculateScore(word);
                
                    if (!playableWords.ContainsWord(word))
                    {
                        playableWords.WordDictionaries[label].Words.Add(word, wordScore);
                    }
                }
            }
        }
    
        return playableWords;
    }
    /*public AllDictionaries PlayableWordsSingleLetters(List<char> letters, List<char> boardLetters, bool hasWildcard)
    {
        AllDictionaries words = _wordList.GetPrunedWordList(boardLetters);
        AllDictionaries playableWords = new AllDictionaries();

        List<char> combinedLetters = new List<char>(letters);
        combinedLetters.AddRange(boardLetters);
        
        foreach (var dictionaryEntry in words.WordDictionaries)
        {
            string sequence = dictionaryEntry.Key;
            WordDictionary wordDict = dictionaryEntry.Value;
            
            if (!playableWords.ContainsDictionary(sequence))
            {
                playableWords.AddDictionary(new WordDictionary(sequence));
            }
            
            foreach (var wordEntry in wordDict.Words)
            {
                string word = wordEntry.Key;
            
                // Check if the word can be formed with the available letters
                if (_wordSearch.ContainsLetters(combinedLetters, word, hasWildcard))
                {
                    int wordScore = _wordScoring.CalculateScore(word);
                
                    // Add to the appropriate dictionary if not already present
                    if (!playableWords.ContainsWord(word))
                    {
                        playableWords.WordDictionaries[sequence].Words.Add(word, wordScore);
                    }
                }
            }
        }
    
        return playableWords;
    }*/

   public AllDictionaries PlayableWordsMultipleLetters(List<char> letters, List<string> multipleBoardLetters, bool hasWildcard)
    {
        // Get pruned word dictionaries
        AllDictionaries words = _wordList.GetPrunedWordList(multipleBoardLetters);
        AllDictionaries playableWords = new AllDictionaries();
        
        // Iterate through each dictionary in AllDictionaries
        foreach (var dictionaryEntry in words.WordDictionaries)
        {
            string sequence = dictionaryEntry.Key; // This is the sequence/sequence
            WordDictionary wordDict = dictionaryEntry.Value; // This is the dictionary for that sequence
            
            // Create a corresponding dictionary in playableWords if it doesn't exist
            if (!playableWords.ContainsDictionary(sequence))
            {
                playableWords.AddDictionary(new WordDictionary(sequence));
            }
            
            // Now check each word in this dictionary
            foreach (var wordEntry in wordDict.Words)
            {
                string word = wordEntry.Key;
                
                // Check if the word can be formed with the available letters
                if (PlayableWithSequence(letters, word, sequence, hasWildcard))
                {
                    int wordScore = _wordScoring.CalculateScore(word);
                    
                    // Add to the appropriate dictionary if not already present
                    if (!playableWords.ContainsWord(word))
                    {
                        playableWords.WordDictionaries[sequence].Words.Add(word, wordScore);
                    }
                }
            }
        }
        
        return playableWords;
    }

    public AllDictionaries PlayableWordsSingleAndMultipleLetters(
        List<char> letters, List<char> boardLetters, List<string> multipleBoardLetters, bool hasWildcard)
    {
        // Get words for single letters and multiple letters
        AllDictionaries playableSingleLetterWords = PlayableWordsSingleLetters(letters, boardLetters, hasWildcard);
        AllDictionaries playableMultipleLetterWords = PlayableWordsMultipleLetters(letters, multipleBoardLetters, hasWildcard);

        // Create a combined result dictionary
        AllDictionaries playableWords = new AllDictionaries();

        // First, add all single letter dictionaries
        foreach (var dictionaryEntry in playableSingleLetterWords.WordDictionaries)
        {
            string sequence = dictionaryEntry.Key;
            WordDictionary wordDict = dictionaryEntry.Value;

            // Add this dictionary to our result
            if (!playableWords.ContainsDictionary(sequence))
            {
                playableWords.AddDictionary(new WordDictionary(sequence));
            }

            // Add all words from this dictionary
            foreach (var wordEntry in wordDict.Words)
            {
                if (!playableWords.ContainsWord(wordEntry.Key))
                {
                    playableWords.WordDictionaries[sequence].Words.Add(wordEntry.Key, wordEntry.Value);
                }
            }
        }

        // Then add all multiple letter dictionaries
        foreach (var dictionaryEntry in playableMultipleLetterWords.WordDictionaries)
        {
            string sequence = dictionaryEntry.Key;
            WordDictionary wordDict = dictionaryEntry.Value;

            // Add this dictionary to our result if it doesn't exist yet
            if (!playableWords.ContainsDictionary(sequence))
            {
                playableWords.AddDictionary(new WordDictionary(sequence));
            }

            // Add all words from this dictionary
            foreach (var wordEntry in wordDict.Words)
            {
                if (!playableWords.ContainsWord(wordEntry.Key))
                {
                    playableWords.WordDictionaries[sequence].Words.Add(wordEntry.Key, wordEntry.Value);
                }
            }
        }

        return playableWords;
    }
}