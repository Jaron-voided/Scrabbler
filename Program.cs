
using Scrabbler;

using Scrabbler.Interfaces;

IAllDictionaries allDictionaries = new AllDictionaries();
IWordDictionary wordDictionary = new WordDictionary();
IWordList wordList = new WordList();
IWordScoring wordScoring = new WordScoring();
IWordSearch wordSearch = new WordSearch();
IPlayableWords playableWords = new PlayableWords(wordList, wordSearch, wordScoring);

Game game = new Game(playableWords, wordList, wordScoring, allDictionaries, wordDictionary, wordSearch);
game.Run();

