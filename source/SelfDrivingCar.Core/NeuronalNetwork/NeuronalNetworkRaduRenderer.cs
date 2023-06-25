using SelfDrivingCar.Core.Rendering;
using SelfDrivingCar.Core.Utils;
using System.Drawing;

namespace SelfDrivingCar.Core.NeuronalNetwork;

/// <summary>
/// Renderer for simple Neuronal Network implementation.
/// </summary>
/// <remarks>
/// This is a C# port of the neuronal network visualizer created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
/// </remarks>
[PublicAPI]
public class NeuronalNetworkRaduRenderer
{
    ///  <summary>
    ///  Renders the neuronal network.
    ///  </summary>
    ///  <param name="network">The neuronal network to render.</param>
    ///  <param name="context">The context to render to.</param>
    ///  <param name="viewport">The current visible viewport.</param>
    ///  <param name="zoomFactor">The current zoom factor.</param>
    ///  <remarks>
    ///  The coordinate system has X from left to right and Y from bottom to top.
    ///  The <paramref name="viewport"/> can be used to cull the rendering.
    ///  The <paramref name="zoomFactor"/> can be used to draw elements independent of the current zoom factor.
    /// e.g. to draw a line at 3 pixels width independent of the current zoom factor set the stroke thickness to <code>3f / zoomFactor</code>.
    ///  </remarks>
    public void Render(NeuronalNetworkRadu network, RenderContext context, Vector4 viewport, float zoomFactor)
    {
        float height = (viewport.W - viewport.Y);
        float width = (viewport.Z - viewport.X);
        float border = height * 0.1f;
        height -= 2 * border;
        width -= 2 * border;
        float neuronSize = height * 0.1f;

        float GetY(int level)
        {
            return CarMath.Lerp(border, border + height, (float)level / network.Levels.Length);
        }

        float GetX(int neuron, int neuronCount)
        {
            return CarMath.Lerp(border, border + width, (float)neuron / (neuronCount - 1));
        }

        Color GetColor(double value)
        {
            return Color.FromArgb(
                (int)(Math.Abs(value) * 255),
                value < 0 ? 0 : 255,
                value < 0 ? 0 : 255,
                value >= 0 ? 0 : 255);
        }

        // draw connections
        for (int l = 0; l < network.Levels.Length; l++)
        {
            var level = network.Levels[l];
            var inputCount = level.Inputs.Length;
            for (int i = 0; i < inputCount; ++i)
            {
                var outputCount = level.Outputs.Length;
                for (int o = 0; o < outputCount; ++o)
                {
                    var weight = level.Weights[i][o];
                    var color = GetColor(weight);
                    context.DrawLine(
                        new Vector2(GetX(i, inputCount), GetY(l)),
                        new Vector2(GetX(o, outputCount), GetY(l + 1)),
                        color, 2f / zoomFactor, new[] { 7f, 3f });
                }
            }
        }

        // draw network input nodes
        {
            var level = network.Levels[0];
            var inputCount = level.Inputs.Length;
            for (int i = 0; i < inputCount; ++i)
            {
                var value = level.Inputs[i];
                var color = GetColor(value);

                context.DrawEllipse(new Vector2(GetX(i, inputCount), GetY(0)), neuronSize * 1.1f, neuronSize * 1.1f,
                    Color.DimGray, 1f / zoomFactor, null, Color.Black);

                context.DrawEllipse(new Vector2(GetX(i, inputCount), GetY(0)), neuronSize, neuronSize,
                    null, 0f, null, color);
            }
        }

        // draw level outputs
        for (int l = 0; l < network.Levels.Length; l++)
        {
            var level = network.Levels[l];
            var outputCount = level.Outputs.Length;
            for (int o = 0; o < outputCount; ++o)
            {
                var value = level.Outputs[o];
                var bias = level.Biases[o];
                var valueColor = GetColor(value);
                var biasColor = GetColor(bias);

                context.DrawEllipse(new Vector2(GetX(o, outputCount), GetY(l + 1)), neuronSize * 1.1f, neuronSize * 1.1f,
                    Color.DimGray, 1f / zoomFactor, null, Color.Black);

                context.DrawEllipse(new Vector2(GetX(o, outputCount), GetY(l + 1)), neuronSize, neuronSize,
                    null, 0f, null, valueColor);

                context.DrawEllipse(new Vector2(GetX(o, outputCount), GetY(l + 1)), neuronSize * 1.1f, neuronSize * 1.1f,
                    biasColor, 2f / zoomFactor, new[] { 3f, 3f }, null);
            }
        }
    }
}
