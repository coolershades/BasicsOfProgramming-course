using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var wristX = x + Manipulator.Palm * Math.Cos(Math.PI - alpha);
            var wristY = y + Manipulator.Palm * Math.Sin(Math.PI - alpha);
            var shoulderToWristDistance = Math.Sqrt(wristX * wristX + wristY * wristY);
            
            var shoulder = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                shoulderToWristDistance,
                Manipulator.Forearm) + Math.Atan2(wristY, wristX);
            
            var elbow = TriangleTask.GetABAngle(
                Manipulator.UpperArm,
                Manipulator.Forearm,
                shoulderToWristDistance);

            var wrist = - alpha - shoulder - elbow;
            
            if (Double.IsNaN(shoulder) || double.IsNaN(elbow))
                return new[] { double.NaN, double.NaN, double.NaN };
            
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        public const int TestSeed = 518;
        public const int NumTests = 1000;

        [Test]
        public void TestMoveManipulatorTo()
        {
            var random = new Random(TestSeed);
            for (var testNo = 0; testNo < NumTests; testNo++)
            {
                var x = random.NextDouble();
                var y = random.NextDouble();
                var alpha = random.NextDouble();
                
                var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
                
                if (!Double.IsNaN(angles[0]))
                {
                    var joints = AnglesToCoordinatesTask.GetJointPositions(
                        angles[0], angles[1], angles[2]);
                    
                    Assert.AreEqual(joints[2].X, x, 1e-3);
                    Assert.AreEqual(joints[2].Y, y, 1e-3);
                }
            }
        }
    }
}