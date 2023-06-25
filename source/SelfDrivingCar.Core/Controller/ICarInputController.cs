namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// Interface for controllers that provide driving inputs to a car.
/// </summary>
[PublicAPI]
public interface ICarInputController : IController
{
    /// <summary>
    /// Gets or sets the throttle.
    /// </summary>
    /// <remarks>The throttle value MUST be between -1 and 1.</remarks>
    public float Throttle { get; set; }

    /// <summary>
    /// Gets or sets the steering input.
    /// </summary>
    /// <remarks>The steering input MUST be between -1 and 1.</remarks>
    public float SteeringInput { get; set; }
}
