using SelfDrivingCar.Core;
using SelfDrivingCar.Core.Rendering;
using System.Numerics;
using System.Windows.Input;

namespace SelfDrivingCar.Wpf.Rendering;

public class WpfEntityGraphRenderControl : WpfRenderControl
{
    private bool _accelerate;
    private bool _decelerate;
    private bool _steerLeft;
    private bool _steerRight;
    private EntityGraph? _entityGraph;
    private int[] _activeLayers = Array.Empty<int>();

    public WpfEntityGraphRenderControl()
    {
        Focusable = true;
        Origin = new(0.5, 0.7);
        ZoomFactor = 20f;
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

    protected override void OnRender(RenderContext renderContext, Vector4 viewport, float zoomFactor)
    {
        if (EntityGraph == null) return;

        EntityGraph.Render(renderContext, ActiveLayers, viewport, zoomFactor);
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