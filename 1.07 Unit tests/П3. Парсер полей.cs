using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] {"text"})]
        [TestCase("hello world", new[] {"hello", "world"})]
        [TestCase("hello    world", new[] {"hello", "world"})]
        [TestCase("\"a 'b' 'c' d\"", new[] {"a 'b' 'c' d"})]
        [TestCase("'\"1\" \"2\"'", new[] {"\"1\" \"2\""})]
        [TestCase("\'\'", new[] {""})]
        [TestCase("a\"b\"c", new[] {"a", "b", "c"})]
        [TestCase("\"\"", new[] {""})]
        [TestCase("\'\'\"\"", new[] {"", ""})]
        [TestCase("\'\'\'", new[] {"", ""})] 
        [TestCase("'\"'", new[] {"\""})]
        [TestCase("\"\\\\\"", new[] {"\\"})]
        [TestCase("\\\\", new[] {"\\\\"})]
        [TestCase("\\\"\"", new[] {"\\", ""})] 
        [TestCase("  a b  ", new[] {"a", "b"})]
        [TestCase("'   a'", new[] {"   a"})]
        [TestCase(" ", new string[0])]
        [TestCase("\" ", new[] {" "})]
        [TestCase("\'\\\'\\\'\'", new[] {"\'\'"})]
        [TestCase("\"\\\"\\\"\"", new[] {"\"\""})]

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();
            var i = 0;
            while (i < line.Length)
            {
                if (line[i] == '\'' || line[i] == '\"' || line[i] != ' ')
                {
                    var tokenTmp = ReadUnspecifiedField(line, i);
                    i += tokenTmp.Length;
                    tokens.Add(tokenTmp);
                }
                else
                    i++;
            }
            
            return tokens;
        }

        public static Token ReadUnspecifiedField(string line, int startIndex)
        {
            if (line[startIndex] == '\'' || line[startIndex] == '\"')
                return ReadQuotedField(line, startIndex);
			
            return ReadField(line, startIndex);
        }
		
        private static Token ReadField(string line, int startIndex)
        {
            var field = new StringBuilder();
            for (var i = startIndex; i < line.Length && !IsDivider(line[i]); i++)
                field.Append(line[i]);
            
            return new Token(field.ToString(), startIndex, field.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex) => 
            QuotedFieldTask.ReadQuotedField(line, startIndex);

        public static bool IsDivider(char a) => a == '\'' || a == '\"' || a == ' ';
    }
}