using System;
using System.IO;
using System.Diagnostics;

namespace IntervalMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch(); 

            stopwatch.Start();

            var job = new IntervalMergeJob(); 
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CSVs/edge_cases.csv");
            job.ImportIntervals(path, 7);

            stopwatch.Stop();
            Console.WriteLine(string.Format("Job took {0}s", stopwatch.ElapsedMilliseconds / (decimal)1000));
        }
    }
}
