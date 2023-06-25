using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Colliders;

/// <summary>
/// Base class for collider components of an <see cref="Entity"/>.
/// </summary>
[PublicAPI]
public abstract class BaseCollider : IComponent
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <summary>
    /// Gets an enumeration for all line geometries of the collider.
    /// </summary>
    /// <returns>Returns an enumeration of lines. if <c>infinite</c> is true, then the line has an infinite length beyond the start and end point.</returns>
    public abstract IEnumerable<(Vector2 start, Vector2 end, bool infinite)> GetLineGeometry();

    /// <summary>
    /// Gets an enumeration for all polygon geometries of the collider.
    /// </summary>
    /// <returns>Returns an enumeration of polygons.</returns>
    public abstract IEnumerable<IList<Vector2>> GetPolygonGeometry();

    /// <summary>
    /// Gets an enumeration of all intersections of the given line with all lines and polygons of the collider.
    /// </summary>
    /// <param name="start">Start point of the line.</param>
    /// <param name="end">End point of the line.</param>
    /// <returns>Returns the intersection points and the position along the line from 0 (start) to 1 (end).</returns>
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

        foreach (var geometry in GetPolygonGeometry())
        {
            foreach (var intersection in CarMath.GetLinePolygonIntersections(start, end, geometry))
            {
                yield return intersection;
            }
        }
    }

    /// <summary>
    /// Checks if the given polygon collides with any of the line or polygon geometries of the collider.
    /// </summary>
    /// <param name="polygon">The polygon to check for.</param>
    /// <returns>Returns <c>true</c> if the given polygon collides with the collider; otherwise <c>false</c>.</returns>
    public virtual bool CheckPolygonCollision(IList<Vector2> polygon)
    {
        foreach (var (start, end, infinite) in GetLineGeometry())
        {
            if (CarMath.CheckPolygonIntersection(polygon, start, end, infinite)) return true;
        }

        foreach (var geometry in GetPolygonGeometry())
        {
            if (CarMath.CheckPolygonIntersection(polygon, geometry)) return true;
        }

        return false;
    }
}
