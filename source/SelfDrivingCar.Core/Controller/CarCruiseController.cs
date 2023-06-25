namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// Implementation of a controller that will cruise a car at a constant speed.
/// </summary>
[PublicAPI]
public class CarCruiseController : IController
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <summary>
    /// Gets or sets the cruise speed in m/s.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// Creates a new instance of the controller.
    /// </summary>
    /// <param name="speed">The cruise speed in m/s.</param>
    public CarCruiseController(float speed)
    {
        Speed = speed;
    }

    /// <inheritdoc />
    public void Simulate(double simulationTime, double timeDelta)
    {
        if (Entity == null) return;

        Entity.Position += new Vector2(0f, (float)(Speed * timeDelta));
    }
}
