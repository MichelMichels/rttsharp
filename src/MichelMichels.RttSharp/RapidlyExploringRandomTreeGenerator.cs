using System.Drawing;
using MichelMichels.RttSharp.Models;

namespace MichelMichels.RttSharp;

public class RapidlyExploringRandomTreeGenerator
{
    private Random random = new();

    public RapidlyExploringRandomTree Build(PointF initialVertex, int vertexCount, int width, int height, double distance)
    {
        RapidlyExploringRandomTree rtt = new();
        rtt.Vertices.Add(initialVertex);

        for (int i = 0; i < vertexCount; i++)
        {
            GenerateNextVertex(rtt, width, height, distance);
        }

        return rtt;
    }

    public void GenerateNextVertex(RapidlyExploringRandomTree rtt, int width, int height, double distance)
    {
        PointF randomVertex = GenerateRandomVertex(width, height);
        PointF nearestVertex = CalculateNearestVertex(randomVertex, rtt.Vertices);
        PointF newVertex = StepFromPointToPoint(nearestVertex, randomVertex, distance);

        rtt.Vertices.Add(newVertex);
        rtt.Edges.Add(new LineF(nearestVertex, newVertex));
    }

    private PointF GenerateRandomVertex(int width, int height)
    {
        float x = (float)(random.NextDouble() * width);
        float y = (float)(random.NextDouble() * height);
        return new PointF(x, y);
    }
    private PointF CalculateNearestVertex(PointF vertex, List<PointF> vertices)
    {
        if (vertices.Count == 0)
        {
            return vertex;
        }

        PointF nearest = vertices[0];

        foreach (PointF v in vertices)
        {
            if (CalculateDistance(v, vertex) < CalculateDistance(nearest, vertex))
            {
                nearest = v;
            }
        }

        return nearest;
    }

    private float CalculateDistance(PointF a, PointF b)
    {
        return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    private PointF StepFromPointToPoint(PointF a, PointF b, double epsilon)
    {
        if (CalculateDistance(a, b) < epsilon)
        {
            return b;
        }
        else
        {
            double theta = Math.Atan2(b.Y - a.Y, b.X - a.X);
            return new PointF((float)(a.X + epsilon * Math.Cos(theta)), (float)(a.Y + epsilon * Math.Sin(theta)));
        }
    }
}
