namespace Mazes
{
	public static class EmptyMazeTask
	{
        public static void MoveToSide(Robot robot, int steps, Direction direction)
        {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(direction);
        }
		
        public static void MoveOut(Robot robot, int width, int height)
		{
            MoveToSide(robot, width - 3, Direction.Right);
            MoveToSide(robot, height - 3, Direction.Down);
        }
	}
}