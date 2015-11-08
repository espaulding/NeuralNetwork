/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System.Collections.Generic;
using System.Data;

namespace Classifier {
    public class Sample {
        public List<string> data = new List<string>();
        public string classLabel;

        public Sample(DataRow row, int classIndex) {
            Add(row,classIndex);
        }

        private void Add(DataRow row, int classIndex) {
            for (int c = 0; c < row.ItemArray.Length; c++) {
                if (c != classIndex) {
                    data.Add(row.ItemArray[c].ToString());
                } else {
                    classLabel = row.ItemArray[c].ToString();
                }
            }
        }
    }
}
