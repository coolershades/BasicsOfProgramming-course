using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
	    {
		    var visitedCell = new HashSet<Point>() {start};
		    var queue = new Queue<SinglyLinkedList<Point>>();
		    queue.Enqueue(new SinglyLinkedList<Point>(start));

		    while (queue.Count != 0)
		    {
			    var currentPoint = queue.Dequeue();
			    var possibleMoves = GetNeighbours(currentPoint.Value)
				    .Where(p => map.InBounds(p)
				                 && map.Dungeon[p.X, p.Y] == MapCell.Empty
				                 && !visitedCell.Contains(p));

			    foreach (var point in possibleMoves)
			    {
				    queue.Enqueue(new SinglyLinkedList<Point>(point, currentPoint));
				    visitedCell.Add(point);
			    }

			    if (chests.Contains(currentPoint.Value)) yield return currentPoint;
		    }
	    }
	    
	    public static IEnumerable<Point> GetNeighbours(Point p)
	    {
		    int[] d = { -1, 0, 1 };
		    return d
			    .SelectMany(dx => d
				    .Where(dy => dx == 0 || dy == 0)
				    .Select(y => new Point(p.X + dx, p.Y + y)));
	    }
	}
}