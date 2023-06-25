namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// Car controller for manually controlling a car with keyboard.
/// </summary>
[PublicAPI]
public class KeyboardCarInputController : ICarInputController
{
    private readonly GetInputCallback _getInputCallback;

    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <inheritdoc />
    public float Throttle { get; set; }

    /// <inheritdoc />
    public float SteeringInput { get; set; }

    /// <summary>
    /// Delegate for a keyboard input callback.
    /// </summary>
    /// <param name="accelerate">Must beset to true if the accelerate key is currently down.</param>
    /// <param name="decelerate">Must beset to true if the decelerate key is currently down.</param>
    /// <param name="steerLeft">Must beset to true if the steer left key is currently down.</param>
    /// <param name="steerRight">Must beset to true if the steer right key is currently down.</param>
    public delegate void GetInputCallback(out bool accelerate, out bool decelerate, out bool steerLeft, out bool steerRight);

    /// <summary>
    /// Creates a new instance of the controller.
    /// </summary>
    /// <param name="getInputCallback">Callback for getting the keyboard inputs.</param>
    public KeyboardCarInputController(GetInputCallback getInputCallback)
    {
        _getInputCallback = getInputCallback;
    }

    /// <inheritdoc />
    public void Simulate(double simulationTime, double timeDelta)
    {
        Throttle = 0f;
        SteeringInput = 0f;

        _getInputCallback(out var accelerate, out var decelerate, out var steerLeft, out var steerRight);

        if (accelerate == decelerate)
        {
            Throttle = 0f;
        }
        else if (accelerate)
        {
            Throttle = 1f;
        }
        else if (decelerate)
        {
            Throttle = -1f;
        }

        if (steerLeft == steerRight)
        {
            SteeringInput = 0f;
        }
        else if (steerRight)
        {
            SteeringInput = 1f;
        }
        else if (steerLeft)
        {
            SteeringInput = -1f;
        }
    }
}
