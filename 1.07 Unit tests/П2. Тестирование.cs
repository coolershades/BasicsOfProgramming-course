[TestCase("text", new[] {"text"})]
[TestCase("hello world", new[] {"hello", "world"})]
[TestCase("\"\'\'\"", new[] {"\'\'"})]
[TestCase("'\"1\" \"2\"'", new[] {"\"1\" \"2\""})]
[TestCase("'' \"bcd ef\" 'x y'", new[] {"", "bcd ef", "x y"})]
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