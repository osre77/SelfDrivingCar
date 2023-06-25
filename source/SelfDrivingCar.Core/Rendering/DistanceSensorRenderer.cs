using SelfDrivingCar.Core.Sensors;
using System.Drawing;

namespace SelfDrivingCar.Core.Rendering;

/// <summary>
/// Renders the debug visualization of the <see cref="DistanceSensor"/> component.
/// </summary>
[PublicAPI]
public class DistanceSensorRenderer : IRenderer
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <inheritdoc />
    public int Layer { get; }

    /// <summary>
    /// Creates a new instance of the renderer.
    /// </summary>
    /// <param name="layer">The layer this render should draw on.</param>
    public DistanceSensorRenderer(int layer)
    {
        Layer = layer;
    }

    /// <inheritdoc />
    public void Render(RenderContext context, Vector4 viewport, float zoomFactor)
    {
        if (Entity == null) return;

        foreach (var sensor in Entity.GetSensors<DistanceSensor>())
        {
            var startPoint = sensor.GetStartPoint();
            var vector = sensor.GetVector();
            var valueVector = vector * (1f - sensor.NormalizedValue);
            var midPoint = startPoint + valueVector;
            var endPoint = startPoint + vector;

            if (sensor.NormalizedValue < 1f)
            {
                context.DrawLine(startPoint, midPoint, Color.Yellow, 3f / zoomFactor, null);
            }

            if (sensor.NormalizedValue > 0f)
            {
                context.DrawLine(midPoint, endPoint, Color.Black, 3f / zoomFactor, null);
            }
        }
    }
}
