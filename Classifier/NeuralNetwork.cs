/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Classifier {
    //currently this classifier assumes that the data is discrete
    //if continuous data needs to be classified then the data must be binned
    //into discrete values first
    static public class NeuralNetwork {
        static public string classLabel;
        static public int    classIndex = -1;
        static public List<List<string>> allValues;
        static public List<string> classValues;
        static public List<int> counts = null;
        static public double eta = .05;
        static public Network neural;

        #region API Functions

        //consume a data to build a neural network
        static public void Train(DataTable t) {
            ConfusionMatrix matrix = null;
            neural = new Network(counts, t.Columns.Count - 1, eta);

            List<Sample> data = new List<Sample>();
            foreach (DataRow row in t.Rows) { data.Add(new Sample(row, classIndex)); }

            bool doEpoch = true; //int epoch = 1;
            while (doEpoch) {

                foreach (Sample s in data) { //do a single epoch over the data
                    neural.UpdateDataLayer(s);
                    neural.FeedForward();
                    neural.BackProp();
                }

                matrix = Test(t);
                if (matrix.accuracy > .97) { doEpoch = false; }
                //Console.WriteLine("Epoch {0} got {1} samples wrong out of {2}", epoch, matrix.numWrong, data.Count);
                //epoch++;
                //if ((epoch % 100) == 0) {
                //    Console.ReadLine();
                //}
            }
        }

        static public ConfusionMatrix Test(DataTable t) {
            ConfusionMatrix cmat = new ConfusionMatrix(classValues);
            int numDims = t.Columns.Count - 1; //remove 1 for the class labels
            foreach (DataRow row in t.Rows) {
                //convert the datarow into an expression
                Sample sample = new Sample(row, classIndex);
               
                //test the sample and update results
                string prediction = TestInstance(sample);
                cmat.matrix[prediction][sample.classLabel]++;
                if (sample.classLabel.Equals(prediction)) {
                    cmat.numCorrect++;
                } else {
                    cmat.numWrong++;
                }
            }
            cmat.accuracy = ((double)cmat.numCorrect) / t.Rows.Count;
            return cmat;
        }

        static public string TestInstance(Sample sample) {
            neural.UpdateDataLayer(sample);
            neural.FeedForward();
            return (neural.GetMaxOutputIndex() + 1).ToString();
        }

        static public ConfusionMatrix CrossValidation(DataTable data, int folds) {
            DataTable t = CollectionExtensions.OrderRandomly(data.AsEnumerable()).CopyToDataTable();
            ConfusionMatrix res = new ConfusionMatrix(classValues);
            int n = t.Rows.Count;
            int c = (int)Math.Floor(n / (double)folds);
            int start = 0;
            int stop = c - 1;
            int fold = 1;

            while (start < n) {
                if (stop > n) { stop = n; }

                //use start and stop to get a subset of data as test
                //set everything else as train
                IEnumerable<DataRow> testEnum = null, trainEnum = null;
                CollectionExtensions.Split(t.AsEnumerable(), start, stop, ref testEnum, ref trainEnum);
                DataTable trainingData = trainEnum.CopyToDataTable();

                start = stop + 1;
                stop = start - 1 + c;

                Train(trainingData);

                ConfusionMatrix subres = Test(testEnum.CopyToDataTable());
                res.Add(subres); //also updates the accuracy
                fold++;
            }
            return res;
        }

        #endregion //API Functions

        #region Helper Functions

        static private List<int> SetMinus(List<int> lhs, List<int> rhs) {
            List<int> res = lhs.ToList();
            foreach (int i in rhs) {
                if (res.Contains(i)) { res.Remove(i); }
            }
            return res;
        }

        static private List<int> SetMinus(List<int> lhs, int rhs) {
            List<int> res = lhs.ToList();
            if (res.Contains(rhs)) { res.Remove(rhs); }
            return res;
        }

        static private List<int> SetPlus(List<int> lhs, int rhs) {
            List<int> res = lhs.ToList();
            if (!res.Contains(rhs)) { res.Add(rhs); }
            return res;
        }

        static private List<int> SetPlus(List<int> lhs, List<int> rhs) {
            List<int> res = lhs.ToList();
            foreach (int i in rhs) {
                if (!res.Contains(i)) { res.Add(i); }
            }
            return res;
        }

        #endregion //Helper Functions
    }
}
