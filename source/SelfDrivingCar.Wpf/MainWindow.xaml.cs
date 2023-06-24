using SelfDrivingCar.Core;
using SelfDrivingCar.Core.Colliders;
using SelfDrivingCar.Core.Controller;
using SelfDrivingCar.Core.Extensions;
using SelfDrivingCar.Core.Rendering;
using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Windows;
using DrawingColor = System.Drawing.Color;

namespace SelfDrivingCar.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private CarPhysicsController? _carController;
    private Entity? _heroCar;
    private RoadController? _roadController;

    public EntityGraph? EntityGraph { get; set; }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SetStaticOutput(string output)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.BeginInvoke(new Action<string>(SetStaticOutput), output);
            return;
        }
        StaticOutput.Text = output;
    }

    private void WriteLog(string message)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.BeginInvoke(new Action<string>(WriteLog), message);
            return;
        }
        LogOutput.AppendText(message + "\n");
        LogOutput.ScrollToEnd();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        Reset();
        RenderControl.Focus();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        CleanUp();
    }

    private void CleanUp()
    {
        if (EntityGraph != null)
        {
            EntityGraph.StopSimulation();
        }

        EntityGraph = null;
        _carController = null;
        _heroCar = null;
        _roadController = null;
        _exceptionCount = 0;
    }

    private void Reset()
    {
        CleanUp();

        EntityGraph = new EntityGraph();
        EntityGraph.Add(new Entity()
            .WithCollider(new RoadCollider())
            .WithController(_roadController = new RoadController(3, 3f))
            .WithRenderer(new RoadRenderer(0)));

        AddTrafficCar(0, 10f);

        EntityGraph.Add(_heroCar = new Entity()
            .WithCollider(new CarCollider())
            .WithController(new KeyboardCarInputController(RenderControl.GetInput))
            .WithController(_carController = new CarPhysicsController())
            .WithCarSensorPattern1()
            .WithRenderer(new CarRenderer(2, DrawingColor.Blue))
            .WithRenderer(new DistanceSensorRenderer(3)));

        RenderControl.EntityGraph = EntityGraph;
        RenderControl.ActiveLayers = new[] { 0, 1, 2, 3 };
        RenderControl.Offset = new Point(0d, _heroCar.Position.Y);

        StaticOutput.Clear();
        LogOutput.Clear();

        EntityGraph.FrameSimulated += EntityGraphOnFrameSimulated;
    }

    private void AddTrafficCar(int lane, float position)
    {
        if (_roadController == null) return;

        EntityGraph?.Add(new Entity(new Vector2(_roadController.GetLanePosition(lane), position), 0f)
            .WithCollider(new CarCollider())
            .WithController(new CarCruiseController(10f))
            .WithRenderer(new CarRenderer(1, DrawingColor.Red)));
    }

    private int _exceptionCount;

    private void EntityGraphOnFrameSimulated(object? sender, FrameSimulatedEventArgs e)
    {
        if (_heroCar == null || _carController == null || EntityGraph == null) return;

        RenderControl.Offset = new Point(0d, _heroCar.Position.Y);
        RenderControl.Refresh();

        string exMessage = string.Empty;
        if (e.FrameException != null)
        {
            _exceptionCount++;
            exMessage = $"\nException ({_exceptionCount}):\n{e.FrameException.Message}";
        }

        int aliveCars = EntityGraph.Entities.Count(entity => entity.GetController<CarPhysicsController>()?.IsDead == false);

        SetStaticOutput(
            $"Time: {EntityGraph.SimulationTime:F2}s, Frame: {EntityGraph.FrameCount}\n" +
            $"Alive cars: {aliveCars}\n" +
            "\n" +
            $"Throttle: {_carController.Throttle:F2}\n" +
            $"Acc:      {_carController.Acceleration:F2}px/s²\n" +
            $"Speed:    {_carController.CurrentSpeed:F2}px/s\n" +
            $"Steering: {_carController.SteeringInput:F2}\n" +
            $"Position: {_carController.Entity?.Position.X ?? 0d:F2}; {_carController.Entity?.Position.Y ?? 0d:F2}\n" +
            $"Angle:    {(_carController.Entity?.Angle ?? 0d) / Math.PI * 180d:F1}°\n" +
            $"{exMessage}");

        if (aliveCars == 0)
        {
            EntityGraph.StopSimulation();
        }
    }

    private void Reset_OnClick(object sender, RoutedEventArgs e)
    {
        Reset();
    }

    private void Start_OnClick(object sender, RoutedEventArgs e)
    {
        WriteLog("Starting simulation");
        EntityGraph?.StartSimulation(false);
    }

    private void Pause_OnClick(object sender, RoutedEventArgs e)
    {
        EntityGraph?.StopSimulation();
        WriteLog("Simulation stopped");
    }

    private void ZoomDefault_OnClick(object sender, RoutedEventArgs e)
    {
        RenderControl.ZoomFactor = 20d;
    }

    private void ZoomIn_OnClick(object sender, RoutedEventArgs e)
    {
        RenderControl.ZoomFactor *= 1.25;
    }

    private void ZoomOut_OnClick(object sender, RoutedEventArgs e)
    {
        RenderControl.ZoomFactor /= 1.25;
    }
}
