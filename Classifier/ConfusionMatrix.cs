using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifier {
    public class ConfusionMatrix {
        public Dictionary<string,Dictionary<string,int>> matrix;
        public List<string> values;
        public int longestValue = 0;
        public double accuracy;
        public double numCorrect;
        public double numWrong;


        //given a list of n class values build a matrix of all zeros that is n by n
        public ConfusionMatrix(List<string> values) {
            this.values = values;
            
            matrix = new Dictionary<string, Dictionary<string, int>>();
            foreach(string v in values){
                if (v.Length > longestValue) { longestValue = v.Length; }
                matrix.Add(v,new Dictionary<string,int>());
                foreach (string x in values) {
                    matrix[v].Add(x,0);
                }
            }
        }

        //Add a confusion matrix to this confusion matrix
        //they must both be built from the exact same class values
        public void Add(ConfusionMatrix rhs) {
            foreach (KeyValuePair<string, Dictionary<string, int>> row in rhs.matrix) {
                foreach (KeyValuePair<string, int> col in row.Value) {
                    matrix[row.Key][col.Key] += rhs.matrix[row.Key][col.Key];
                }
            }
            numCorrect += rhs.numCorrect;
            numWrong += rhs.numWrong;
            accuracy = numCorrect / (numCorrect + numWrong);
        }

        public override string ToString() {
            string output = "";
            string form = "{0,4}";

            output += "Accuracy: " + this.accuracy.ToString("n3") + "\n";
            output += "Correct : " + this.numCorrect.ToString("n0") + "\n";
            output += "Wrong   : " + this.numWrong.ToString("n0") + "\n\n";

            //col headers
            output += "  ";
            foreach (string v in values) { output += string.Format(form, v) + "|"; }
            output += "\n-------------------";

            //rows
            foreach(KeyValuePair<string,Dictionary<string,int>> row in matrix){
                output += "\n";
                output += row.Key;
                foreach (KeyValuePair<string, int> col in row.Value) {
                    output += "|" + string.Format(form, col.Value.ToString());
                }
                output += "|";
            }

            return output;
        }
    }
}
