using System;
using MPoint = System.Windows.Point;
using Dpoint = System.Drawing.Point;

namespace APP.Helpers
{
    public static class PointHelper
    {
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static double GetDistance(this MPoint p1, MPoint p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double GetDistance(this MPoint p1, Dpoint p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double GetDistance(this Dpoint p1, MPoint p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double GetDistance(this Dpoint p1, Dpoint p2)
        {
            return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }
    }
}