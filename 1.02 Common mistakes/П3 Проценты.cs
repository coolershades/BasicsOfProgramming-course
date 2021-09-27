public static double Calculate(string userInput)
{
     var allData = userInput.Split(' ');
     var originalAmount = double.Parse(allData[0]);
     var interestRate = double.Parse(allData[1]);
     var depositTerm = double.Parse(allData[2]);
     return originalAmount * Math.Pow((interestRate / (12 * 100.0)) + 1, depositTerm);
}
   