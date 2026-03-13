using System.Drawing;

namespace MichelMichels.RttSharp.Models;

public struct LineF(PointF start, PointF end)
{
    public PointF Start { get; set; } = start;
    public PointF End { get; set; } = end;
}