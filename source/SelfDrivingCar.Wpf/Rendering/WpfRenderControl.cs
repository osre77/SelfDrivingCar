using SelfDrivingCar.Core;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SelfDrivingCar.Wpf.Rendering;

public class WpfRenderControl : Control
{
    private readonly WpfRenderContext _renderContext;
    private bool _accelerate;
    private bool _decelerate;
    private bool _steerLeft;
    private bool _steerRight;
    private double _zoomFactor = 20d;
    private EntityGraph? _entityGraph;
    private int[] _activeLayers = Array.Empty<int>();
    private Point _origin = new Point(0.5, 0.7);
    private Point _offset = new Point(0d, 0d);

    public WpfRenderControl()
    {
        Focusable = true;
        _renderContext = new WpfRenderContext(VisualTreeHelper.GetDpi(this).PixelsPerDip);
    }

    public EntityGraph? EntityGraph
    {
        get => _entityGraph;
        set
        {
            _entityGraph = value;
            Refresh();
        }
    }

    public int[] ActiveLayers
    {
        get => _activeLayers;
        set
        {
            _activeLayers = value;
            Refresh();
        }
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

        if (EntityGraph == null) return;

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
            _renderContext.DrawingContext = drawingContext;

            EntityGraph.Render(_renderContext, ActiveLayers, new Vector4(
                (float)(-origin.X / ZoomFactor + Offset.X),
                (float)(origin.Y / ZoomFactor + Offset.Y),
                (float)(origin.X / ZoomFactor + Offset.X),
                (float)(-(ActualHeight - origin.Y) / ZoomFactor + Offset.Y)), (float)ZoomFactor);
        }
        finally
        {
            drawingContext.Pop();
            _renderContext.DrawingContext = null;
            drawingContext.Pop();
        }
    }

    public void GetInput(out bool accelerate, out bool decelerate, out bool steerLeft, out bool steerRight)
    {
        accelerate = _accelerate;
        decelerate = _decelerate;
        steerLeft = _steerLeft;
        steerRight = _steerRight;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                _accelerate = true;
                e.Handled = true;
                break;
            case Key.Down:
                _decelerate = true;
                e.Handled = true;
                break;
            case Key.Left:
                _steerLeft = true;
                e.Handled = true;
                break;
            case Key.Right:
                _steerRight = true;
                e.Handled = true;
                break;
        }
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up: _accelerate = false; e.Handled = true; break;
            case Key.Down: _decelerate = false; e.Handled = true; break;
            case Key.Left: _steerLeft = false; e.Handled = true; break;
            case Key.Right: _steerRight = false; e.Handled = true; break;
        }
        base.OnKeyUp(e);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        Focus();
        base.OnMouseDown(e);
    }

    protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        _accelerate = false;
        _decelerate = false;
        _steerLeft = false;
        _steerRight = false;
        base.OnLostKeyboardFocus(e);
    }
}