using System.Runtime.CompilerServices;

namespace SelfDrivingCar.Core.Utils;

/// <summary>
/// Math utility functions.
/// </summary>
[PublicAPI]
public static class CarMath
{
    /// <summary>
    /// LERP a value.
    /// </summary>
    /// <param name="a">Start value.</param>
    /// <param name="b">End value.</param>
    /// <param name="t">Amount to LERP between 0 and 1.</param>
    /// <returns>Returns the new value between a and b.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// LERP a value.
    /// </summary>
    /// <param name="a">Start value.</param>
    /// <param name="b">End value.</param>
    /// <param name="t">Amount to LERP between 0 and 1.</param>
    /// <returns>Returns the new value between a and b.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// Get a point on a circle.
    /// </summary>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="angle">Angle of the point.</param>
    /// <returns>Returns the point on the circle.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetCirclePoint(float radius, float angle)
    {
        return new Vector2((float)Math.Sin(angle) * radius, (float)Math.Cos(angle) * radius);
    }

    /// <summary>
    /// Bound a value between a min. and a maximum.
    /// </summary>
    /// <param name="value">Value to bound.</param>
    /// <param name="minimum">Minimum value.</param>
    /// <param name="maximum">Maximum value.</param>
    /// <returns>The bound value between minimum and maximum.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Bound(double value, float minimum, float maximum)
    {
        return (float)Math.Max(minimum, Math.Min(value, maximum));
    }

    /// <summary>
    /// Rotate a vector.
    /// </summary>
    /// <param name="vector">Vector to rotate.</param>
    /// <param name="angle">Angle in rad.</param>
    /// <returns></returns>
    public static Vector2 Rotate(Vector2 vector, float angle)
    {
        return new Vector2(
            (float)(Math.Cos(-angle) * vector.X - Math.Sin(-angle) * vector.Y),
            (float)(Math.Sin(-angle) * vector.X + Math.Cos(-angle) * vector.Y));
    }

    /// <summary>
    /// Convert an angle from degree to radians.
    /// </summary>
    /// <param name="deg">Angle in degree.</param>
    /// <returns>Returns the angle in radians.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DegToRad(float deg)
    {
        return deg / 180f * (float)Math.PI;
    }

    /// <summary>
    /// Convert an angle from radians to degree.
    /// </summary>
    /// <param name="rad">Angle in radians.</param>
    /// <returns>Returns the angle in degree.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float RadToDeg(float rad)
    {
        return rad / (float)Math.PI * 180f;
    }

    /// <summary>
    /// Intersect 2 finite lines.
    /// </summary>
    /// <param name="startA">Start of line a.</param>
    /// <param name="endA">End of line a.</param>
    /// <param name="startB">Start of line b.</param>
    /// <param name="endB">End of line b.</param>
    /// <returns>Returns the intersection point of <c>null</c> if the lines do not intersect.</returns>
    public static Vector2? IntersectLines(Vector2 startA, Vector2 endA, Vector2 startB, Vector2 endB)
    {
        return IntersectLines(startA, endA, false, startB, endB, false, out _, out _);
    }

