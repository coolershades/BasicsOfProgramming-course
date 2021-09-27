using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("0123'", 4, "", 1)]
        [TestCase("01'abc", 2, "abc", 4)]
        [TestCase(@"'abc\""'00", 0, "abc\"", 7)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            if (startIndex == line.Length - 1) // случай, когда открвающая кавычка стоит в конце строки
                return new Token("", startIndex, 1);
            
            var value = new StringBuilder();
            var currentIndex = startIndex + 1;

            while (currentIndex < line.Length && line[currentIndex] != line[startIndex])
            {
                if (line[currentIndex] == '\\')
                    value.Append(line[++currentIndex]);
                else
                    value.Append(line[currentIndex]);
                currentIndex++;
            }
            
            var factLength = currentIndex - startIndex;
            if (currentIndex < line.Length) // если есть закрывающая кавычка
                factLength++;

            return new Token(value.ToString(), startIndex, factLength);
        }
    }
}