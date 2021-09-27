using System.Collections.Generic;
using Greedy.Architecture;
using System.Drawing;
 
namespace Greedy
{
    class PointData
    {
        public readonly Point Previous;
        public readonly int Weight;
 
        public PointData(Point previous, int weight = 0)
        {
            Previous = previous;
            Weight = weight;
        }
    }
 
    public class DijkstraPathFinder
    {
        private Dictionary<Point, PointData> visitedPoints;
        private HashSet<Point> targetsSet;
 
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            visitedPoints = new Dictionary<Point, PointData>();
            targetsSet = new HashSet<Point>(targets);
 
            var openPointsData = new Dictionary<Point, PointData>
            {
                [start] = new PointData(start)
            };
 
            while (openPointsData.Count != 0)
            {
                var current = GetCurrentPoint(openPointsData);
                visitedPoints.Add(current, openPointsData[current]);
                openPointsData.Remove(current);
 
                if (targetsSet.Contains(current))
                    yield return GetPathToStart(current, start);
 
                AddNear(state, current, openPointsData);
            }
        }
 
        private PathWithCost GetPathToStart(Point current, Point start)
        {
            var path = new PathWithCost(visitedPoints[current].Weight);
 
            while (current != start)
            {
                path.Path.Add(current);
                current = visitedPoints[current].Previous;
            }
 
            path.Path.Add(start);
            path.Path.Reverse();
 
            return path;
        }
 
        private Point GetCurrentPoint(Dictionary<Point, PointData> openPointsData)
        {
            var currentPoint = default(Point);
            var maxValue = int.MaxValue;
 
            foreach (var point in openPointsData)
                if (maxValue > point.Value.Weight)
                {
                    currentPoint = point.Key;
                    maxValue = point.Value.Weight;
                }
 
            return currentPoint;
        }
 
        private void AddNear(State state, Point current, IDictionary<Point, PointData> openPointsData)
        {
            foreach (var point in GetNearPoints(current))
            {
                if (visitedPoints.ContainsKey(point) || 
                    !state.InsideMap(point) || state.IsWallAt(point))
                    continue;
 
                var nearWidth = state.CellCost[point.X, point.Y] + visitedPoints[current].Weight;
 
                if (!openPointsData.ContainsKey(point) || nearWidth < openPointsData[point].Weight)
                    openPointsData[point] = new PointData(current, nearWidth);
            }
        }
 
        private IEnumerable<Point> GetNearPoints(Point area)
        {
            yield return new Point(area.X - 1, area.Y);
            yield return new Point(area.X + 1, area.Y);
            yield return new Point(area.X, area.Y - 1);
            yield return new Point(area.X, area.Y + 1);
        }
    }
}