    /// <summary>
    /// Intersect lines.
    /// </summary>
    /// <param name="startA">Start of line a.</param>
    /// <param name="endA">End of line a.</param>
    /// <param name="infiniteA"><c>true</c> if line a has an infinite length beyond the start and end point.</param>
    /// <param name="startB">Start of line b.</param>
    /// <param name="endB">End of line b.</param>
    /// <param name="infiniteB"><c>true</c> if line b has an infinite length beyond the start and end point.</param>
    /// <param name="positionA">Is set to the intersection position along line a between 0 (start) and 1 (end).</param>
    /// <param name="positionB">Is set to the intersection position along line b between 0 (start) and 1 (end).</param>
    /// <returns>Returns the intersection point of <c>null</c> if the lines do not intersect.</returns>
    /// <remarks>
    /// This is a C# port of the line intersection function created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
    /// </remarks>
    public static Vector2? IntersectLines(Vector2 startA, Vector2 endA, bool infiniteA, Vector2 startB, Vector2 endB, bool infiniteB, out float positionA, out float positionB)
    {
        // Original js code from www.radufromfinland.com self-driving car
        //function getIntersection(A, B, C, D)
        //{
        //    const tTop= (D.x - C.x) * (A.y - C.y) - (D.y - C.y) * (A.x - C.x);
        //    const uTop= (C.y - A.y) * (A.x - B.x) - (C.x - A.x) * (A.y - B.y);
        //    const bottom= (D.y - C.y) * (B.x - A.x) - (D.x - C.x) * (B.y - A.y);
        //    if (bottom != 0)
        //    {
        //        const t= tTop / bottom;
        //        const u= uTop / bottom;
        //        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        //        {
        //            return {
        //                x: lerp(A.x, B.x, t),
        //                y: lerp(A.y, B.y, t),
        //                offset: t
        //            }
        //        }
        //    }
        //    return null;
        //}

        var tTop = (endB.X - startB.X) * (startA.Y - startB.Y) - (endB.Y - startB.Y) * (startA.X - startB.X);
        var uTop = (startB.Y - startA.Y) * (startA.X - endA.X) - (startB.X - startA.X) * (startA.Y - endA.Y);
        var bottom = (endB.Y - startB.Y) * (endA.X - startA.X) - (endB.X - startB.X) * (endA.Y - startA.Y);

        if (bottom != 0)
        {
            positionA = tTop / bottom;
            positionB = uTop / bottom;
            if ((infiniteA || (positionA is >= 0 and <= 1)) && (infiniteB || (positionB is >= 0 and <= 1)))
            {
                return new Vector2(
                    Lerp(startA.X, endA.X, positionA),
                    Lerp(startA.Y, endA.Y, positionA));
            }
        }

        positionA = 0f;
        positionB = 0f;
        return null;
    }

    /// <summary>
    /// Checks if two polygons intersects.
    /// </summary>
    /// <param name="polygonA">Polygon a.</param>
    /// <param name="polygonB">Polygon b.</param>
    /// <returns>Returns <c>true</c> if the two polygons intersect.</returns>
    /// <remarks>
    /// This is a C# port of the line intersection function created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
    /// </remarks>
    public static bool CheckPolygonIntersection(IList<Vector2> polygonA, IList<Vector2> polygonB)
    {
        // Original js code from www.radufromfinland.com self-driving car
        //function polysIntersect(poly1, poly2)
        //{
        //    for (let i = 0; i < poly1.length; i++)
        //    {
        //        for (let j = 0; j < poly2.length; j++)
        //        {
        //            const touch= getIntersection(
        //                poly1[i],
        //                poly1[(i + 1) % poly1.length],
        //                poly2[j],
        //                poly2[(j + 1) % poly2.length]
        //            );
        //            if (touch)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        int edgeCountA = polygonA.Count;
        int edgeCountB = polygonB.Count;

        for (int a = 0; a < edgeCountA; ++a)
        {
            for (int b = 0; b < edgeCountB; ++b)
            {
                if (IntersectLines(
                        polygonA[a], polygonA[(a + 1) % edgeCountA],
                        polygonB[b], polygonB[(b + 1) % edgeCountB]) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Intersects a finite line with a polygon.
    /// </summary>
    /// <param name="start">Start of the line.</param>
    /// <param name="end">End of the line.</param>
    /// <param name="polygon">The polygon.</param>
    /// <returns>Returns an enumeration with all intersection points. The position is a value from 0 (start) to 1 (end) along the line.</returns>
    public static IEnumerable<(Vector2 point, float position)> GetLinePolygonIntersections(Vector2 start, Vector2 end,
        IList<Vector2> polygon)
    {
        int edgeCount = polygon.Count;

        for (int a = 0; a < edgeCount; ++a)
        {
            var point = IntersectLines(
                start, end, false,
                polygon[a], polygon[(a + 1) % edgeCount], false, out var position, out _);

            if (point.HasValue)
            {
                yield return (point.Value, position);
            }
        }
    }

    /// <summary>
    /// Check if a polygon and a line intersect.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    /// <param name="lineStart">Start point of the line.</param>
    /// <param name="lineEnd">End point of the line.</param>
    /// <param name="lineInfinite"><c>true</c> if the line has an infinite length beyond the start and end point.</param>
    /// <returns></returns>
    public static bool CheckPolygonIntersection(IList<Vector2> polygon, Vector2 lineStart, Vector2 lineEnd, bool lineInfinite)
    {
        int edgeCount = polygon.Count;

        for (int a = 0; a < edgeCount; ++a)
        {
            if (IntersectLines(
                    polygon[a], polygon[(a + 1) % edgeCount], false,
                    lineStart, lineEnd, lineInfinite, out _, out _) != null)
            {
                return true;
            }
        }
        return false;
    }
}
