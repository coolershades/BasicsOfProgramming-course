using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			return (r1.Left + r1.Width >= r2.Left && r1.Left <= r2.Left
				|| r2.Left + r2.Width >= r1.Left && r2.Left <= r1.Left)
				&& (r2.Top <= r1.Top + r1.Height && r1.Top <= r2.Top
				|| r1.Top <= r2.Top + r2.Height && r2.Top <= r1.Top);
		}

		// Площадь пересечения прямоугольников
		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			int a, b, c, d;
			a = Math.Max(r1.Left, r2.Left);
			b = Math.Min(r1.Left + r1.Width, r2.Left + r2.Width);
			c = Math.Min(r1.Top + r1.Height, r2.Top + r2.Height);
			d = Math.Max(r1.Top, r2.Top);

			if (a < b && d < c)
				return (b - a) * (c - d);
			else
				return 0;
		}

		// Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
		// Иначе вернуть -1
		// Если прямоугольники совпадают, можно вернуть номер любого из них.
		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			if (r1.Top >= r2.Top && r1.Left >= r2.Left 
				&& r1.Top + r1.Height <= r2.Top + r2.Height 
				&& r1.Left + r1.Width <= r2.Left + r2.Width)
				return 0;
			if (r2.Top >= r1.Top && r2.Left >= r1.Left
				&& r2.Top + r2.Height <= r1.Top + r1.Height 
				&& r2.Left + r2.Width <= r1.Left + r1.Width)
				return 1;
			return -1;
		}
	}
}