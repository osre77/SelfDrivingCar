using SelfDrivingCar.Core.Controller;
using System.Numerics;

namespace SelfDrivingCar.Core.Colliders;

public class RoadCollider : BaseCollider
{
    public override IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry()
    {
        if (Entity == null) yield break;
        var roadController = Entity.GetController<RoadController>();
        if (roadController == null) yield break;

        yield return (new Vector2(roadController.LeftBorder, 0f), new Vector2(roadController.LeftBorder, 1f), true);
        yield return (new Vector2(roadController.RightBorder, 0f), new Vector2(roadController.RightBorder, 1f), true);
    }

    public override IEnumerable<IList<Vector2>> GetPolyGeometry()
    {
        yield break;
    }
}
