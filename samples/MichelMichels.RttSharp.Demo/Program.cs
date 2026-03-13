// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using MichelMichels.RttSharp;
using MichelMichels.RttSharp.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.ColorSpaces;
using System.Reflection;
using System.Reflection.Metadata;

Console.WriteLine("Rapidly Exploring Random Tree Generator Demo");

RapidlyExploringRandomTreeGenerator generator = new();

System.Drawing.PointF startVertex = new(250, 250);
int vertexCount = 5000;
int width = 500;
int height = 500;
double distance = 6.0;
RapidlyExploringRandomTree rtt = generator.Build(startVertex, vertexCount, width, height, distance);

Console.WriteLine($"Generated {rtt.Vertices.Count} vertices and {rtt.Edges.Count} edges.");

using Image<Rgba32> image = new(width, height);
image.Mutate(ctx => ctx.Fill(Color.White));

foreach (LineF edge in rtt.Edges)
{
    image.Mutate(ctx =>
    {
        ctx.DrawLine(
            Color.Red,
            1,
            new PointF(edge.Start.X, edge.Start.Y),
            new PointF(edge.End.X, edge.End.Y)
            );
    });
}

image.Save("output.png");