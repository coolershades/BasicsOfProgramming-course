using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double GetMedian(int x, int y, double[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var allPixels = new List<double>();

            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (x + i >= 0 && x + i < width && y + j >= 0 && y + j < height)
                        allPixels.Add(original[x + i, y + j]);
			
            allPixels.Sort();
			
            if (allPixels.Count % 2 > 0)
                return allPixels[allPixels.Count / 2];
            return (allPixels[allPixels.Count / 2 - 1] + allPixels[allPixels.Count / 2]) / 2;
        }

        public static double[,] MedianFilter(double[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filteredImage = new double[width, height];
			
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                filteredImage[x, y] = GetMedian(x, y, original);

            return filteredImage;
        }
    }
}