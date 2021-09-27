using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		const double G = 9.8;
		public static double FindSightAngle(double v, double distance)
		{
			return Math.Asin((distance * G) / (v * v)) / 2;
		}
	}
}