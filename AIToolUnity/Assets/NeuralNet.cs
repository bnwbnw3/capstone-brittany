using System;
using System.Collections.Generic;

public class NeuralNet
{
    private int _numInputs;
    private int _numOutputs;
    private int _numHiddenLayers;
    private int _neuronsPerHiddenLayer;
    private List<NeuronLayer> allLayers;
    private List<Double> weightsFromNN;

    //TODO: figure out how many layers, hidden, out etc to have and set them in constructor
	public NeuralNet()
	{
	}

    public void CreateNet()
    {
    }

    public List<Double> getWeights() 
    {
        weightsFromNN = new List<Double>();
        foreach(NeuronLayer nl in allLayers)
        {
            weightsFromNN.AddRange(nl.getWeights());
        }

        return weightsFromNN;
    }

    public int getNumOfTotalWeights()
    {
        int num = 0;
        foreach (NeuronLayer nl in allLayers)
        {
            num += nl.getNumWeights();
        }
        return num;
    }

    //replaces the weights with new ones ( is this for ALL weights or some?)
    public void replaceWeights(List<Double> weights)
    {
        throw new NotImplementedException();
    }

    public List<Double> Update(List<Double> inputs)
    {
        List<Double> outputs = new List<Double>();
        int weight = 0;

        //make sure input count is correct
        if (inputs.Count != _numInputs)
        {
            return outputs;
        }

        //go through each layer and calc sigmoid function to get output
        for (int i = 0; i < _numHiddenLayers + 1; i++)
        {
            if (i > 0)
            {
                inputs = outputs;
            }
            outputs.Clear();
            weight = 0;

            for (int j = 0; j < allLayers[i].getNumNeurons(); j++)
            {
                double netInput = 0;
                int numInputs = allLayers[i].neurons[j].getNumInputs();

                for (int k = 0; k < numInputs; k++)
                {
                    netInput += allLayers[i].neurons[j].getWeights()[k] * inputs[weight++];
                }

                //add bias
                netInput += allLayers[i].neurons[j].getWeights()[numInputs-1] * CParams::dBias;
                
                outputs.Add(Sigmoid(netInput, CParams::dActivationResponse));
                weight = 0;

            }
        }
        return outputs;
    }

    public double Sigmoid(double activation, double response)
    {
        throw new NotImplementedException();
    }
}
