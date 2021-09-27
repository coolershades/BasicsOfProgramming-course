using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
	public class NotGreedyPathFinder : IPathFinder
	{
		private List<Point> bestPath = new List<Point>();
		private int bestChestsCount;
		private int chestCount;

		public List<Point> FindPathToCompleteGoal(State state)
		{
			chestCount = state.Chests.Count;
			
			FindNextOptimalPath(state, state.Position, 0, 0,
				state.Chests, new List<Point>());
			return bestPath;
		}

		private void FindNextOptimalPath(
			State state,
			Point start,
			int energyUsed,
			int foundChestsCount,
			List<Point> availableChests,
			List<Point> pathTravelled)
		{
			if (foundChestsCount > bestChestsCount)
			{
				bestChestsCount = foundChestsCount;
				bestPath = pathTravelled;
			}
			
			if (foundChestsCount == chestCount || bestChestsCount == chestCount) return;
			
			var finder = new DijkstraPathFinder();
			var pathsToChests = finder.GetPathsByDijkstra(state, start, availableChests)
				.Where(path => path.Cost + energyUsed <= state.Energy);
			
			foreach (var path in pathsToChests)
			{
				availableChests.Remove(path.End);
				
				FindNextOptimalPath(state, path.End, path.Cost + energyUsed, foundChestsCount + 1, 
					availableChests, pathTravelled.Concat(path.Path.Skip(1)).ToList());
				
				availableChests.Add(path.End);
			}
		}
	}
}