using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var angle = shoulder;
            var elbowPos = new PointF((float) (Manipulator.UpperArm * Math.Cos(angle)), 
                (float) (Manipulator.UpperArm * Math.Sin(angle)));

            angle += elbow - Math.PI;
            var wristPos = new PointF((float) (elbowPos.X + Manipulator.Forearm * Math.Cos(angle)), 
                (float) (elbowPos.Y + Manipulator.Forearm * Math.Sin(angle)));
            
            angle += wrist - Math.PI;
            var palmEndPos = new PointF((float) (wristPos.X + Manipulator.Palm * Math.Cos(angle)), 
                (float) (wristPos.Y + Manipulator.Palm * Math.Sin(angle)));

            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(0, Math.PI, Math.PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI / 2, Manipulator.Palm, Manipulator.UpperArm + Manipulator.Forearm)]
        
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }

    [TestFixture]
    public class UpperArmCoordinates_Tests
    {
        [TestCase(Math.PI / 3, Manipulator.UpperArm * 0.5, Manipulator.UpperArm * 0.866)]
        [TestCase(-Math.PI / 6, Manipulator.UpperArm * 0.866, Manipulator.UpperArm * -0.5)]
        
        public void TestGetUpperArmPosition(double shoulder, double upperArmX, double upperArmY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, 0, 0);
            
            Assert.AreEqual(upperArmX, joints[0].X, 1e-2);
            Assert.AreEqual(upperArmY, joints[0].Y, 1e-2);
        }
    }
}