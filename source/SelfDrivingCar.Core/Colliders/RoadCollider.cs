using SelfDrivingCar.Core.Parameters;

namespace SelfDrivingCar.Core.Colliders;

/// <summary>
/// Collider for a road <see cref="Entity"/>.
/// </summary>
[PublicAPI]
public class RoadCollider : BaseCollider
{
    /// <inheritdoc />
    public override IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry()
    {
        if (Entity == null) yield break;
        var roadController = Entity.GetParameterSet<RoadParameterSet>();
        if (roadController == null) yield break;

        yield return (new Vector2(roadController.LeftBorder, 0f), new Vector2(roadController.LeftBorder, 1f), true);
        yield return (new Vector2(roadController.RightBorder, 0f), new Vector2(roadController.RightBorder, 1f), true);
    }

    /// <inheritdoc />
    public override IEnumerable<IList<Vector2>> GetPolygonGeometry()
    {
        yield break;
    }
}
