/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifier {
    public class Neuron {
        private static Random weightGen = new Random();
        public List<double> weights;
        public List<Value> inputs = new List<Value>();
        public Value output = new Value(0);
        public double delta = 0;
        public double target = .5;
        public double eta;
        public int number;

        public Neuron(List<Value> invalues, double eta, int index) {
            this.number = index;           //this this neuron what number it is in its layer. neuron indexes start at 1 not 0
            this.eta = eta;                //set the learning rate for this neuron
            this.inputs.Add(new Value(1)); //add the bias to the front of the input
            this.inputs.AddRange(invalues);  //now add incoming values starting from index 1

            weights = new List<double>();  //start weights randomly betwen -.05 and .05
            for (int i = 0; i < inputs.Count;i++) { weights.Add(weightGen.NextDouble() * .1 - .05); }
        }

        public void FeedForward() {
            output.value = 0; //find linear combination
            for (int i = 0; i < inputs.Count; i++) {
                output.value += inputs[i].value * weights[i];
            }
            Sigmoid(output); //next send the value through the sigmoid function
        }

        //update the value sent in directly
        public void Sigmoid(Value v) {
            v.value = 1 / (1 + Math.Exp(-1 * v.value));
        }

        public void CalcOutDelta() {
            delta = output.value*(1-output.value)*(target-output.value);
        }

        public void CalcHiddenDelta(List<Neuron> outputs) {
            double sum = 0;
            foreach (Neuron n in outputs) {
                sum += n.weights[number] * n.delta;
            }
            delta = output.value * (1 - output.value) * sum;
        }

        public void UpdateWeights() {
            for (int i = 0; i < inputs.Count; i++) {
                weights[i] += eta * delta * inputs[i].value;
            }
        }
    }
}
