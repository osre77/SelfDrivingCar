using SelfDrivingCar.Core.Rendering;

namespace SelfDrivingCar.Core;

/// <summary>
/// A graph of <see cref="Entity"/>s.
/// </summary>
[PublicAPI]
public class EntityGraph
{
    private Timer? _simulationTimer;
    private double _simulationFrequency = 60d;
    private double _simulationTimeScale = 1d;

    /// <summary>
    /// Gets the entities of the graph.
    /// </summary>
    public IList<Entity> Entities { get; } = new List<Entity>();

    /// <summary>
    /// Gets the number of simulated frames.
    /// </summary>
    public int FrameCount { get; private set; }

    /// <summary>
    /// Gets the total simulation time in s.
    /// </summary>
    public double SimulationTime { get; private set; }

    /// <summary>
    /// Gets or sets the simulation frequency.
    /// </summary>
    public double SimulationFrequency
    {
        get => _simulationFrequency;
        set
        {
            _simulationFrequency = value;
            UpdateSimulationTimer();
        }
    }

    /// <summary>
    /// Gets or sets the time scaling of the simulation.
    /// </summary>
    public double SimulationTimeScale
    {
        get => _simulationTimeScale;
        set
        {
            _simulationTimeScale = value;
            UpdateSimulationTimer();
        }
    }

    /// <summary>
    /// Occurs after a frame was simulated.
    /// </summary>
    public event EventHandler<FrameSimulatedEventArgs>? FrameSimulated;

    /// <summary>
    /// Renders all entities in the graph.
    /// </summary>
    /// <param name="context">The context to render to.</param>
    /// <param name="layers">The layer numbers to render. Only the matching renderer components will be rendered.</param>
    /// <param name="viewport">The current visible viewport.</param>
    /// <param name="zoomFactor">The current zoom factor.</param>
    /// <remarks>
    /// The coordinate system has X from left to right and Y from bottom to top.
    /// The <paramref name="viewport"/> can be used to cull the rendering.
    /// The <paramref name="zoomFactor"/> can be used to draw elements independent of the current zoom factor.
    /// e.g. to draw a line at 3 pixels width independent of the current zoom factor set the stroke thickness to <code>3f / zoomFactor</code>.
    /// </remarks>
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

    /// <summary>
    /// Adds new <see cref="Entity"/>.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public void Add(Entity entity)
    {
        entity.Graph = this;
        Entities.Add(entity);
    }

    /// <summary>
    /// Simulates a single frame.
    /// </summary>
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

    /// <summary>
    /// Start the simulation.
    /// </summary>
    /// <param name="reset"><c>true</c> to reset the <see cref="FrameCount"/> and <see cref="SimulationTime"/></param>.
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

    /// <summary>
    /// Stop (pause) the simulation.
    /// </summary>
    public void StopSimulation()
    {
        if (_simulationTimer != null)
        {
            _simulationTimer.Dispose();
            _simulationTimer = null;
        }
    }

    private void OnFrameSimulated(double simulationTime, int frameCount, Exception? exception = null)
    {
        FrameSimulated?.Invoke(this, new FrameSimulatedEventArgs(simulationTime, frameCount, exception));
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

/// <summary>
/// Event arguments for frame simulation events.
/// </summary>
/// <inheritdoc />
[PublicAPI]
public class FrameSimulatedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the simulation time in s of the event.
    /// </summary>
    public double SimulationTime { get; }

    /// <summary>
    /// Gets the total frame count of the event.
    /// </summary>
    public int FrameCount { get; }

    /// <summary>
    /// Gets the exception of the event.
    /// </summary>
    /// <remarks>
    /// If the frame had no errors the value is <c>null</c>.
    /// </remarks>
    public Exception? FrameException { get; }

    /// <summary>
    /// Creates a new instance of the event arguments.
    /// </summary>
    /// <param name="simulationTime">The simulation time in s.</param>
    /// <param name="frameCount">The number of simulated frames.</param>
    /// <param name="frameException">The exception of the frame or <c>null</c> if there was no error.</param>
    public FrameSimulatedEventArgs(double simulationTime, int frameCount, Exception? frameException = null)
    {
        SimulationTime = simulationTime;
        FrameCount = frameCount;
        FrameException = frameException;
    }
}
