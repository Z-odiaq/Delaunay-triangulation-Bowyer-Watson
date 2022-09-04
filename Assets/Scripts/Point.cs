using System.Collections.Generic;
using UnityEngine;
//point class
public class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public List<Triangle> AdjacentTriangles { get; set; } = new List<Triangle>();
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
    public double DistanceSquared(Point other)
    {
        return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);
    }

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

}
