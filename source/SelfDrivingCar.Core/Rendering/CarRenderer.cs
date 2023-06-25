using SelfDrivingCar.Core.Controller;
using SelfDrivingCar.Core.Parameters;
using System.Drawing;

namespace SelfDrivingCar.Core.Rendering;

/// <summary>
/// Renders the appearance of a car.
/// </summary>
[PublicAPI]
public class CarRenderer : IRenderer
{
    private readonly Color _color;
    private CarParameterSet? _carParameters;

    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <inheritdoc />
    public int Layer { get; }

    /// <summary>
    /// Creates a new instance of the renderer.
    /// </summary>
    /// <param name="layer">The layer this render should draw on.</param>
    /// <param name="color">The color of the car.</param>
    public CarRenderer(int layer, Color color)
    {
        Layer = layer;
        _color = color;
    }

    /// <inheritdoc />
    public void Render(RenderContext context, Vector4 viewport, float zoomFactor)
    {
        if (Entity == null) return;
        _carParameters ??= Entity.GetParameterSet<CarParameterSet>();
        if (_carParameters == null) return;

        bool isDead = Entity.GetController<CarPhysicsController>()?.IsDead == true;

        context.DrawRectangle(Entity.Position, _carParameters.Width, _carParameters.Length, Entity.Angle, null, 0f, null, isDead ? Color.Gray : _color);
    }
}
