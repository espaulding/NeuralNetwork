/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace Classifier {
    public class Network {
        public List<int> counts;  //number of neurons in each layer
        public List<List<Neuron>> brain = new List<List<Neuron>>();
        public List<Value> output;
        public List<Value> dataLayer = new List<Value>();

        public Network(List<int> counts, int numDimensions, double defaultEta) {
            this.counts = counts;

            //create a psuedo layer called data that the network will use for the first layer
            for (int i = 0; i < numDimensions; i++) { dataLayer.Add(new Value(0)); }
            
            //make first layer
            brain.Add(new List<Neuron>());
            for (int i = 1; i <= counts[0]; i++) {
                brain[0].Add(new Neuron(dataLayer, defaultEta, i));
            }

            //make all the other layers
            for (int layer = 1; layer < counts.Count; layer++) {
                brain.Add(new List<Neuron>());

                //collect pointers to the outputs of the previous layer
                List<Value> inputs = new List<Value>();
                for (int neuron = 0; neuron < counts[layer - 1]; neuron++) {
                    inputs.Add(brain[layer - 1][neuron].output);
                }

                //create the specified number of neurons for this layer
                for (int i = 1; i <= counts[layer]; i++) {
                    brain[layer].Add(new Neuron(inputs, defaultEta, i));
                }
            }

            //link the output of the last layer to a more accessible array and set target classes
            output = new List<Value>();
            foreach (Neuron n in brain[brain.Count - 1]) { output.Add(n.output); }
        }

        //update the data cloud for the network to draw upon
        public void UpdateDataLayer(Sample s) {
            for (int i = 0; i < s.data.Count; i++) { dataLayer[i].value = Double.Parse(s.data[i]); }
            for (int i = 0; i < brain[brain.Count - 1].Count; i++) {
                brain[brain.Count - 1][i].target = 0;
            }
            brain[brain.Count - 1][int.Parse(s.classLabel) - 1].target = 1;
        }

        public int GetMaxOutputIndex() {
            double max = output[0].value;
            int index = 0;
            for (int i = 1; i < output.Count; i++) {
                if (output[i].value > max) {
                    max = output[i].value;
                    index = i;
                }
            }
            return index;
        }

        //feed the data forward through the network finding the output values for each neuron
        public void FeedForward() {
            for (int layer = 0; layer < counts.Count; layer++) {
                for (int neuron = 0; neuron < counts[layer]; neuron++) {
                    brain[layer][neuron].FeedForward();
                }
            }
        }

        //using the output values work backwards through the network finding the errors
        //and using them to adjust the weights of each neuron
        public void BackProp() {

            //find all the errors in the weights before actually updating a weight
            for (int layer = counts.Count-1; layer >= 0; layer--) {
                for (int neuron = 0; neuron < counts[layer]; neuron++) {
                    if (layer != counts.Count - 1) {
                        brain[layer][neuron].CalcHiddenDelta(brain[layer + 1]);
                    } else {
                        brain[layer][neuron].CalcOutDelta();
                    }
                }
            }

            //now actually update the weights
            for (int layer = counts.Count - 1; layer >= 0; layer--) {
                for (int neuron = 0; neuron < counts[layer]; neuron++) {
                    brain[layer][neuron].UpdateWeights();
                }
            }
        }
    }
}
