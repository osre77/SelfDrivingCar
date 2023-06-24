using SelfDrivingCar.Core.Utils;
using System.Numerics;

namespace SelfDrivingCar.Core.Colliders;

public class CarCollider : BaseCollider
{
    public float Width { get; set; } = 1.8f;

    public float Length { get; set; } = 4f;

    public override IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry()
    {
        yield break;
    }

    public override IEnumerable<IList<Vector2>> GetPolyGeometry()
    {
        if (Entity == null) yield break;

        float w2 = Width / 2;
        float l2 = Length / 2;
        float r = (float)Math.Sqrt(w2 * w2 + l2 * l2);
        float a = (float)Math.Atan2(w2, l2);

        yield return new[]
        {
            Entity.Position + CarMath.GetCirclePoint(r, Entity.Angle + a),
            Entity.Position + CarMath.GetCirclePoint(r, Entity.Angle + (float)Math.PI - a),
            Entity.Position + CarMath.GetCirclePoint(r, Entity.Angle + (float)Math.PI + a),
            Entity.Position + CarMath.GetCirclePoint(r, Entity.Angle - a)
        };
    }
}
