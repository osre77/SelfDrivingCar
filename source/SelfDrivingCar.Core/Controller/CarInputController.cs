namespace SelfDrivingCar.Core.Controller;

public abstract class CarInputController : BaseController
{
    public float Throttle { get; set; }

    public float SteeringInput { get; set; }
}
