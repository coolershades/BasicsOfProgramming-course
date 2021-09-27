using System.Drawing;

namespace func_rocket
{
    public class ForcesTask
    {
        public static RocketForce GetThrustForce(double forceValue)
        {
            return rocket => new Vector(forceValue, 0).Rotate(rocket.Direction);
        }
		
        public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
        {
            return rocket => gravity(spaceSize, rocket.Location);  
        }
		
        public static RocketForce Sum(params RocketForce[] forces)
        {
            return rocket =>
            {
                var sum = new Vector(0, 0);
                foreach (var force in forces)
                    sum += force(rocket);
                return sum;
            };
        }
    }
}