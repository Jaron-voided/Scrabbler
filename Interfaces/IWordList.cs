namespace Scrabbler.Interfaces;

public interface IWordList
{
    Dictionary<string, int> GetWordList();
    AllDictionaries GetPrunedWordList(List<char> singleBoardLetter);
    AllDictionaries GetPrunedWordList(List<string> multipleBoardLetter);
    AllDictionaries GetPrunedWordList(List<char> singleBoardLetter, List<string> multipleBoardLetter);
}