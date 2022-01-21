using System;
using System.Collections.Generic;

namespace PylonGameEngine.Mathematics
{
    public class BezierCurve2D
    {
        public List<Vector2> Points;

        public BezierCurve2D(Vector2 StartPoint)
        {
            Points = new List<Vector2>() { StartPoint };
        }

        public BezierCurve2D(List<Vector2> points)
        {
            Points = points;
        }

        public BezierCurve2D(List<BezierSegment2D> segments)
        {
            foreach (var segment in segments)
            {
                AddSegment(segment);
            }
        }

        public void AddSegment(BezierSegment2D segment)
        {
            Points.Add(segment.Point1);
            Points.Add(segment.ControlPoint);
            Points.Add(segment.Point2);
        }

        public void JoinNewSegment(Vector2 ControlPoint, Vector2 Point2)
        {
            if (Points.Count == 0)
                throw new Exception("Must be one or more Points in Curve");
            //     AddSegment(new BezierSegment2D(Points.Last(), ControlPoint, Point2));
            Points.Add(ControlPoint);
            Points.Add(Point2);
        }

        public List<BezierSegment2D> ToIndividualSegments()
        {
            //if ((Points.Count + 1) % 3 != 0)
            //    throw new Exception("Invalid Bezier Curve");
            var Segments = new List<BezierSegment2D>();
            for (int i = 0; i <= Points.Count - 2; i += 2)
            {
                Segments.Add(new BezierSegment2D(Points[i], Points[i + 1], Points[i + 2]));
            }
            return Segments;
        }
    }

    public struct BezierSegment2D
    {
        public Vector2 Point1;
        public Vector2 ControlPoint;
        public Vector2 Point2;

        public BezierSegment2D(Vector2 P1, Vector2 CP, Vector2 P2)
        {
            Point1 = P1;
            ControlPoint = CP;
            Point2 = P2;
        }
    }
}
