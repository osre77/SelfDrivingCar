using SelfDrivingCar.Core.Rendering;
using System.Numerics;

namespace SelfDrivingCar.Core;

public class EntityGraph
{
    private Timer? _simulationTimer;
    private double _simulationFrequency = 60d;
    private double _simulationTimeScale = 1d;

    public IList<Entity> Entities { get; } = new List<Entity>();

    public void Render(RenderContext context, int[] layers, Vector4 viewport, float zoomFactor)
    {
        foreach (var layer in layers)
        {
            foreach (var entity in Entities)
            {
                entity.Render(context, layer, viewport, zoomFactor);
            }
        }
    }

    public void Add(Entity entity)
    {
        entity.Graph = this;
        Entities.Add(entity);
    }

    public int FrameCount { get; private set; }

    public double SimulationTime { get; private set; }

    public double SimulationFrequency
    {
        get => _simulationFrequency;
        set
        {
            _simulationFrequency = value;
            UpdateSimulationTimer();
        }
    }

    public double SimulationTimeScale
    {
        get => _simulationTimeScale;
        set
        {
            _simulationTimeScale = value;
            UpdateSimulationTimer();
        }
    }

    public void SimulateFrame()
    {
        FrameCount += 1;

        var timeDelta = 1d / SimulationFrequency;
        SimulationTime += timeDelta;

        try
        {
            foreach (var entity in Entities)
            {
                entity.Simulate(SimulationTime, timeDelta);
            }

            OnFrameSimulated(SimulationTime, FrameCount);
        }
        catch (Exception ex)
        {
            OnFrameSimulated(SimulationTime, FrameCount, ex);
        }
    }

    private void OnFrameSimulated(double simulationTime, int frameCount, Exception? exception = null)
    {
        FrameSimulated?.Invoke(this, new FrameSimulatedEventArgs(simulationTime, frameCount, exception));
    }

    public event EventHandler<FrameSimulatedEventArgs>? FrameSimulated;

    public void StartSimulation(bool reset)
    {
        if (reset)
        {
            FrameCount = 0;
            SimulationTime = 0d;
        }
        if (_simulationTimer == null)
        {
            _simulationTimer = new Timer(_ =>
            {
                SimulateFrame();
            });
            UpdateSimulationTimer();
        }
    }

    public void StopSimulation()
    {
        if (_simulationTimer != null)
        {
            _simulationTimer.Dispose();
            _simulationTimer = null;
        }
    }

    private void UpdateSimulationTimer()
    {
        if (_simulationTimer == null) return;

        if (SimulationFrequency <= 0d || SimulationTimeScale <= 0d)
        {
            _simulationTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        else
        {
            var interval = (int)(1d / (SimulationFrequency * SimulationTimeScale) * 1000);
            _simulationTimer.Change(interval, interval);
        }
    }
}

public class FrameSimulatedEventArgs : EventArgs
{
    public double SimulationTime { get; }

    public int FrameCount { get; }

    public Exception? FrameException { get; }

    public FrameSimulatedEventArgs(double simulationTime, int frameCount, Exception? frameException = null)
    {
        SimulationTime = simulationTime;
        FrameCount = frameCount;
        FrameException = frameException;
    }
}
