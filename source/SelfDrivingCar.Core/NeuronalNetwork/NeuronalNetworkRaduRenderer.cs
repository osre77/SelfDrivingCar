using System.Drawing;
using System.Numerics;
using SelfDrivingCar.Core.Rendering;
using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.NeuronalNetwork;

/// <summary>
/// Renderer for simple Neuronal Network implementation.
/// </summary>
/// <remarks>
/// This is a C# port of the neuronal network visualizer created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
/// </remarks>
public class NeuronalNetworkRaduRenderer
{
    public void Render(NeuronalNetworkRadu network, RenderContext context, Vector4 viewport, float zoomLevel)
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
                        color, 2f / zoomLevel, new[] { 7f, 3f });
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
                    Color.DimGray, 1f / zoomLevel, null, Color.Black);

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
                    Color.DimGray, 1f / zoomLevel, null, Color.Black);

                context.DrawEllipse(new Vector2(GetX(o, outputCount), GetY(l + 1)), neuronSize, neuronSize,
                    null, 0f, null, valueColor);

                context.DrawEllipse(new Vector2(GetX(o, outputCount), GetY(l + 1)), neuronSize * 1.1f, neuronSize * 1.1f,
                    biasColor, 2f / zoomLevel, new []{3f, 3f}, null);
            }
        }
    }
}
