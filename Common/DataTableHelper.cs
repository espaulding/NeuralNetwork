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
    public static class DataTableHelper {

        //gather a list of the possible values for each dimension
        static public List<List<string>> GetDistinctValues(DataTable t, string classLabel, ref int classIndex) {
            List<List<string>> allDims = new List<List<string>>(t.Columns.Count);
            int x = 0;
            for (int c = 0; c < t.Columns.Count; c++) {
                string colName = t.Columns[c].ColumnName;
                if (!colName.Equals(classLabel)) {
                    List<string> distinctValues = (from r in t.AsEnumerable() select r[colName]).Distinct().OfType<string>().ToList();
                    allDims.Add(distinctValues);
                    x++;
                } else {
                    classIndex = c;
                }
            }
            if (classIndex == -1) { throw new Exception("Class label was not found in the data"); }
            return allDims;
        }

        static public List<string> GetClassValues(DataTable t, string classLabel) {
            List<string> allClass = null;
            int x = 0;
            for (int c = 0; c < t.Columns.Count; c++) {
                string colName = t.Columns[c].ColumnName;
                if (colName.Equals(classLabel)) {
                    allClass = (from r in t.AsEnumerable() select r[colName]).Distinct().OfType<string>().ToList();
                    x++;
                } 
            }
            return allClass;
        }

        //get the crossproduct of the distinct values of the dimensions listed in dims from the datatable
        static public List<List<string>> InstantiationList(List<int> dims, List<List<string>> allValues) {
            List<List<string>> sequences = new List<List<string>>();
            List<List<string>> insList = new List<List<string>>();

            foreach (int d in dims) { sequences.Add(allValues[d]); }
            var something = CollectionExtensions.CartesianProduct(sequences);

            foreach (var line in something) {
                List<string> l = new List<string>();
                foreach (var s in line) {
                    l.Add(s);
                }
                insList.Add(l);
            }

            return insList;
        }

        //basically return a subset of the datatable with only the rows that match
        static public DataTable MatchRows(List<string> values, List<int> dims, DataTable t) {
            DataTable copyTable = t.Copy();
            for (int row = copyTable.Rows.Count - 1; row >= 0; row--) {
                bool keep = true;
                //check to see if this row matches the current parental instantiation
                for (int dim = 0; dim < dims.Count; dim++) {
                    keep &= values[dim].Equals(copyTable.Rows[row][dims[dim]]);
                }
                if (!keep) { copyTable.Rows.RemoveAt(row); }
            }
            return copyTable;
        }
    }
}
