using SelfDrivingCar.Core.Rendering;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using DrawingColor = System.Drawing.Color;

namespace SelfDrivingCar.Wpf.Rendering;

public class WpfRenderContext : RenderContext
{
    public WpfRenderContext(double pixelsPerDip)
    {
        PixelsPerDip = pixelsPerDip;
    }

    public double PixelsPerDip { get; }

    public override void DrawPolygon(IEnumerable<Vector2> path, DrawingColor? strokeColor, float strokeThickness, float[]? dashStyle, DrawingColor? fillColor)
    {
        if (DrawingContext == null) return;

        DrawingContext.DrawGeometry(GetBrush(fillColor), GetPen(strokeColor, strokeThickness, dashStyle),
            new PathGeometry(new[]
            {
                // ReSharper disable PossibleMultipleEnumeration
                new PathFigure(GetPoint(path.First()), path.Skip(1).Select(p => new LineSegment(GetPoint(p), true)), true)
                // ReSharper restore PossibleMultipleEnumeration
            }));
    }

    public override void DrawPolyLine(IEnumerable<Vector2> path, DrawingColor? strokeColor, float strokeThickness, float[]? dashStyle, bool close)
    {
        if (DrawingContext == null) return;

        DrawingContext.DrawGeometry(null, GetPen(strokeColor, strokeThickness, dashStyle),
            new PathGeometry(new[]
            {
                // ReSharper disable PossibleMultipleEnumeration
                new PathFigure(GetPoint(path.First()), path.Skip(1).Select(p => new LineSegment(GetPoint(p), true)), close)
                // ReSharper restore PossibleMultipleEnumeration
            }));
    }

    public override void DrawEllipse(Vector2 center, float width, float height, DrawingColor? strokeColor, float strokeThickness,
        float[]? dashStyle, DrawingColor? fillColor)
    {
        if (DrawingContext == null) return;

        DrawingContext.DrawEllipse(GetBrush(fillColor), GetPen(strokeColor, strokeThickness, dashStyle),
            GetPoint(center), width / 2d, height / 2d);
    }

    public override void DrawText(Vector2 position, string text, DrawingColor color, float size, string fontName, bool bold = false, bool italic = false)
    {
        if (DrawingContext == null) return;

        try
        {
            DrawingContext.PushTransform(new ScaleTransform(1d, -1d, position.X, position.Y));
            DrawingContext.DrawText(new FormattedText(text,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily(fontName),
                    italic ? FontStyles.Italic : FontStyles.Normal,
                    bold ? FontWeights.Bold : FontWeights.Normal,
                    FontStretches.Normal), size, GetBrush(color),
                PixelsPerDip), GetPoint(position));
        }
        finally
        {
            DrawingContext.Pop();
        }
    }

    public DrawingContext? DrawingContext { get; set; }

    public static Pen? GetPen(DrawingColor? color, float thickness, float[]? dashStyle)
    {
        if (!color.HasValue || color.Value.A == 0 || thickness <= 0f) return null;

        var pen = new Pen(GetBrush(color), thickness)
        {
            DashCap = PenLineCap.Flat,
            EndLineCap = PenLineCap.Round,
            LineJoin = PenLineJoin.Round
        };
        if (dashStyle != null && dashStyle.Any())
        {
            pen.DashStyle = new DashStyle(dashStyle.Select(d => (double)d), 0d);
        }
        return pen;
    }

    public static Brush? GetBrush(DrawingColor? color)
    {
        if (!color.HasValue || color.Value.A == 0) return null;

        return new SolidColorBrush(GetColor(color.Value));
    }

    public static Color GetColor(DrawingColor color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static Point GetPoint(Vector2 vector)
    {
        return new Point(vector.X, vector.Y);
    }
}