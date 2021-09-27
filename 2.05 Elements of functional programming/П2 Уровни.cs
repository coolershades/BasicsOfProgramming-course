using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        private static readonly Rocket StandardRocket 
            = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

        private static readonly Vector StandardTarget
            = new Vector(600, 200);
		
        private static readonly Physics StandardPhysics = new Physics();

        public static IEnumerable<Level> CreateLevels()
        {
            yield return StandardLevelWithGravity("Zero", (size, location) => Vector.Zero);
            yield return StandardLevelWithGravity("Heavy", (size, location) => new Vector(0, 0.9));
			
            yield return new Level("Up",
                StandardRocket,
                new Vector(700, 500),
                (size, location) => new Vector(0, -300 / (size.Height - location.Y + 300.0)),
                StandardPhysics);
            
            yield return StandardLevelWithGravity("WhiteHole", AnomalyPhysics.WhiteHoleGravity);
            yield return StandardLevelWithGravity("BlackHole", AnomalyPhysics.BlackHoleGravity);
            yield return StandardLevelWithGravity("BlackAndWhite", AnomalyPhysics.BlackAndWhiteHoleGravity);
        }
        
        private static Level StandardLevelWithGravity(string name, Gravity gravity)
            => new Level(name, StandardRocket, StandardTarget, gravity, StandardPhysics);
        
        private static class AnomalyPhysics
        {
            public static Gravity WhiteHoleGravity = (size, location) =>
            {
                var targetToLocation = location - StandardTarget;
                var d = targetToLocation.Length;
                return targetToLocation.Normalize() * 140 * d / (d * d + 1);
            };
            
            public static Gravity BlackHoleGravity = (size, location) =>
            {
                var anomaly = (StandardTarget + StandardRocket.Location) / 2;
                var d = (location - anomaly).Length;
                return (anomaly - location).Normalize() * 300 * d / (d * d + 1);
            };
            
            public static Gravity BlackAndWhiteHoleGravity = (size, location)
                => (WhiteHoleGravity(size, location) + BlackHoleGravity(size, location)) / 2;
        }
    }
}