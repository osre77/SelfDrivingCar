using SelfDrivingCar.Core.Utils;
using System.Drawing;
using System.Numerics;

namespace SelfDrivingCar.Core.Rendering;

public abstract class RenderContext
{
    public virtual void DrawLine(Vector2 a, Vector2 b, Color? strokeColor, float strokeThickness, float[]? dashStyle)
    {
        DrawPolyLine(new[] { a, b }, strokeColor, strokeThickness, dashStyle, false);
    }

    public virtual void DrawRectangle(Vector2 center, float width, float height, float angle, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor)
    {
        float w2 = width / 2;
        float h2 = height / 2;
        float r = (float)Math.Sqrt(w2 * w2 + h2 * h2);
        float a = (float)Math.Atan2(w2, h2);
        DrawPolygon(new[]
        {
            center + CarMath.GetCirclePoint(r, angle + a),
            center + CarMath.GetCirclePoint(r, angle + (float)Math.PI - a),
            center + CarMath.GetCirclePoint(r, angle + (float)Math.PI + a),
            center + CarMath.GetCirclePoint(r, angle - a)
        }, strokeColor, strokeThickness, dashStyle, fillColor);
    }

    public virtual void DrawRectangle(Vector2 center, float width, float height, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor)
    {
        float w2 = width / 2;
        float h2 = height / 2;
        DrawPolygon(new[]
        {
            center + new Vector2(w2, h2),
            center + new Vector2(w2, -h2),
            center + new Vector2(-w2, -h2),
            center + new Vector2(-w2, h2)
        }, strokeColor, strokeThickness, dashStyle, fillColor);
    }

    public abstract void DrawPolygon(IEnumerable<Vector2> path, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor);

    public abstract void DrawPolyLine(IEnumerable<Vector2> path, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, bool close);

    public abstract void DrawEllipse(Vector2 center, float width, float height, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor);

    public abstract void DrawText(Vector2 position, string text, Color color, float size, string fontName, bool bold = false, bool italic = false);

    public abstract void PushRotateTransform(float angle, Vector2 center);

    public abstract void PushTranslateTransform(Vector2 offset);

    public abstract void Pop();
}
