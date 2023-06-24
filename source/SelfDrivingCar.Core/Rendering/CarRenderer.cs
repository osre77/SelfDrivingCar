using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Controller;
using System.Drawing;
using System.Numerics;

namespace SelfDrivingCar.Core.Rendering;

public class CarRenderer : BaseRenderer
{
    private readonly Color _color;

    public CarRenderer(int layer, Color color) : base(layer)
    {
        _color = color;
    }

    public override void Render(RenderContext context, Vector4 viewport, float zoomFactor)
    {
        if (Entity == null) return;
        var carCollider = Entity.GetCollider<CarCollider>();
        if (carCollider == null) return;

        bool isDead = Entity.GetController<CarPhysicsController>()?.IsDead == true;

        context.DrawPolygon(carCollider.GetPolyGeometry().First(), null, 0f, null, isDead ? Color.Gray : _color);
    }
}
