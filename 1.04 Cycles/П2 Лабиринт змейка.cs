using System.Runtime.ExceptionServices;

namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveToSide(Robot robot, int steps, Direction direction)
        {
			for (int i = 0; i < steps; i++)
				robot.MoveTo(direction);
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
			// loops -- кол-во петель, если считать по правой стороне лабиринта
			for (int loops = (height - 1) / 4; loops > 0; loops--)
            {
				MoveToSide(robot, width - 3, Direction.Right);
				MoveToSide(robot, 2, Direction.Down);
				MoveToSide(robot, width - 3, Direction.Left);
				if (loops > 1)
					MoveToSide(robot, 2, Direction.Down);
            }
		}
	}
}