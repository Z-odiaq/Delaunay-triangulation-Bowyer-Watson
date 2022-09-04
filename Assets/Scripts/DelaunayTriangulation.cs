

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DelaunayTriangulation : MonoBehaviour
{

    public List<Point> points;
    public int radius = 200;
    public int rejectionSamples = 10;
    public Vector2 regionSize = new Vector2(1000, 1000);
    void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }
    public Triangle SuperTriangle(List<Point> points)
    {
        // Find the maximum and minimum vertex bounds. 
        // This is to allow calculation of the bounding triangle 
        double minX = points[0].X, maxX = points[0].X;
        double minY = points[0].Y, maxY = points[0].Y;
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].X < minX) minX = points[i].X;
            if (points[i].X > maxX) maxX = points[i].X;
            if (points[i].Y < minY) minY = points[i].Y;
            if (points[i].Y > maxY) maxY = points[i].Y;
        }

        double dx = maxX - minX;
        double dy = maxY - minY;
        double deltaMax = Math.Max(dx, dy);
        double midx = (minX + maxX) / 2;
        double midy = (minY + maxY) / 2;

        // Set up the supertriangle 
        // This is a triangle which encompasses all the sample points. 
        // The supertriangle coordinates are added to the end of the 
        // vertex list. The supertriangle is the first triangle in 
        // the triangle list. 
        Point p1 = new Point(midx - 20 * deltaMax, midy - deltaMax);
        Point p2 = new Point(midx, midy + 20 * deltaMax);
        Point p3 = new Point(midx + 20 * deltaMax, midy - deltaMax);
        return new Triangle(p1, p2, p3);
    }

    void OnDrawGizmos()
    {

        if (points != null)
        {
            foreach (Point point in points)
            {
                Gizmos.DrawSphere(new Vector3((float)point.X, 0, (float)point.Y), 10);
            }

            List<Triangle> triangles = BowyerWatson(points);
            Gizmos.color = Color.red;
            if (triangles != null)
            {
                for (int i = 0; i < triangles.Count; i++)
                {
                    while (i < triangles.Count)
                    {
                        Gizmos.DrawLine(ToVector3D(triangles[i].Vertices[0]), ToVector3D(triangles[i].Vertices[1]));
                        Gizmos.DrawLine(ToVector3D(triangles[i].Vertices[1]), ToVector3D(triangles[i].Vertices[2]));
                        Gizmos.DrawLine(ToVector3D(triangles[i].Vertices[2]), ToVector3D(triangles[i].Vertices[0]));
                        i++;
                    }
                }
            }
        }
    }

    Vector3 ToVector3D(Point point)
    {
        return new Vector3((float)point.X, 0, (float)point.Y);
    }
    List<Triangle> BowyerWatson(List<Point> points)
    {
        List<Triangle> triangulation = new List<Triangle>();
        Triangle superTriangle = SuperTriangle(points);
        triangulation.Add(superTriangle);
        List<Edge> edges = new List<Edge>();
        foreach (Point p in points)
        {

            foreach (Triangle tr in triangulation.ToList())
            {
                if (tr.IsPointInCircumcircle(p))
                {
                    edges.Add(new Edge(tr.Vertices[0], tr.Vertices[1]));
                    edges.Add(new Edge(tr.Vertices[1], tr.Vertices[2]));
                    edges.Add(new Edge(tr.Vertices[2], tr.Vertices[0]));
                    triangulation.Remove(tr);
                }
            }
            edges = uniqueEdges(edges);
            foreach (Edge e in edges)
            {
                triangulation.Add(new Triangle(e.p1, e.p2, p));
            }
        }
        //delete verticies which are connected to supertriangle 
        foreach (Triangle t in triangulation.ToList())
        {
            if (t.Vertices.Contains(superTriangle.Vertices[0]) || t.Vertices.Contains(superTriangle.Vertices[1]) || t.Vertices.Contains(superTriangle.Vertices[2]))
            {
                triangulation.Remove(t);
            }
        }

        return triangulation;

    }


    List<Edge> uniqueEdges(List<Edge> edges)
    {
        List<Edge> uniqueEdges = new List<Edge>();
        foreach (Edge e in edges)
        {
            if (!uniqueEdges.Any(ed => ed.Equals(e)))
            {
                uniqueEdges.Add(e);
            }
        }
        return uniqueEdges;
    }


}