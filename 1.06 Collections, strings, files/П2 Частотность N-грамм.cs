using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, List<string>> GetNGrams(List<string> words)
        {
            var nGrams = new Dictionary<string, List<string>>();
            for (int i = 0; i < words.Count - 1; i++) 
            {
                var keyPhrase = words[i];
                // сначала обрабатывает биграмму, а затем -- триграмму, если она есть
                for (int j = 1; i + j < words.Count && j <= 2; j++)
                {
                    if (nGrams.ContainsKey(keyPhrase))
                        nGrams[keyPhrase].Add(words[i + j]);
                    else
                        nGrams.Add(keyPhrase, new List<string> { words[i + j] });
                    keyPhrase += " " + words[i + j];
                }
            }

            return nGrams;
        }

        public static Dictionary<string, Dictionary<string, int>> GetNGramsFrequencyDictionary(List<List<string>> text)
        {
            var allNGrams = new Dictionary<string, Dictionary<string, int>>();
            
            foreach (var sentence in text)
            {
                var nGrams = GetNGrams(sentence);
                foreach (var wordSuggestions in nGrams) 
                {
                    foreach (var suggestion in wordSuggestions.Value)
                        if (allNGrams.ContainsKey(wordSuggestions.Key))
                        {
                            if (allNGrams[wordSuggestions.Key].ContainsKey(suggestion))
                                allNGrams[wordSuggestions.Key][suggestion]++;
                            else
                                allNGrams[wordSuggestions.Key].Add(suggestion, 1);
                        }
                        else
                            allNGrams.Add(wordSuggestions.Key, new Dictionary<string, int>() {{suggestion, 1}});
                }
            }

            return allNGrams;
        }

        public static string GetMostFrequentSuggestion(
            KeyValuePair<string, Dictionary<string, int>> wordSuggestionAndFrequency)
        {
            var maxFrequency = 0;
            var mostFrequentSuggestion = "";
            foreach (var suggestionFrequency in wordSuggestionAndFrequency.Value)
            {
                if (suggestionFrequency.Value > maxFrequency)
                {
                    mostFrequentSuggestion = suggestionFrequency.Key;
                    maxFrequency = suggestionFrequency.Value;
                }
                else if (suggestionFrequency.Value == maxFrequency 
                         && string.CompareOrdinal(suggestionFrequency.Key, mostFrequentSuggestion) < 0)
                    mostFrequentSuggestion = suggestionFrequency.Key;
            }
            return mostFrequentSuggestion;
        }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var allNGrams = GetNGramsFrequencyDictionary(text);

            foreach (var wordSuggestionAndFrequency in allNGrams)
                result.Add(wordSuggestionAndFrequency.Key,
                    GetMostFrequentSuggestion(wordSuggestionAndFrequency));

            return result;
        }
    }
}