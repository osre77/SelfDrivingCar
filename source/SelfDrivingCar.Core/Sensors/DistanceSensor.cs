using SelfDrivingCar.Core.Utils;
using System.Numerics;

namespace SelfDrivingCar.Core.Sensors;

public class DistanceSensor : BaseSensor
{
    public Vector2 Position { get; set; }

    public float Angle { get; set; }

    public float Range { get; set; }

    public DistanceSensor(Vector2 position, float angle, float range = 10f)
    {
        Position = position;
        Angle = angle;
        Range = range;

        Value = Range;
        NormalizedValue = 0f;
    }

    public override void Simulate(double simulationTime, double timeDelta)
    {
        if (Entity?.Graph == null) return;

        var start = GetStartPoint();
        var end = GetEndPoint();

        var positions = Entity.Graph.Entities
            .Where(e => !ReferenceEquals(e, Entity))
            .SelectMany(e => e.Colliders)
            .SelectMany(c => c.GetCollisionPoints(start, end))
            .Select(i => i.position);

        // ReSharper disable PossibleMultipleEnumeration
        if (positions.Any())
        {
            var closesPosition = positions.Min(p => p);

            NormalizedValue = 1 - closesPosition;
            Value = Range * closesPosition;
        }
        // ReSharper restore PossibleMultipleEnumeration
        else
        {
            NormalizedValue = 0f;
            Value = Range;
        }
    }

    public Vector2 GetStartPoint()
    {
        if (Entity == null) return new Vector2();
        return Entity.Position + CarMath.Rotate(Position, Entity.Angle);
    }

    public Vector2 GetVector()
    {
        if (Entity == null) return new Vector2();
        return CarMath.Rotate(new Vector2(0f, Range), Entity.Angle + Angle);
    }

    public Vector2 GetEndPoint()
    {
        if (Entity == null) return new Vector2();
        return Entity.Position + CarMath.Rotate(Position, Entity.Angle) + GetVector();
    }
}
