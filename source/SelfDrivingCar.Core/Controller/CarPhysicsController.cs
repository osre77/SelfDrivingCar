using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Controller;

public class CarPhysicsController : BaseController
{
    public float Throttle { get; set; }

    public float MaxForwardAcceleration { get; set; } = 20f;

    public float MaxReverseAcceleration { get; set; } = 10f;

    public float MaxBreakingAcceleration { get; set; } = 30f;

    public float DragBase { get; set; } = 1f;

    public float DragFactor { get; set; } = 0.02f;

    public float Acceleration { get; set; }

    public float MaxForwardSpeed { get; set; } = 15f;

    public float MaxReverseSpeed { get; set; } = 5f;

    public float CurrentSpeed { get; set; }

    public float SteeringInput { get; set; }

    public float SteeringFactor { get; set; } = (float)(5 / 180d * Math.PI);

    public bool IsDead { get; set; }

    public override void Simulate(double simulationTime, double timeDelta)
    {
        if (IsDead || Entity?.Graph == null) return;

        var inputController = Entity.GetController<CarInputController>();
        if (inputController != null)
        {
            Throttle = CarMath.Bound(inputController.Throttle, -1f, 1f);
            SteeringInput = CarMath.Bound(inputController.SteeringInput, -1f, 1f);
        }

        bool isBreaking = Math.Sign(Throttle) != Math.Sign(CurrentSpeed);

        if (Throttle > 0f)
        {
            Acceleration = Throttle * (isBreaking ? MaxBreakingAcceleration : MaxForwardAcceleration);
        }
        else if (Throttle < 0f)
        {
            Acceleration = Throttle * (isBreaking ? MaxBreakingAcceleration : MaxReverseAcceleration);
        }
        else
        {
            Acceleration = 0f;
        }

        if (CurrentSpeed > 0f)
        {
            Acceleration -= CurrentSpeed * CurrentSpeed * DragFactor + DragBase;
        }
        else if (CurrentSpeed < 0f)
        {
            Acceleration += CurrentSpeed * CurrentSpeed * DragFactor + DragBase;
        }

        CurrentSpeed = CarMath.Bound(CurrentSpeed + Acceleration * timeDelta, -MaxReverseSpeed, MaxForwardSpeed);

        if (Math.Abs(Throttle) < 0.01 && Math.Abs(CurrentSpeed) < 0.5)
        {
            CurrentSpeed = 0f;
        }

        var angle = Entity.Angle + (float)(SteeringInput * SteeringFactor * CurrentSpeed * timeDelta);

        if (angle > Math.PI)
        {
            angle -= (float)(Math.PI * 2f);
        }
        if (angle < -Math.PI)
        {
            angle += (float)(Math.PI * 2f);
        }

        var vector = CarMath.GetCirclePoint((float)(CurrentSpeed * timeDelta), angle);

        Entity.Position += vector;
        Entity.Angle = angle;

        var collider = Entity.GetCollider<CarCollider>();

        if (collider != null)
        {
            var collision = Entity.Graph.Entities
                .Where(e => !ReferenceEquals(e, Entity) && e.GetController<CarPhysicsController>() == null)
                .SelectMany(e => e.Colliders)
                .Any(c => c.CheckPolygonCollision(collider.GetPolyGeometry().First()));

            if (collision)
            {
                IsDead = true;
            }
        }
    }
}
