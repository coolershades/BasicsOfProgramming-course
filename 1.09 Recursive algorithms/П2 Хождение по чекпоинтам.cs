using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
	public static class PathFinderTask
	{
		static List<int> bestOrder = new List<int>();
		
		public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
		{
			var minSum = Double.MaxValue;
			MakeTrivialPermutation(new int[checkpoints.Length], 1, 
				checkpoints, ref minSum, 0);
			return bestOrder.ToArray();
		}

		private static void MakeTrivialPermutation(int[] permutation, int position, 
			Point[] checkpoints, ref double minSum, double currentSum)
		{
			if (position == permutation.Length)
			{
				bestOrder = permutation.ToList();
				minSum = currentSum;
				return;
			}

			for (int i = 0; i < permutation.Length; i++)
			{
				if (Array.IndexOf(permutation, i, 0, position) != -1) continue;
				permutation[position] = i;
				
				double updCurrentSum = currentSum;
				updCurrentSum += PointExtensions.DistanceTo(checkpoints[permutation[position - 1]],
					checkpoints[permutation[position]]);
				if (updCurrentSum > minSum) return;
				
				MakeTrivialPermutation(permutation, position + 1, 
					checkpoints, ref minSum, updCurrentSum);
			}
		}
	}
}