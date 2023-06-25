using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Sensors;

/// <summary>
/// A distance sensor for a car.
/// </summary>
[PublicAPI]
public class DistanceSensor : ISensor
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <inheritdoc />
    public float Value { get; set; }

    /// <inheritdoc />
    public float NormalizedValue { get; set; }

    /// <summary>
    /// Gets or sets the position of the sensor relative to the <see cref="Entity"/> position in m.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the angle of the sensor relative to the <see cref="Entity"/> angle in rad.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets or sets the range of the sensor in m.
    /// </summary>
    public float Range { get; set; }

    /// <summary>
    /// Creates a new instance of the sensor.
    /// </summary>
    /// <param name="position">The position of the sensor relative to the <see cref="Entity"/> in m.</param>
    /// <param name="angle">The angle of the sensor relative to the <see cref="Entity"/> in rad.</param>
    /// <param name="range">The range of the sensor in m.</param>
    public DistanceSensor(Vector2 position, float angle, float range = 10f)
    {
        Position = position;
        Angle = angle;
        Range = range;

        Value = Range;
        NormalizedValue = 0f;
    }

    /// <inheritdoc />
    public void Simulate(double simulationTime, double timeDelta)
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

    /// <summary>
    /// Gets the absolute start point of the sensor ray.
    /// </summary>
    /// <returns>Returns the absolute start point.</returns>
    public Vector2 GetStartPoint()
    {
        if (Entity == null) return new Vector2();
        return Entity.Position + CarMath.Rotate(Position, Entity.Angle);
    }

    /// <summary>
    /// Gets the absolute end point of the sensor ray.
    /// </summary>
    /// <returns>Returns the absolute end point.</returns>
    public Vector2 GetEndPoint()
    {
        if (Entity == null) return new Vector2();
        return Entity.Position + CarMath.Rotate(Position, Entity.Angle) + GetVector();
    }

    /// <summary>
    /// Gets the sensor vector from the start to the end point.
    /// </summary>
    /// <returns>Returns the sensor vector.</returns>
    public Vector2 GetVector()
    {
        if (Entity == null) return new Vector2();
        return CarMath.Rotate(new Vector2(0f, Range), Entity.Angle + Angle);
    }
}
