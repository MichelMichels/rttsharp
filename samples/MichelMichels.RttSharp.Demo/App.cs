using System.Diagnostics;
using MichelMichels.RttSharp;
using MichelMichels.RttSharp.Models;
using Sdl3Sharp;
using Sdl3Sharp.Events;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;

public class App : AppBase
{
    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    private readonly RapidlyExploringRandomTreeGenerator rapidlyExploringRandomTreeGenerator = new();
    private readonly RapidlyExploringRandomTree rapidlyExploringRandomTree = new();

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("Hello World", 800, 600, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        Task.Run(() =>
        {
            for (int i = 0; i < 10000; i++)
            {
                rapidlyExploringRandomTreeGenerator.GenerateNextVertex(rapidlyExploringRandomTree, 800, 600, 6.0);
            }
        });


        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        mRenderer.DrawColorFloat = (0, 0, 0, 1);
        mRenderer.TryClear();
        Point<float>[] array = rapidlyExploringRandomTree.Vertices.Select(v => new Point<float>(v.X, v.Y)).ToArray();
        ReadOnlySpan<Point<float>> points = new(array);

        mRenderer.TryRenderLines(points);
        mRenderer.TryRenderPresent();

        return Continue;
    }

    protected override AppResult OnEvent(Sdl sdl, ref Event @event)
    {
        if (@event.Type is EventType.WindowCloseRequested)
        {
            return Success;
        }

        return Continue;
    }

    protected override void OnQuit(Sdl sdl, AppResult result)
    {
        mRenderer?.Dispose();
        mRenderer = default!;

        mWindow?.Dispose();
        mWindow = default!;
    }
}