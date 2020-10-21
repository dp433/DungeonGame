using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public static class BfsTask
	{
	    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
			var way = new SinglyLinkedList<Point>(start);
			var visited = new HashSet<Point> { start };
			var queue = new Queue<SinglyLinkedList<Point>>();

			queue.Enqueue(way);

			while(queue.Count != 0)
			{
				var point = queue.Dequeue();
				var moves = point.Value.GetMoves(map).Where(move => !visited.Contains(move));

				foreach(var move in moves)
				{
					way = new SinglyLinkedList<Point>(move, point);
					visited.Add(move);
					queue.Enqueue(way);
					if (chests.Contains(move))
						yield return way;
				}
			}

            yield break;
		}

		public static IEnumerable<Point> GetMoves(this Point point, Map map)
		{
			var range = Enumerable.Range(-1, 3);

			return range
				.SelectMany(x => range.Select(y => new Point(point.X + x, point.Y + y)))
				.Where(move => move.IsValidMove(point, map));
		}

		private static bool IsValidMove(this Point point, Point other, Map map) =>
			point != other &&
			(point.X - other.X == 0 || point.Y - other.Y == 0) &&
			map.InBounds(point) &&
			map.Dungeon[point.X, point.Y] != MapCell.Wall;
	}
}