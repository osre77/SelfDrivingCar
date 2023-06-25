using System.Numerics;
using System.Runtime.CompilerServices;

namespace SelfDrivingCar.Core.Utils;

public static class CarMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    public static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetCirclePoint(float radius, float angle)
    {
        return new Vector2((float)Math.Sin(angle) * radius, (float)Math.Cos(angle) * radius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Bound(double value, float minimum, float maximum)
    {
        return (float)Math.Max(minimum, Math.Min(value, maximum));
    }

    public static Vector2 Rotate(Vector2 vector, float angle)
    {
        return new Vector2(
            (float)(Math.Cos(-angle) * vector.X - Math.Sin(-angle) * vector.Y),
            (float)(Math.Sin(-angle) * vector.X + Math.Cos(-angle) * vector.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DegToRad(float deg)
    {
        return deg / 180f * (float)Math.PI;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float RadToDeg(float rad)
    {
        return rad / (float)Math.PI * 180f;
    }

    public static Vector2? IntersectLines(Vector2 startA, Vector2 endA, Vector2 startB, Vector2 endB)
    {
        return IntersectLines(startA, endA, false, startB, endB, false, out _, out _);
    }

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
