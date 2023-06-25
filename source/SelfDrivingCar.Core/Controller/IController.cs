namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// Interface for <see cref="Entity"/> controllers.
/// </summary>
/// <remarks>
/// A controller is an component that has a simulation callback for every frame and typically performs
/// some calculations and potentially updates the entity position and angle.
/// </remarks>
[PublicAPI]
public interface IController : IComponent
{
    /// <summary>
    /// Simulate the controller for 1 frame.
    /// </summary>
    /// <param name="simulationTime">Current time of the simulation in seconds.</param>
    /// <param name="timeDelta">Time delta between this and the last frame in seconds.</param>
    void Simulate(double simulationTime, double timeDelta);
}
