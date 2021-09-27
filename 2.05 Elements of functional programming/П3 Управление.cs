using System;

namespace func_rocket
{
	public class ControlTask
	{
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var rocketToTarget = target - rocket.Location;

			var difference = rocketToTarget.Angle - rocket.Direction;
			if (Math.Abs(difference) < 0.5 || Math.Abs(rocketToTarget.Angle - rocket.Velocity.Angle) < 0.5)
				difference = (difference + rocketToTarget.Angle - rocket.Velocity.Angle) / 2;

			if (difference < 0) return Turn.Left;
			if (difference > 0) return Turn.Right;
			return Turn.None;
		}
	}
}