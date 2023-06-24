using System.Numerics;

namespace SelfDrivingCar.Core.Controller;

public class CarCruiseController : BaseController
{
    public float Speed { get; set; }

    public CarCruiseController(float speed)
    {
        Speed = speed;
    }

    public override void Simulate(double simulationTime, double timeDelta)
    {
        if (Entity == null) return;

        Entity.Position += new Vector2(0f, (float)(Speed * timeDelta));
    }
}
