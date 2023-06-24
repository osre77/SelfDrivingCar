namespace SelfDrivingCar.Core.Sensors;

public abstract class BaseSensor : Component
{
    public float Value { get; set; }

    public float NormalizedValue { get; set; }

    public abstract void Simulate(double simulationTime, double timeDelta);
}
