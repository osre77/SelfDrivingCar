namespace SelfDrivingCar.Core.Sensors;

/// <summary>
/// Interface for <see cref="Entity"/> sensors.
/// </summary>
/// <remarks>
/// A sensor is an component that has a simulation callback and does some kind of measurement.
/// </remarks>
[PublicAPI]
public interface ISensor : IComponent
{
    /// <summary>
    /// Gets the value of the sensor.
    /// </summary>
    float Value { get; set; }

    /// <summary>
    /// Gets the normalized sensor value.
    /// </summary>
    /// <remarks>
    /// The normalized value mist be between -1 and 1.
    /// </remarks>
    float NormalizedValue { get; set; }

    /// <summary>
    /// Simulate the sensor for 1 frame.
    /// </summary>
    /// <param name="simulationTime">Current time of the simulation in seconds.</param>
    /// <param name="timeDelta">Time delta between this and the last frame in seconds.</param>
    void Simulate(double simulationTime, double timeDelta);
}
