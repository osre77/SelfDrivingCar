using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.Parameters;

/// <summary>
/// Parameter set for a straight road.
/// </summary>
public class RoadParameterSet : IParameterSet
{
    /// <inheritdoc />
    public Entity? Entity { get; set; }

    /// <summary>
    /// Gets the lane count.
    /// </summary>
    public int LaneCount { get; }

    /// <summary>
    /// Gets the width of a lane in m.
    /// </summary>
    public float LaneWidth { get; }

    /// <summary>
    /// Gets the position of the left border.
    /// </summary>
    public float LeftBorder { get; }

    /// <summary>
    /// Gets the position of the right border.
    /// </summary>
    public float RightBorder { get; }

    /// <summary>
    /// Creates a new road parameter set.
    /// </summary>
    /// <param name="laneCount">Number of lanes.</param>
    /// <param name="laneWidth">Width of a lane in m.</param>
    public RoadParameterSet(int laneCount, float laneWidth)
    {
        LaneCount = laneCount;
        LaneWidth = laneWidth;
        RightBorder = laneWidth * laneCount / 2;
        LeftBorder = -RightBorder;
    }

    /// <summary>
    /// Gets the x-position af a lane.
    /// </summary>
    /// <param name="index">Index of the lane between 0 and <see cref="LaneCount"/> - 1.</param>
    /// <returns>Returns the x-position.</returns>
    public float GetLanePosition(int index)
    {
        return CarMath.Lerp(LeftBorder + LaneWidth / 2, RightBorder + LaneWidth / 2, (float)index / LaneCount);
    }
}
