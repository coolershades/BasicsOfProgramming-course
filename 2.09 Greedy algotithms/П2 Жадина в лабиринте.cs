using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
 
namespace Greedy
{
    public class Cell
    {
        public readonly Cell Previous;
        public readonly Point Point;
        public readonly int Cost;
 
        public Cell(int cost, Point point, Cell prev)
        {
            Previous = prev;
            Cost = cost;
            Point = point;
        }
 
        public List<Point> GetPathToStart()
        {
            var result = new List<Point>();
            var current = this;
 
            while (current != null)
            {
                result.Add(current.Point);
                current = current.Previous;
            }
            result.Reverse();
            
            return result;
        }
    } 
 
    public class GreedyPathFinder : IPathFinder
    {
        private int energy;
        private Point startPos;
        private HashSet<Point> chests;
 
        public List<Point> FindPathToCompleteGoal(State state)
        {
            startPos = state.Position;
            chests = new HashSet<Point>(state.Chests);
 
            var resultDirection = new List<Point>();
 
            for (int i = 0; i < state.Goal; i++)
            {
                var newPath = GetMinDirToChest(state);
 
                if (energy > state.InitialEnergy || newPath == null)
                    return new List<Point>();
 
                resultDirection.AddRange(newPath);
            }
 
            return resultDirection;
        }
 
        private List<Point> GetMinDirToChest(State state)
        {
            var cells = new Cell[state.MapWidth, state.MapHeight];
            var queue = new Queue<Cell>();
            
            if (chests.Contains(startPos))
            {
                chests.Remove(startPos);
                return new List<Point>();
            }
            
            var minDir = AddNewCellsToQueue(state, cells, queue, null, 
                new Cell(0, startPos, null));
            while (queue.Count > 0)
            {
                var currentDir = queue.Dequeue();
                minDir = AddNewCellsToQueue(state, cells, queue, minDir, currentDir);
            }
            
            if (minDir == null) return null;
            chests.Remove(minDir.Point);
            energy += minDir.Cost;
            startPos = minDir.Point;
            return minDir.GetPathToStart();
        }

        private Cell AddNewCellsToQueue(State state, Cell[,] arr, 
            Queue<Cell> queue, Cell minDir, Cell currentDir)
        {
            foreach (var point in GetNearPoints(currentDir.Point))
            {
                if (!PointIsSuitable(state, point, arr, currentDir)) continue;
                
                var newDir = new Cell(state.CellCost[point.X, point.Y] + currentDir.Cost,
                    point, currentDir.Cost != 0 ? currentDir : null);
 
                if (minDir == null || minDir.Cost > newDir.Cost)
                {
                    queue.Enqueue(newDir);
                    arr[point.X, point.Y] = newDir;
                    if (chests.Contains(newDir.Point))
                        minDir = newDir;
                }
            }
            
            return minDir;
        }
        
        private IEnumerable<Point> GetNearPoints(Point area)
        {
            yield return new Point(area.X - 1, area.Y);
            yield return new Point(area.X + 1, area.Y);
            yield return new Point(area.X, area.Y - 1);
            yield return new Point(area.X, area.Y + 1);
        }
 
        private bool PointIsSuitable(State state, Point point, Cell[,] arr, Cell currentDir)
        {
            var x = point.X;
            var y = point.Y;
 
            return state.InsideMap(point) && 
                   !state.IsWallAt(point) &&
                   (arr[x, y] == null || 
                    arr[x, y].Cost > currentDir.Cost + state.CellCost[x, y]);
        }
    }
}