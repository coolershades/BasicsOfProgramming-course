using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(new Vector()
            {
                X = End.X - Begin.X,
                Y = End.Y - Begin.Y
            });
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
    }

    public static class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            return GetLength(new Vector()
            {
                X = segment.Begin.X - segment.End.X,
                Y = segment.Begin.Y - segment.End.Y
            });
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
			var segment1 = new Segment () { Begin = segment.Begin, End = vector };
			var segment2 = new Segment () { Begin = segment.End, End = vector };
			
			return GetLength(segment1) + GetLength(segment2) == GetLength(segment);
        }

        public static Vector Add(Vector a, Vector b)
        {
            return new Vector() { X = a.X + b.X, Y = a.Y + b.Y};
        }
    } 
}