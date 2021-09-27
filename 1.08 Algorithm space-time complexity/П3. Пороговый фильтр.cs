using System.Collections.Generic;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double GetT(double[,] original, double whitePixelsFraction)
		{
			var allPixels = new List<double>();
			foreach (var pixel in original)
				allPixels.Add(pixel);
			
			allPixels.Sort();
			allPixels.Reverse();

			var n = (int) (whitePixelsFraction * original.GetLength(0) * original.GetLength(1));
			return n > 0 ? allPixels[n - 1] : 256;
		}
		
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var filteredImage = new double[width, height];

			var t = GetT(original, whitePixelsFraction);
			
			for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				filteredImage[x, y] = original[x, y] >= t ? 1.0 : 0.0;

			return filteredImage;
		}
	}
}