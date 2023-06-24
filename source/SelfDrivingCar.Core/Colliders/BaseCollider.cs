using SelfDrivingCar.Core.Utils;
using System.Numerics;

namespace SelfDrivingCar.Core.Colliders;

public abstract class BaseCollider : Component
{
    public abstract IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry();

    public abstract IEnumerable<IList<Vector2>> GetPolyGeometry();

    public virtual IEnumerable<(Vector2 point, float position)> GetCollisionPoints(Vector2 start, Vector2 end)
    {
        foreach (var line in GetLineGeometry())
        {
            var point = CarMath.IntersectLines(start, end, false, line.start, line.end, line.infinite, out var position, out _);
            if (point.HasValue)
            {
                yield return (point.Value, position);
            }
        }

        foreach (var geometry in GetPolyGeometry())
        {
            foreach (var intersection in CarMath.GetLinePolygonIntersections(start, end, geometry))
            {
                yield return intersection;
            }
        }
    }

    public virtual bool CheckPolygonCollision(IList<Vector2> polygon)
    {
        foreach (var (start, end, infinite) in GetLineGeometry())
        {
            if (CarMath.CheckPolygonIntersection(polygon, start, end, infinite)) return true;
        }

        foreach (var geometry in GetPolyGeometry())
        {
            if (CarMath.CheckPolygonIntersection(polygon, geometry)) return true;
        }

        return false;
    }
}
