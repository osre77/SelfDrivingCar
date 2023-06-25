using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Controller;

/// <summary>
/// A controller for computing acceleration and steering physics for a car that are an approximation of a real car.
/// </summary>
/// <remarks>
/// The acceleration and deceleration is computed by the throttle input and a speed dependent drag value.
/// The steering is computed by the steering input but also depends on the current speed.
/// The component also keeps some statistics that can be used in order to compute a scoring for the car.
/// The component will get the 1st <see cref="ICarInputController"/> from the <see cref="Entity"/> for throttle and steering input.
/// </remarks>
[PublicAPI]
public class CarPhysicsController : IController
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <summary>
    /// Gets or sets the current throttle value as a value between -1 and 1.
    /// </summary>
    public float Throttle { get; set; }

    /// <summary>
    /// Gets or sets the maximum forward acceleration in m/s².
    /// </summary>
    public float MaxForwardAcceleration { get; set; } = 20f;

    /// <summary>
    /// Gets or sets the maximum reverse acceleration in m/s².
    /// </summary>
    public float MaxReverseAcceleration { get; set; } = 10f;

    /// <summary>
    /// Gets or sets the maximum breaking acceleration (deceleration) in m/s².
    /// </summary>
    public float MaxBreakingAcceleration { get; set; } = 30f;

    /// <summary>
    /// Gets the base value for the drag in m/s².
    /// </summary>
    public float DragBase { get; set; } = 1f;

    /// <summary>
    /// Gets or sets the drag factor in 1/m.
    /// </summary>
    public float DragFactor { get; set; } = 0.02f;

    /// <summary>
    /// Gets the current acceleration of the car in m/s².
    /// </summary>
    /// <remarks>
    /// Negative values mean that the vehicle is decelerating or accelerating in reverse.
    /// </remarks>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Gets or sets the maximum forward speed in m/s.
    /// </summary>
    public float MaxForwardSpeed { get; set; } = 15f;

    /// <summary>
    /// Gets or sets the maximum reverse speed in m/s.
    /// </summary>
    public float MaxReverseSpeed { get; set; } = 5f;

    /// <summary>
    /// Gets the current speed in m/s.
    /// </summary>
    public float CurrentSpeed { get; private set; }

    /// <summary>
    /// Gets the current steering input as a value between -1 nad 1.
    /// </summary>
    public float SteeringInput { get; set; }

    /// <summary>
    /// Gets the maximum steering value in rad/m.
    /// </summary>
    public float SteeringFactor { get; set; } = (float)(5 / 180d * Math.PI);

    /// <summary>
    /// Gets if the car is dead or alive.
    /// </summary>
    /// <remarks>
    /// A car is dead if it collides with the road border or a traffic car.
    /// </remarks>
    public bool IsDead { get; private set; }

    /// <summary>
    /// Gets the time of death in s after the start of the simulation.
    /// </summary>
    public double TimeOfDeath { get; private set; }

    /// <summary>
    /// Gets the distance traveled in m.
    /// </summary>
    /// <remarks>This is the distance traveled along the road.
    /// <see cref="DistanceMoved"/> for the actual moved distance.</remarks>
    public double DistanceTraveled { get; private set; }

    /// <summary>
    /// Gets the distance moved in m.
    /// </summary>
    /// <remarks>Driving in reverse will reduce the value.</remarks>
    public double DistanceMoved { get; private set; }

    /// <summary>
    /// Gets the average speed in m/s.
    /// </summary>
    public double AverageSpeed { get; private set; }

    public void Simulate(double simulationTime, double timeDelta)
    {
        if (IsDead || Entity?.Graph == null) return;

        var inputController = Entity.GetController<ICarInputController>();
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

        if (CurrentSpeed >= 0)
        {
            DistanceMoved += vector.Length();
        }
        else
        {
            DistanceMoved -= vector.Length();
        }

        DistanceTraveled = Entity.Position.Y;

        AverageSpeed = DistanceMoved / simulationTime;

        var collider = Entity.GetCollider<CarCollider>();

        if (collider != null)
        {
            var collision = Entity.Graph.Entities
                .Where(e => !ReferenceEquals(e, Entity) && e.GetController<CarPhysicsController>() == null)
                .SelectMany(e => e.Colliders)
                .Any(c => c.CheckPolygonCollision(collider.GetPolygonGeometry().First()));

            if (collision)
            {
                IsDead = true;
                TimeOfDeath = simulationTime;
            }
        }
    }
}
