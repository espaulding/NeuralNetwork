/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Classifier {
    class Program {
        static void Main(string[] args) {
            //*** Settings ***
            List<int> counts;
            string datafile   = "iris.csv";
            string classLabel = "Species";
            int numberOfFolds = 10;

            if (args.Length == 0) {
                counts = new int[] { 3, 3, 3}.ToList();
            } else {
                counts = args.Select(x => Convert.ToInt32(x)).ToList();
            }

            string error; //load the data
            DataSet data = FileIO.LoadCSV(datafile, out error);
            DataTable t = data.Tables[0];

            //configure the neural network
            NeuralNetwork.classLabel = classLabel;
            NeuralNetwork.allValues = DataTableHelper.GetDistinctValues(t, classLabel, ref NeuralNetwork.classIndex);
            NeuralNetwork.classValues = DataTableHelper.GetClassValues(t, classLabel);
            NeuralNetwork.counts = counts;
            Console.Write("The network will have size ");
            counts.ForEach(i => Console.Write("{0} ", i));
            Console.WriteLine("\n");

            //do 10-fold cross validation using the set order for possible parents to each dimension
            ConfusionMatrix m = NeuralNetwork.CrossValidation(t, numberOfFolds);
            Console.WriteLine(m.ToString());

            data.Dispose();
            Console.ReadLine();
        }
    }
}
