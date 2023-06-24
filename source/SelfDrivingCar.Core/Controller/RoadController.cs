using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Controller;

public class RoadController : BaseController
{
    public RoadController(int laneCount, float laneWidth)
    {
        LaneCount = laneCount;
        LaneWidth = laneWidth;
        RightBorder = (laneWidth * laneCount) / 2;
        LeftBorder = -RightBorder;
    }

    public int LaneCount { get; }

    public float LaneWidth { get; }

    public float LeftBorder { get; }

    public float RightBorder { get; }

    public float GetLanePosition(int index)
    {
        return CarMath.Lerp(LeftBorder + LaneWidth / 2, RightBorder + LaneWidth / 2, (float)index / LaneCount);
    }

    public override void Simulate(double simulationTime, double timeDelta)
    { }
}
