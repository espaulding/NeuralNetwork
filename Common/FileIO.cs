/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace Classifier {
    static class FileIO {

        //---------------------------------LoadCSV-----------------------------------------
        //Description: Load a comma separated file and turn it into a dataset.
        //-------------------------------------------------------------------------------
        static public DataSet LoadCSV(string filename, out string error) {
            error = "";
            string line;
            DataSet ds = new DataSet();
            DataTable t = ds.Tables.Add();

            try {
                using (StreamReader sr = new StreamReader(filename)) {
                    //assume that the first line of the file contains the column headers
                    line = sr.ReadLine();
                    string[] headers = line.Split(',');
                    foreach (string colHeader in headers) {
                        t.Columns.Add(colHeader, typeof(string));
                    }

                    while ((line = sr.ReadLine()) != null) {
                        t.Rows.Add(line.Split(','));
                    }
                }
            } catch (FileNotFoundException) {
                error = "the file was not found.";
                ds.Dispose();
            } catch (Exception) {
                error = "the file, " + filename + " was unreadable.";
                ds.Dispose();
            }

            return ds;
        }

        //---------------------------------DataSetToString-----------------------------------------
        //Description: turn a DataSet object into a neatly formated human readable string.
        //-------------------------------------------------------------------------------
        static public string DataSetToString(DataSet ds) {
            //format the dataset into a human readable report
            string sReport = "";
            string sCell = "";
            List<int> arrTabs = new List<int>();
            int iTab = 1;
            string sDelimChar = "";

            try {
                //get the field caption widths
                for (int c = 0; c < (ds.Tables[0].Columns.Count); c++) {
                    sCell = ds.Tables[0].Columns[c].Caption;
                    arrTabs.Add(1);
                }

                //get widest field in each column
                for (int r = 0; r < (ds.Tables[0].Rows.Count); r++) {
                    for (int c = 0; c < (ds.Tables[0].Columns.Count); c++) {
                        sCell = ds.Tables[0].Rows[r][c].ToString();
                        if (sCell.Length == 0 || sCell.ToLower().Equals("null")) {
                            sCell = "";
                        }
                        //if this field is longer than any previous field set it as the new column width
                        if (sCell.Length >= arrTabs[c]) {
                            arrTabs[c] = sCell.Length + 1;
                        }
                    }
                }

                //outloop to go through each row of the dataset
                for (int r = 0; r < (ds.Tables[0].Rows.Count); r++) {
                    sReport += System.Environment.NewLine;
                    //innerloop to go through each field on a given row
                    for (int c = 0; c < (ds.Tables[0].Columns.Count); c++) {
                        iTab = arrTabs[c];
                        sCell = ds.Tables[0].Rows[r][c].ToString();
                        if (sCell.Length == 0) {
                            sCell = "";
                        }
                        if (iTab >= sCell.Length) {
                            sReport += sCell + Tab(iTab - sCell.Length);
                        } else {
                            sReport += sCell.Substring(0, iTab - 1);
                        }
                        if (c < ds.Tables[0].Columns.Count - 1) {
                            sReport += sDelimChar;
                        }
                    }
                }
            } catch (Exception) {
            }

            return sReport;
        }

        static private string Tab(int n) {
            string sTab = "                                                                ";
            return sTab.Substring(0, n);
        }
    }
}
