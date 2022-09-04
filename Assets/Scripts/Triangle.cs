using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Triangle
{
    public Point[] Vertices { get; } = new Point[3];
    public Point Circumcenter { get; private set; }
    public double RadiusSquared;
    public double Radius;
    public Triangle(Point p1, Point p2, Point p3)
    {
        string s = p1.X + " " + p1.Y + " " + p2.X + " " + p2.Y + " " + p3.X + " " + p3.Y;
        Vertices[0] = p1;
        Vertices[1] = p2;
        Vertices[2] = p3;
        Circumcenter = CalculateCircumcenter();
        RadiusSquared = Circumcenter.DistanceSquared(p1);
    }
    private Point CalculateCircumcenter()
    {
        var a = Vertices[0];
        var b = Vertices[1];
        var c = Vertices[2];
        var d = (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) * 2;
        var x = (a.X * a.X + a.Y * a.Y) * (b.Y - c.Y) + (b.X * b.X + b.Y * b.Y) * (c.Y - a.Y) + (c.X * c.X + c.Y * c.Y) * (a.Y - b.Y);
        var y = (a.X * a.X + a.Y * a.Y) * (c.X - b.X) + (b.X * b.X + b.Y * b.Y) * (a.X - c.X) + (c.X * c.X + c.Y * c.Y) * (b.X - a.X);
        var z = (a.X * a.X + a.Y * a.Y) * (a.X - b.X) + (b.X * b.X + b.Y * b.Y) * (b.X - c.X) + (c.X * c.X + c.Y * c.Y) * (c.X - a.X);
        return new Point(x / d, y / d);
    }
    public bool SharesEdgeWith(Triangle triangle)
    {
        var sharedVertices = Vertices.Where(o => triangle.Vertices.Contains(o)).Count();
        return sharedVertices == 2;
    }
    public bool IsPointInCircumcircle(Point point)
    {
        return point.DistanceSquared(Circumcenter) <= RadiusSquared;
    }

}
