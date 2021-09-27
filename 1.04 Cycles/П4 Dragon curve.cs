using System;
using System.Drawing;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static Tuple<double, double> Transform(double x, double y, bool randomCase)
		{
			double x1, y1, angle;
			if (randomCase)
				angle = Math.PI / 4;
			else
				angle = 3 * Math.PI / 4;

			x1 = (x * Math.Cos(angle) - y * Math.Sin(angle)) / Math.Sqrt(2);
			y1 = (x * Math.Sin(angle) + y * Math.Cos(angle)) / Math.Sqrt(2);

			if (!randomCase)
				x1 += 1;

			return Tuple.Create(x1, y1);
		}

		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
			double x = 1;
			double y = 0;

			var random = new Random(seed);

			for (int i = 0; i < iterationsCount; i++)
			{
				var nextNumber = random.Next(2);

				double x1 = Transform(x, y, nextNumber == 1).Item1;
				double y1 = Transform(x, y, nextNumber == 1).Item2;

				x = x1;
				y = y1;

				pixels.SetPixel(x, y);
			}
		}
	}
}