using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c < 0
                || a + b < c || a + c < b || b + c < a)
                return Double.NaN;
            
            var cosC = (a * a + b * b - c * c) / (2 * a * b);
            return Math.Acos(cosC);
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(2, 4, 3.46410162, Math.PI / 3)]
        [TestCase(4, 3.46410162, 2, Math.PI / 6)]
        
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var actualAngle = TriangleTask.GetABAngle(a, b, c);
            
            Assert.AreEqual(actualAngle, expectedAngle, 1e-5);
        }
    }
}