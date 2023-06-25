using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.NeuronalNetwork;

/// <summary>
/// Simple Neuronal Network implementation.
/// </summary>
/// <remarks>
/// This is a C# port of the neuronal network created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
/// </remarks>
[PublicAPI]
public class NeuronalNetworkRadu
{
    /// <summary>
    /// Gets the levels of the network.
    /// </summary>
    public Level[] Levels { get; }

    /// <summary>
    /// Create anew instance of a neuronal network.
    /// </summary>
    /// <param name="neuronCounts">Array with number of neurons in each layer.</param>
    /// <remarks>The number of layers will be 1 less then the number of layers provided in <paramref name="neuronCounts"/>.</remarks>
    public NeuronalNetworkRadu(int[] neuronCounts)
    {
        Levels = new Level[neuronCounts.Length - 1];
        for (int l = 0; l < Levels.Length; l++)
        {
            Levels[l] = new(neuronCounts[l], neuronCounts[l + 1]);
        }
    }

    /// <summary>
    /// Calculate new output values by feeding forward new input values.
    /// </summary>
    /// <param name="givenInputs">Array of new input values.</param>
    /// <returns>Returns the values of the network outputs.</returns>
    public double[] FeedForward(IEnumerable<double> givenInputs)
    {
        var inputValues = givenInputs.ToArray();
        var outputs = Levels[0].FeedForward(inputValues);

        for (int l = 1; l < Levels.Length; l++)
        {
            outputs = Levels[l].FeedForward(outputs);
        }
        return outputs;
    }

    /// <summary>
    /// Mutate the current weights and biases of the network.
    /// </summary>
    /// <param name="amount">The amount by which the weights and biases are allowed to be muted at max as a value between 0 and 1.</param>
    public void Mutate(double amount = 1d)
    {
        foreach (var level in Levels)
        {
            level.Mutate(amount);
        }
    }

    /// <summary>
    /// A level of the network.
    /// </summary>
    /// <remarks>
    /// A level is a set of input and output neurons connected by weights.
    /// </remarks>
    public class Level
    {
        /// <summary>
        /// Gets the input values.
        /// </summary>
        public double[] Inputs { get; }

        /// <summary>
        /// Gets the output values.
        /// </summary>
        public double[] Outputs { get; }

        /// <summary>
        /// Gets the biases of the outputs.
        /// </summary>
        public double[] Biases { get; }

        /// <summary>
        /// Gets the weights of the neuron connections.
        /// </summary>
        public double[][] Weights { get; }

        /// <summary>
        /// Creates a new level.
        /// </summary>
        /// <param name="inputCount">Number of input neurons.</param>
        /// <param name="outputCount">Number of output neurons.</param>
        public Level(int inputCount, int outputCount)
        {
            Inputs = new double[inputCount];
            Outputs = new double[outputCount];
            Biases = new double[outputCount];

            Weights = new double[inputCount][];
            for (int i = 0; i < inputCount; ++i)
            {
                Weights[i] = new double[outputCount];
            }

            Randomize();
        }

        /// <summary>
        /// Calculate new output values by feeding forward new input values.
        /// </summary>
        /// <param name="givenInputs">Array of new input values.</param>
        /// <returns>Returns the values of the level outputs.</returns>
        public double[] FeedForward(double[] givenInputs)
        {
            for (int i = 0; i < Inputs.Length && i < givenInputs.Length; i++)
            {
                Inputs[i] = givenInputs[i];
            }

            for (int o = 0; o < Outputs.Length; o++)
            {
                double sum = 0d;
                for (int i = 0; i < Inputs.Length; i++)
                {
                    sum += Inputs[i] * Weights[i][o];
                }

                if (sum > Biases[o])
                {
                    Outputs[o] = 1d;
                }
                else
                {
                    Outputs[o] = 0d;
                }
            }

            return Outputs;
        }

        /// <summary>
        /// Mutate the current weights and biases of the level.
        /// </summary>
        /// <param name="amount">The amount by which the weights and biases are allowed to be muted at max as a value between 0 and 1.</param>
        public void Mutate(double amount)
        {
            var random = new Random();

            for (int b = 0; b < Biases.Length; b++)
            {
                Biases[b] = CarMath.Lerp(Biases[b], random.NextDouble() * 2d - 1d, amount);
            }
            for (int i = 0; i < Weights.Length; i++)
            {
                for (int o = 0; o < Weights[i].Length; o++)
                {
                    Weights[i][o] = CarMath.Lerp(Weights[i][o], random.NextDouble() * 2d - 1d, amount);
                }
            }
        }

        private void Randomize()
        {
            var random = new Random();

            for (int i = 0; i < Inputs.Length; i++)
            {
                for (int o = 0; o < Outputs.Length; o++)
                {
                    Weights[i][o] = random.NextDouble() * 2d - 1d;
                }
            }

            for (int b = 0; b < Biases.Length; b++)
            {
                Biases[b] = random.NextDouble() * 2d - 1d;
            }
        }
    }
}