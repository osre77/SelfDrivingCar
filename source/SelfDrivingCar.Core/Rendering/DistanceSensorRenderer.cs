using SelfDrivingCar.Core.Sensors;
using System.Drawing;
using System.Numerics;

namespace SelfDrivingCar.Core.Rendering;

public class DistanceSensorRenderer : BaseRenderer
{
    public DistanceSensorRenderer(int layer) : base(layer)
    { }

    public override void Render(RenderContext context, Vector4 viewport, float zoomFactor)
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
