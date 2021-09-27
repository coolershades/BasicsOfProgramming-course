using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            task.Run();
            
            var timer = new Stopwatch();
            timer.Start();
            for (var count = 0; count < repetitionCount; count++)
                task.Run(); 
            timer.Stop();
            
            var averageTime = (double) timer.ElapsedMilliseconds / repetitionCount;
            return averageTime;
		}
	}

    internal class StringBuildTest : ITask
    {
        private const int RepetitionCount = 10000;
        
        public void Run()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < RepetitionCount; i++)
                sb.Append('a');
            var str = sb.ToString();
        }
    }

    internal class StringConstructorTest : ITask
    {
        private const int RepetitionCount = 10000;
        
        public void Run()
        {
            var str = new string('a', RepetitionCount);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();
            var stringBuilderTime = benchmark.MeasureDurationInMs
                (new StringBuildTest(), 5000);
            var stringConstructorTime = benchmark.MeasureDurationInMs
                (new StringConstructorTest(), 5000);

            Assert.Less(stringConstructorTime, stringBuilderTime);
        }
    }
}