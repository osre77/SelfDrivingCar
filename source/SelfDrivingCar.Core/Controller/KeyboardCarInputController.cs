namespace SelfDrivingCar.Core.Controller;

public class KeyboardCarInputController : CarInputController
{
    private readonly GetInputCallback _getInputCallback;


    public delegate void GetInputCallback(out bool accelerate, out bool decelerate, out bool steerLeft, out bool steerRight);

    public KeyboardCarInputController(GetInputCallback getInputCallback)
    {
        _getInputCallback = getInputCallback;
    }

    public override void Simulate(double simulationTime, double timeDelta)
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
