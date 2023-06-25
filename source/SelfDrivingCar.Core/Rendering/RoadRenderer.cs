using SelfDrivingCar.Core.Controller;
using SelfDrivingCar.Core.Utils;
using System.Drawing;
using System.Numerics;

namespace SelfDrivingCar.Core.Rendering;

public class RoadRenderer : BaseRenderer
{
    public RoadRenderer(int layer) : base(layer)
    {
    }

    public override void Render(RenderContext context, Vector4 viewport, float zoomFactor)
    {
        var controller = Entity?.GetController<RoadController>();
        if (controller == null) return;

        float bottom = (float)Math.Round((viewport.Y - 2) / 2) * 2;
        float top = (float)Math.Round((viewport.W + 2) / 2) * 2;

        context.DrawRectangle(new Vector2(0f, CarMath.Lerp(bottom, top, 0.5f)), controller.LaneWidth * controller.LaneCount * 1.1f, top - bottom, null, 0f, null, Color.DimGray);
        context.DrawLine(
            new Vector2(controller.LeftBorder, top),
            new Vector2(controller.LeftBorder, bottom),
            Color.White, 0.2f, Array.Empty<float>());
        context.DrawLine(
            new Vector2(controller.RightBorder, top),
            new Vector2(controller.RightBorder, bottom),
            Color.White, 0.2f, Array.Empty<float>());
        for (int n = 1; n < controller.LaneCount; ++n)
        {
            float x = controller.LeftBorder + n * controller.LaneWidth;
            context.DrawLine(
                new Vector2(x, top),
                new Vector2(x, bottom),
                Color.White, 0.2f, new[] { 5f, 5f });
        }

        float markerPosition = (float)Math.Round(bottom / 10) * 10;
        float markerOffset = controller.LaneWidth * controller.LaneCount * 0.6f;
        while (true)
        {
            RenderMarker(context, markerOffset, markerPosition, $"{markerPosition:f0}m");
            markerPosition += 10f;

            if (markerPosition > top) break;
        }
    }

    private void RenderMarker(RenderContext context, float x, float y, string markerText)
    {
        context.DrawPolygon(new[]
        {
            new Vector2(x, y),
            new Vector2(x + 0.5f, y + 0.5f),
            new Vector2(x + 3f, y + 0.5f),
            new Vector2(x + 3f, y - 0.5f),
            new Vector2(x + 0.5f, y - 0.5f)
        }, null, 0f, null, Color.White);

        context.DrawText(new Vector2(x + 0.5f, y + 0.4f), markerText, Color.Black, 0.8f, "Arial", true);
    }
}
