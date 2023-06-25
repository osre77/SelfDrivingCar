using SelfDrivingCar.Core.NeuronalNetwork;
using SelfDrivingCar.Core.Rendering;
using System.Numerics;

namespace SelfDrivingCar.Wpf.Rendering;
internal class WpfNeuronalNetworkRaduRenderControl : WpfRenderControl
{
    private readonly NeuronalNetworkRaduRenderer _networkRenderer;

    public NeuronalNetworkRadu? NeuronalNetwork { get; set; }

    public WpfNeuronalNetworkRaduRenderControl()
    {
        _networkRenderer = new NeuronalNetworkRaduRenderer();
    }

    protected override void OnRender(RenderContext renderContext, Vector4 viewport, float zoomFactor)
    {
        if (NeuronalNetwork == null) return;

        _networkRenderer.Render(NeuronalNetwork, renderContext, viewport, zoomFactor);
    }
}
