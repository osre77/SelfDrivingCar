namespace SelfDrivingCar.Core.Parameters;

/// <summary>
/// Parameter set for a car <see cref="Entity"/>.
/// </summary>
[PublicAPI]
public class CarParameterSet : IParameterSet
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <summary>
    /// Gets or sets the width of the car in m.
    /// </summary>
    public float Width { get; set; } = 1.8f;

    /// <summary>
    /// Gets or sets the length of the car in m.
    /// </summary>
    public float Length { get; set; } = 4f;
}
