using SelfDrivingCar.Core.NeuronalNetwork;
using SelfDrivingCar.Core.Sensors;

namespace SelfDrivingCar.Core.Controller;

public class NeuronalNetworkRaduInputController : CarInputController
{
    public NeuronalNetworkRadu NeuronalNetwork { get; }

    public NeuronalNetworkRaduInputController(int sensorCount)
    {
        NeuronalNetwork = new NeuronalNetworkRadu(new[]
        {
            sensorCount,
            6,
            4
        });
    }

    public override void Simulate(double simulationTime, double timeDelta)
    {
        if (Entity == null) return;
        var sensors = Entity.GetSensors<BaseSensor>();

        var outputs = NeuronalNetwork.FeedForward(sensors.Select(s => (double)s.NormalizedValue));
        if (outputs.Length >= 4)
        {
            bool accelerate = outputs[0] > 0;
            bool steerLeft = outputs[1] > 0;
            bool steerRight = outputs[2] > 0;
            bool decelerate = outputs[3] > 0;

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
}
