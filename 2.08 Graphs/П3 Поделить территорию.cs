using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace Rivals
{
	public class MapCellInfo
	{
		public readonly Point Cell;
		public readonly int Owner;
		public readonly int DistanceFromStart;

		public MapCellInfo(Point cell, int owner, int distanceFromStart)
		{
			Cell = cell;
			Owner = owner;
			DistanceFromStart = distanceFromStart;
		}
	}
	
	public static class MapExtensions
	{
		public static bool MoveIsPossible(this Map map, Point cell)
			=> map.InBounds(cell) && map.Maze[cell.X, cell.Y] == MapCell.Empty;

		public static Queue<MapCellInfo> InitializePlayers(this Map map)
		{
			var queue = new Queue<MapCellInfo>();
			for (var i = 0; i < map.Players.Length; i++)
				queue.Enqueue(new MapCellInfo(map.Players[i], i, 0));
			return queue;
		}
	}
	
	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var queue = map.InitializePlayers();
			var visitedCells = new HashSet<Point>();

			while (queue.Count != 0)
			{
				var currentMapCellInfo = queue.Dequeue();
				var cell = currentMapCellInfo.Cell;
				if (!map.MoveIsPossible(cell) || visitedCells.Contains(cell)) continue;
				visitedCells.Add(cell);
				
				var possibleMoves = GetNeighbours(cell);
				foreach (var move in possibleMoves)
					queue.Enqueue(new MapCellInfo(
						move,
						currentMapCellInfo.Owner, 
						currentMapCellInfo.DistanceFromStart + 1));
				
				yield return new OwnedLocation(
					currentMapCellInfo.Owner, 
					new Point(cell.X, cell.Y), 
					currentMapCellInfo.DistanceFromStart);
			}
		}

		private static IEnumerable<Point> GetNeighbours(Point p)
		{
			int[] d = { -1, 0, 1 };
			return d
				.SelectMany(dx => d
					.Where(dy => dx == 0 || dy == 0)
					.Select(dy => new Point(p.X + dx, p.Y + dy)));
		}
	}
}