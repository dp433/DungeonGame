using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class DungeonTask
	{
		public static MoveDirection[] FindShortestPath(Map map)
		{
			var shortestPaths = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit });
			if (shortestPaths.Any())
			{
				var shortestPath = shortestPaths.First();
				if (shortestPath.Any(point => map.Chests.Contains(point)))
					return GetMoveDirection(shortestPath.Reverse().ToArray());
				var pathsFromInitial = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
				var pathsFromExit = BfsTask.FindPaths(map, map.Exit, map.Chests);
				var pathsThroughChest = pathsFromInitial.Join(pathsFromExit,
					path1 => path1.Value,
					path2 => path2.Value,
					(path1, path2) => path1.Reverse().Concat(path2.Skip(1)))
					.OrderBy(p => p.Count());
				if (pathsThroughChest.Any())
					return GetMoveDirection(pathsThroughChest.First().ToArray());
				return GetMoveDirection(shortestPath.Reverse().ToArray());
			}
			return new MoveDirection[0];
		}

		private static MoveDirection[] GetMoveDirection(Point[] pointPath)
		{
			var directions = new Dictionary<Point, MoveDirection>
			{
				{ new Point(-1, 0), MoveDirection.Left},
				{ new Point(1, 0), MoveDirection.Right},
				{ new Point(0, -1), MoveDirection.Up},
				{ new Point(0, 1), MoveDirection.Down}
			};

			return pointPath
				.Skip(1)
				.Zip(pointPath, (p1, p2) => directions[new Point(p1.X - p2.X, p1.Y - p2.Y)])
				.ToArray();
		}
	}
}
