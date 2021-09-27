using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<string> GetWordsInSentence(string sentence)
        {
            var words = new List<string>();
            var tmpWord = new StringBuilder();

            for (int symbol = 0; symbol < sentence.Length; symbol++)
            {
                if (char.IsLetter(sentence[symbol]) || sentence[symbol] == '\'')
                {
                    tmpWord.Append(sentence[symbol]);
                    if (symbol == sentence.Length - 1)
                        words.Add(tmpWord.ToString().ToLower());
                }
                else if (tmpWord.Length > 0)
                {
                    words.Add(tmpWord.ToString().ToLower());
                    tmpWord.Clear();
                }
            }

            return words;
        }
        
        public static List<List<string>> ParseSentences(string text)
        {
            var sentenceDividers = new[] { '.', '!', '?', ';', ':', '(', ')' };
            var sentences = text.Split(sentenceDividers, StringSplitOptions.RemoveEmptyEntries);
            var sentencesList = new List<List<string>>();

            foreach (var sentence in sentences)
            {
                if (GetWordsInSentence(sentence).Count > 0)
                    sentencesList.Add(GetWordsInSentence(sentence));
            }
            return sentencesList;
        }
    }
}