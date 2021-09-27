using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static string[] GetLabels()
        {
            string[] days = new string[31];
            for (int i = 0; i < 31; i++)
                days[i] = (i + 1).ToString();
            return days;
        }

        public static double[] GetValues(NameData[] names, string name)
        {
            double[] amounts = new double[31];
            for (int i = 0; i < names.Length; i++)
                if (names[i].Name == name && names[i].BirthDate.Day != 1)
                    amounts[names[i].BirthDate.Day - 1]++;
            return amounts;
        }

        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), 
                GetLabels(), // x labels
                GetValues(names, name)); // y values
        }
    }
}