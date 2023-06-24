namespace SelfDrivingCar.Core.Controller;

public abstract class BaseController : Component
{
    public abstract void Simulate(double simulationTime, double timeDelta);
}
