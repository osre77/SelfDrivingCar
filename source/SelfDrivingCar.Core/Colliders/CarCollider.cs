using SelfDrivingCar.Core.Parameters;
using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Colliders;

/// <summary>
/// Collider for a car <see cref="Entity"/>.
/// </summary>
[PublicAPI]
public class CarCollider : BaseCollider
{
    private CarParameterSet? _carParameters;

    /// <inheritdoc />
    public override IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry()
    {
        yield break;
    }

    /// <inheritdoc />
    public override IEnumerable<IList<Vector2>> GetPolygonGeometry()
    {
        if (Entity == null) yield break;
        _carParameters ??= Entity.GetParameterSet<CarParameterSet>();
        if (_carParameters == null) yield break;

        float w2 = _carParameters.Width / 2;
        float l2 = _carParameters.Length / 2;
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
