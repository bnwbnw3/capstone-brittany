using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class NeuronLayer
    {
        int _numNeurons;
        public List<Neuron> neurons;
        NeuronLayer(int numNeurons, int numInputsEachForNeurons)
        {
            _numNeurons = numNeurons;

            for(int i = 0; i <= _numNeurons; i++)
            {
                neurons.Add(new Neuron(numInputsEachForNeurons));
            }
        }

        public int getNumNeurons()
        {
            return _numNeurons;
        }
        public List<Double> getWeights()
        {
            List<Double> allWeights = new List<Double>();
            foreach (Neuron n in neurons)
            {
                allWeights.AddRange(n.getWeights());
            }
            return allWeights;
        }

        public int getNumWeights()
        {
            int num = 0;
            foreach( Neuron n in neurons)
            {
                num += n.getNumWeights();
            }
            return num;
        }
    }
