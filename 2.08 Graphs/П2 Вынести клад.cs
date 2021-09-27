using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
	    private class PathWithChest
	    {
		    public readonly SinglyLinkedList<Point> StartToChest;
		    public readonly SinglyLinkedList<Point> ChestToExit;

		    public PathWithChest(SinglyLinkedList<Point> startToChest, SinglyLinkedList<Point> chestToExit)
		    {
			    StartToChest = startToChest;
			    ChestToExit = chestToExit;
		    }
	    }
	    
	    public static MoveDirection[] FindShortestPath(Map map)
        {
	        var bestPath = BfsTask.FindPaths(map, map.InitialPosition, new[] {map.Exit}).FirstOrDefault();
	        if (bestPath == null) return new MoveDirection[0];

	        if (map.Chests.Any(chest => bestPath.Contains(chest)))
		        return GetDirections(bestPath);

	        var pathsToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
	        var pathsWithChests = pathsToChests
		        .Select(path => new PathWithChest(path,
			        BfsTask.FindPaths(map, path.Value, new[] {map.Exit}).FirstOrDefault()));

	        var shortestPathWithChest = GetShortestPath(pathsWithChests);
	        if (shortestPathWithChest == null) return GetDirections(bestPath);

	        return GetDirections(shortestPathWithChest.StartToChest).Concat(
			        GetDirections(shortestPathWithChest.ChestToExit))
		        .ToArray();
        }

	    private static PathWithChest GetShortestPath(IEnumerable<PathWithChest> pathsToExit)
        {
	        var paths = pathsToExit.ToArray();
	        if (!paths.Any() || paths.First().ChestToExit == null) return null;
            
	        var minPathLength = int.MaxValue;
	        var bestPath = paths.First();
            
	        foreach (var path in paths)
	        {
		        if (path.StartToChest.Length + path.ChestToExit.Length >= minPathLength) continue;
		        
		        minPathLength = path.StartToChest.Length + path.ChestToExit.Length;
		        bestPath = path;
	        }

	        return bestPath;
        }

	    private static MoveDirection[] GetDirections(SinglyLinkedList<Point> list)
	    {
		    var result = new LinkedList<MoveDirection>();
		    var currentValue = list.Value;
			
		    var previousList = list;
			
		    while (previousList.Previous != null)
		    {
			    previousList = previousList.Previous;
			    var difference = TakeDifference(previousList.Value, currentValue);
				
			    result.AddFirst(pointToDirection[difference]);
			    currentValue = previousList.Value;
		    }
			
		    return result.ToArray();
	    }

	    private static readonly Dictionary<Point, MoveDirection> pointToDirection = new Dictionary<Point, MoveDirection>
	    {
		    {new Point(1, 0), MoveDirection.Right},
		    {new Point(0, -1), MoveDirection.Up},
		    {new Point(-1, 0), MoveDirection.Left},
		    {new Point(0, 1), MoveDirection.Down},
	    };

	    private static Point TakeDifference(Point p1, Point p2) => new Point(p2.X - p1.X, p2.Y - p1.Y);
    }
}