using System.Numerics;

namespace SelfDrivingCar.Core.Rendering;

public abstract class BaseRenderer : Component
{
    public int Layer { get; }

    protected BaseRenderer(int layer)
    {
        Layer = layer;
    }

    public abstract void Render(RenderContext context, Vector4 viewport, float zoomFactor);
}
