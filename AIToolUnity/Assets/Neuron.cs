using System;
using System.Collections.Generic;

public class Neuron
{
    int _numInputs;
    List<Double> weights;

    public Neuron(int numInputs)
	{
        //+1 for bias
        _numInputs = numInputs+1;
        weights = new List<Double>();

        //initialize weights to be random.
        for (int i = 0; i < numInputs + 1; i++)
        {
            weights.Add( new Random().NextDouble());
        }

	}

    public List<Double> getWeights()
    {
        return weights;
    }

    public int getNumInputs()
    {
        return _numInputs;
    }

    public int getNumWeights()
    {
        return weights.Count;
    }

}
