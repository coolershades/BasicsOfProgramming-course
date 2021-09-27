using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static string[] GetDaysLabels()
        {
            string[] days = new string[30];
            for (int i = 0; i < 30; i++)
                days[i] = (i + 2).ToString();
            return days;
        }

        public static string[] GetMonthsLabels()
        {
            string[] months = new string[12];
            for (int i = 0; i < 12; i++)
                months[i] = (i + 1).ToString();
            return months;
        }

        public static double[,] GetHeat(NameData[] names)
        {
            double[,] heat = new double[30, 12];
            for (int i = 0; i < names.Length; i++)
                if (names[i].BirthDate.Day != 1)
                    heat[names[i].BirthDate.Day - 2, names[i].BirthDate.Month - 1]++;
            return heat;
        }
		
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            return new HeatmapData(
                "Пример карты интенсивностей",
                GetHeat(names),
                GetDaysLabels(),
                GetMonthsLabels());
        }
    }
}