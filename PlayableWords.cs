namespace Scrabbler;

public static class WordsToPlay
{

    public static Dictionary<string, int> PlayableWords(List<char> letters, bool hasWildcard)
    {
        Dictionary<string, int> words = WordList.GetWordList();
        Dictionary<string, int> playableWords = new Dictionary<string, int>();
        
        foreach (var (word, _) in words)
        {
            if (WordSearch.ContainsLetters(letters, word, hasWildcard))
            {
                int wordScore = WordScoring.CalculateScore(word);

                if (!playableWords.ContainsKey(word))
                {
                    playableWords.Add(word, wordScore);
                }
            }
        }
        
        return playableWords;
    }
    
    // Sees if you can play a certain word with your characters and multiple letters on board already
    public static bool PlayableWithSequence(List<char> letters, string word, string sequence, bool hasWildcard)
    {
        string remainingWord = word.Replace(sequence, "");
        return WordSearch.ContainsLetters(letters, remainingWord, hasWildcard);
    }

    
    public static AllDictionaries PlayableWordsSingleLetters(List<char> letters, List<char> boardLetters, bool hasWildcard)
    {
        AllDictionaries words = WordList.GetPrunedWordList(boardLetters);
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
                if (WordSearch.ContainsLetters(combinedLetters, word, hasWildcard))
                {
                    int wordScore = WordScoring.CalculateScore(word);
                
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

   public static AllDictionaries PlayableWordsMultipleLetters(List<char> letters, List<string> multipleBoardLetters, bool hasWildcard)
    {
        // Get pruned word dictionaries
        AllDictionaries words = WordList.GetPrunedWordList(multipleBoardLetters);
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
                    int wordScore = WordScoring.CalculateScore(word);
                    
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

    public static AllDictionaries PlayableWordsSingleAndMultipleLetters(
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