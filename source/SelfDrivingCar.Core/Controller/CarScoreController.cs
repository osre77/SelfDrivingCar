namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// Controller to compute the score of the car.
/// </summary>
[PublicAPI]
public class CarScoreController : IController
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <inheritdoc />
    public void Simulate(double simulationTime, double timeDelta)
    {
        // TODO: implement scoring.
    }
}
