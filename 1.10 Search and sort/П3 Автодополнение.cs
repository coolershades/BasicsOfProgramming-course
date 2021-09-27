using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var first = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            count = Math.Min(count, GetCountByPrefix(phrases, prefix));
            var topStrings = new string[count];

            for (var i = 0; i < count; i++)
                topStrings[i] = phrases[first + i];

            return topStrings;
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            return right - left - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            var phrases = new List<string>();
            var prefix = "aa";

            var topPhrases = AutocompleteTask.GetTopByPrefix(phrases, prefix, 3);
            
            CollectionAssert.IsEmpty(topPhrases);
        }
        
        [Test]
        public void TopByPrefix_Ordinary()
        {
            var phrases = new List<string> {"a", "aaa", "aab", "aacg", "bd"};
            var prefix = "aa";

            var actualTopPhrases = AutocompleteTask.GetTopByPrefix(phrases, prefix, 2);
            var expectedTopPhrases = new string[] {"aaa", "aab"};
            
            CollectionAssert.AreEqual(expectedTopPhrases, actualTopPhrases);
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            var phrases = new List<string> {"aa", "ab", "ac"};
            var prefix = "";

            var count = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            
            Assert.AreEqual(phrases.Count, count);
        }

        [Test]
        public void CountByPrefix_Ordinary()
        {
            var phrases = new List<string> {"a", "aaa", "aab", "aacg", "bd"};
            var prefix = "aa";

            var actualPhraseCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            
            Assert.AreEqual(3, actualPhraseCount);
        }
    }
}