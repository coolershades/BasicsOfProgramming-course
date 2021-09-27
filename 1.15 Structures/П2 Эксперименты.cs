using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            var counts = Constants.FieldCounts;
            foreach (var count in counts)
            {
                var classTime = new ExperimentResult(count,
                    benchmark.MeasureDurationInMs
                        (new ClassArrayCreationTask(count), repetitionsCount));
                var structTime = new ExperimentResult(count,
                    benchmark.MeasureDurationInMs
                        (new StructArrayCreationTask(count), repetitionsCount));
                
                classesTimes.Add(classTime);
                structuresTimes.Add(structTime);
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();
            
            var counts = Constants.FieldCounts;
            foreach (var count in counts)
            {
                var classTime = new ExperimentResult(count,
                    benchmark.MeasureDurationInMs
                        (new MethodCallWithClassArgumentTask(count), repetitionsCount));
                var structTime = new ExperimentResult(count,
                    benchmark.MeasureDurationInMs
                        (new MethodCallWithStructArgumentTask(count), repetitionsCount));
                
                classesTimes.Add(classTime);
                structuresTimes.Add(structTime);
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}