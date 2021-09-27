using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static Tuple<string, string> GetLastWordPair(string[] words)
        {
            if (words.Length >= 2)
                return Tuple.Create(words[words.Length - 2], words[words.Length - 1]);
            return Tuple.Create("", words[0]);
        }
        
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var phrase = new StringBuilder().Append(phraseBeginning);
            var beginningWords = phraseBeginning.Split(' ');
            Tuple<string, string> lastWords = GetLastWordPair(beginningWords);
            
            for (var wordsInPhrase = beginningWords.Length; 
                wordsInPhrase < wordsCount + beginningWords.Length; 
                wordsInPhrase++)
            {  
                var keyToNextWord = "";
                if (wordsInPhrase >= 2 && nextWords.ContainsKey(lastWords.Item1 + " " + lastWords.Item2))
                    keyToNextWord = lastWords.Item1 + " " + lastWords.Item2;
                else if (nextWords.ContainsKey(lastWords.Item2))
                    keyToNextWord = lastWords.Item2;
                else 
                    break;
                
                phrase.Append(" " + nextWords[keyToNextWord]);
                lastWords = Tuple.Create(lastWords.Item2, nextWords[keyToNextWord]);
            }

            return phrase.ToString();
        }
    }
}