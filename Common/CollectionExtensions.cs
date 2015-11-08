/*
 * Eric Spaulding, Graduate Student
 * Professor Doug Raiford
 * Machine Learning - Spring 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Classifier {
    public static class CollectionExtensions {
        private static Random random = new Random();

        public static void Split<T>(this IEnumerable<T> t, int start, int stop, ref IEnumerable<T> testData, ref IEnumerable<T> trainData) {
            List<T> data = new List<T>(t);
            List<T> test = new List<T>();
            List<T> train = new List<T>();
            for (int c = 0; c < data.Count; c++) {
                if (c >= start && c <= stop) { //put this into the test set
                    test.Add(data[c]);
                } else { //put this into the training set
                    train.Add(data[c]);
                }
            }
            testData = test;
            trainData = train;
        }

        //Code found at http://social.msdn.microsoft.com/Forums/en-US/b832b9ff-5e1a-490f-bcf6-3e72070b5879/shuffle-datatable-rows
        public static IEnumerable<T> OrderRandomly<T>(this IEnumerable<T> collection) {
            // Order items randomly
            List<T> randomly = new List<T>(collection);
            while (randomly.Count > 0) {
                Int32 index = random.Next(randomly.Count);
                yield return randomly[index];
                randomly.RemoveAt(index);
            }
        } // OrderRandomly

        //Code found at http://stackoverflow.com/questions/3093622/generating-all-possible-combinations
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences) {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
                );
        }
    }
}
