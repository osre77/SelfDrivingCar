using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SelfDrivingCar.Core.Rendering;

namespace SelfDrivingCar.Wpf.Rendering;

public abstract class WpfRenderControl : Control
{
    private double _zoomFactor = 1d;
    private Point _origin = new(0, 1);
    private Point _offset = new(0d, 0d);

    public WpfRenderContext RenderContext { get; }

    protected WpfRenderControl()
    {
        RenderContext = new WpfRenderContext(VisualTreeHelper.GetDpi(this).PixelsPerDip);
    }

    public Point Origin
    {
        get => _origin;
        set
        {
            _origin = value;
            Refresh();
        }
    }

    public Point Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            Refresh();
        }
    }

    public double ZoomFactor
    {
        get => _zoomFactor;
        set
        {
            _zoomFactor = value;
            Refresh();
        }
    }

    public void Refresh()
    {
        if (CheckAccess())
        {
            InvalidateVisual();
        }
        else
        {
            Dispatcher.BeginInvoke(InvalidateVisual);
        }
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        drawingContext.PushClip(new RectangleGeometry(new Rect(0d, 0d, ActualWidth, ActualHeight)));

        drawingContext.DrawRectangle(Background, null, new Rect(new Point(0d, 0d), new Size(ActualWidth, ActualHeight)));

        try
        {
            var origin = new Point(ActualWidth * Origin.X, ActualHeight * Origin.Y);

            drawingContext.PushTransform(new TransformGroup
            {
                Children = new TransformCollection(new Transform[]
                {
                    new TranslateTransform(origin.X, origin.Y),
                    new TranslateTransform(-Offset.X, -Offset.Y),
                    new ScaleTransform(ZoomFactor, -ZoomFactor, origin.X, origin.Y)
                })
            });
            RenderContext.DrawingContext = drawingContext;

            OnRender(RenderContext, new Vector4(
                (float)(-origin.X / ZoomFactor + Offset.X),
                (float)(-(ActualHeight - origin.Y) / ZoomFactor + Offset.Y),
                (float)((ActualWidth - origin.X) / ZoomFactor + Offset.X),
                (float)(origin.Y / ZoomFactor + Offset.Y)), (float)ZoomFactor);
        }
        finally
        {
            drawingContext.Pop();
            RenderContext.DrawingContext = null;
            drawingContext.Pop();
        }
    }

    protected abstract void OnRender(RenderContext renderContext, Vector4 viewport, float zoomFactor);
}
