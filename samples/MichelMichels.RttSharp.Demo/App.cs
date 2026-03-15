using MichelMichels.RttSharp;
using MichelMichels.RttSharp.Models;
using Sdl3Sharp;
using Sdl3Sharp.Events;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing;

public class App : AppBase
{
    private const int WINDOW_MARGIN = 16;
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

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        mRenderer.DrawColorFloat = (0, 0, 0, 1);
        mRenderer.TryClear();

        Rect<float> renderArea = CalculateTreeRenderArea();

        IncreaseTree(5_000, 10, renderArea);
        RenderTree(renderArea);
        RenderUserInterface();

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

    private void IncreaseTree(int max, int vertexCount, Rect<float> renderArea)
    {
        if (rapidlyExploringRandomTree.Vertices.Count > max)
        {
            return;
        }

        for (int i = 0; i < vertexCount; i++)
        {
            rapidlyExploringRandomTreeGenerator.GenerateNextVertex(rapidlyExploringRandomTree, (int)renderArea.Width, (int)renderArea.Height, 20);
        }
    }
    private void RenderTree(Rect<float> renderArea)
    {
        foreach (LineF line in rapidlyExploringRandomTree.Edges)
        {
            mRenderer.DrawColorFloat = new Sdl3Sharp.Video.Coloring.Color<float>(255, 0, 0, 1);
            mRenderer.TryRenderLine(line.Start.X + renderArea.Left, line.Start.Y + renderArea.Top, line.End.X + renderArea.Left, line.End.Y + renderArea.Top);
        }
    }

    private void RenderUserInterface()
    {
        int height = mRenderer.Window!.Size.Height;
        int width = mRenderer.Window!.Size.Width;

        mRenderer.DrawColor = new Sdl3Sharp.Video.Coloring.Color<byte>(0, 255, 0, 255);
        mRenderer.TryRenderDebugText(WINDOW_MARGIN, WINDOW_MARGIN, "Rapidly Exploring Random Tree demo");
        mRenderer.TryRenderDebugText(WINDOW_MARGIN, height - WINDOW_MARGIN, "Author: Michel Michels");

        mRenderer.TryRenderDebugText(width - 4 * WINDOW_MARGIN, WINDOW_MARGIN, rapidlyExploringRandomTree.Vertices.Count.ToString());

        mRenderer.TryRenderRect(CalculateTreeRenderArea());
    }

    private Rect<float> CalculateTreeRenderArea()
    {
        int textHeight = 16;
        int height = mRenderer.Window!.Size.Height;
        int width = mRenderer.Window!.Size.Width;

        int rectWidth = width - 2 * WINDOW_MARGIN;
        int rectHeight = height - 2 * WINDOW_MARGIN - 2 * textHeight;
        return new(WINDOW_MARGIN, WINDOW_MARGIN + textHeight, rectWidth, rectHeight);
    }
}