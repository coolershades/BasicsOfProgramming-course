using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		public static double GetDistance(double x1, double y1, double x2, double y2)
		{
			// Метод расчитывает расстояние от точки 1 до точки 2
			return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
		}

		public static double GetDistanceToSegment(double xa, double ya, double xb, double yb, double x, double y)
		{
			// Для удобства будем называть точку Х точкой С.
			// AB == c, AC = b, BC = a;

			double a, b, c;
			a = GetDistance(xb, yb, x, y);
			b = GetDistance(xa, ya, x, y);
			c = GetDistance(xa, ya, xb, yb);

			// Проверяем, лежит ли точка в области между прямыми, 
			// перпеникулярными отрезку и проходящими через вершины отрезка
			if (a * a + c * c >= b * b && b * b + c * c >= a * a && a != 0 && c != 0) 
			{
				double cosb = (a * a + c * c - b * b) / (2.0 * a * c);
                return a * Math.Sqrt(1 - cosb * cosb);
			}
			else
				return Math.Min(GetDistance(x, y, xa, ya), GetDistance(x, y, xb, yb));
		}
	}
}