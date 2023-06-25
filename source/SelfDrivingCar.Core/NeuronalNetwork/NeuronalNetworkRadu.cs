using SelfDrivingCar.Core.Utils;

namespace SelfDrivingCar.Core.NeuronalNetwork;

/// <summary>
/// Simple Neuronal Network implementation.
/// </summary>
/// <remarks>
/// This is a C# port of the neuronal network created by Radu Mariescu-Istodor for the self-driving car project on www.radufromfinland.com.
/// </remarks>
public class NeuronalNetworkRadu
{
    public Level[] Levels { get; }

    public NeuronalNetworkRadu(int[] neuronCounts)
    {
        Levels = new Level[neuronCounts.Length - 1];
        for (int l = 0; l < Levels.Length; l++)
        {
            Levels[l] = new(neuronCounts[l], neuronCounts[l + 1]);
        }
    }

    public double[] FeedForward(IEnumerable <double> givenInputs)
    {
        var inputValues = givenInputs.ToArray();
        var outputs = Levels[0].FeedForward(inputValues);

        for (int l = 1; l < Levels.Length; l++)
        {
            outputs = Levels[l].FeedForward(outputs);
        }
        return outputs;
    }

    public void Mutate(double amount = 1d)
    {
        foreach (var level in Levels)
        {
            level.Mutate(amount);
        }
    }

    public class Level
    {
        public double[] Inputs { get; }

        public double[] Outputs { get; }

        public double[] Biases { get; }

        public double[][] Weights { get; }

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