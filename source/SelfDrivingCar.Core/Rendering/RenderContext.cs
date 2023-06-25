using SelfDrivingCar.Core.Utils;
using System.Drawing;

namespace SelfDrivingCar.Core.Rendering;

/// <summary>
/// A framework independent render context.
/// </summary>
/// <remarks>
/// The coordinate system has X from left to right and Y from bottom to top.
/// Angles are in rad. Zero is straight up (+Y). Positive values will rotate clockwise.
/// </remarks>
[PublicAPI]
public abstract class RenderContext
{
    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="a">Point a of the line.</param>
    /// <param name="b">Point b of the line.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    public virtual void DrawLine(Vector2 a, Vector2 b, Color? strokeColor, float strokeThickness, float[]? dashStyle)
    {
        DrawPolyLine(new[] { a, b }, strokeColor, strokeThickness, dashStyle, false);
    }

    /// <summary>
    /// Draws a rotated rectangle.
    /// </summary>
    /// <param name="center">Center point of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="angle">Angle of the rectangle in rad.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    /// <param name="fillColor">Fill color of the rectangle.</param>
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

    /// <summary>
    /// Draws an axis aligned rectangle.
    /// </summary>
    /// <param name="center">Center point of the rectangle.</param>
    /// <param name="width">Width of the rectangle.</param>
    /// <param name="height">Height of the rectangle.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    /// <param name="fillColor">Fill color of the rectangle.</param>
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

    /// <summary>
    /// Draws a polygon.
    /// </summary>
    /// <param name="path">Corner points of the polygon.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    /// <param name="fillColor">Fill color of the rectangle.</param>
    public abstract void DrawPolygon(IEnumerable<Vector2> path, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor);

    /// <summary>
    /// Draws a polygon line.
    /// </summary>
    /// <param name="path">Corner points of the polygon.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    /// <param name="close"><c>true</c> to close the polygon; <c>false</c> to draw an open polygon line.</param>
    public abstract void DrawPolyLine(IEnumerable<Vector2> path, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, bool close);

    /// <summary>
    /// Draws an ellipse.
    /// </summary>
    /// <param name="center">Center point of the ellipse.</param>
    /// <param name="width">Width of the ellipse.</param>
    /// <param name="height">Height of the ellipse.</param>
    /// <param name="strokeColor">Color of the line.</param>
    /// <param name="strokeThickness">Thickness of the line.</param>
    /// <param name="dashStyle">Dash style of the line. The actual length of the dash/space is <paramref name="strokeThickness"/> * dash value.</param>
    /// <param name="fillColor">Fill color of the rectangle.</param>
    public abstract void DrawEllipse(Vector2 center, float width, float height, Color? strokeColor,
        float strokeThickness, float[]? dashStyle, Color? fillColor);

    /// <summary>
    /// Draw text.
    /// </summary>
    /// <param name="position">Lower left position of the text.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="color">Color of the text.</param>
    /// <param name="size">Size (height) of the text.</param>
    /// <param name="fontName">Name of the font.</param>
    /// <param name="bold"><c>true</c> to draw bold text.</param>
    /// <param name="italic"><c>true</c> to draw italic text.</param>
    public abstract void DrawText(Vector2 position, string text, Color color, float size, string fontName, bool bold = false, bool italic = false);
}
