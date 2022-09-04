
public class Edge
{
    public Point p1;
    public Point p2;
    public Edge(Point p1, Point p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public bool Equals(Edge other)
    {
        return (p1.Equals(other.p1) && p2.Equals(other.p2)) || (p1.Equals(other.p2) && p2.Equals(other.p1));
    }
}
