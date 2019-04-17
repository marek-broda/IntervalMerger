using System;
using System.IO;

namespace IntervalMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            var job = new IntervalMergeJob(); 
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CSVs/basic_test.csv");
            job.ImportIntervals(path, 7);
        }
    }
}
