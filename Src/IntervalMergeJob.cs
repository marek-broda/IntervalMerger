using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using IntervalMerger.Model;
using IntervalMerger.CsvParser;
using IntervalMerger.IntervalActions;

namespace IntervalMerger
{
    public class IntervalMergeJob
    {
        private IntervalAdder _intervalAdder { get; set; }
        private IntervalDeleter _intervalDeleter { get; set; }

        /* 
            Re these intervals. We can assume that they come 
            in Arrival desc order.

            With each parsed row, we will add to the stack, the interval,
            and the result of it's merge.

            We use stack in event of removal, we cycle back through added 
            to get last added instance of the interval. We remove that, 
            and then go again from there. 
            */
        // Item1 : Interval added
        // Item2 : Result of merge
        private Stack<Tuple<IntervalEntry, IEnumerable<Interval>>> _intervalsStack { get; set; }

        public IntervalMergeJob()
        {
            _intervalsStack = new Stack<Tuple<IntervalEntry, IEnumerable<Interval>>>();
            _intervalDeleter = new IntervalDeleter();
        }

        /// <summary>
        /// Reads the items in the given file, parses them to IntervalEntries
        /// and then processes each intervalentry row by row.
        /// </summary>
        /// <param name="filePath">
        /// The path to the CSV containing the interval entries 
        /// </param>
        /// <param name="mergeDistance">
        /// The distance allowed between two intervals for them to be
        /// considered viable for a merge.
        /// </param>
        public void ImportIntervals(string filePath, int mergeDistance)
        {
            _intervalAdder = new IntervalAdder(mergeDistance);

            // read the file
            var streamReader = new StreamReader(File.OpenRead(filePath));

            var csvParser = new CsvParserService(streamReader);

            // loop through entries 
            foreach (var entry in csvParser.ReadCsv())
            {
                IEnumerable<Interval> newResult = null;

                if (entry.Action == IntervalAction.Added)
                {
                    newResult = AddEntryToIntervalsStack(entry);
                }
                else if (entry.Action == IntervalAction.Removed)
                {
                    // we need to loop back through the stack to the last instance 
                    // of the interval - then we need to recalculate. 
                    var recalculationStack = GetRecalculationStack(entry);

                    // and now we recalculate 
                    foreach (var recalculationInterval in recalculationStack)
                    {
                        newResult = AddEntryToIntervalsStack(recalculationInterval);
                    }
                }
                else
                {
                    var existingIntervals = GetCurrentIntervals();
                    newResult = _intervalDeleter.DeleteIntervalFrom(existingIntervals, entry.Interval);

                    PushLatestResultsToStack(entry, newResult);
                }

                // Write the result to the console.
                Console.WriteLine(string.Join(" ", newResult.Select(i => i.ToString())));
            }
        }

        private IEnumerable<Interval> AddEntryToIntervalsStack(IntervalEntry entry)
        {
            // Get the existing interval merge, or create one if 
            // there isn't one yet.
            var existingIntervals = GetCurrentIntervals();

            // Merge the new entry in
            var newResult = _intervalAdder.MergeIntervalIn(existingIntervals, entry.Interval);

            // push the result to the stack
            PushLatestResultsToStack(entry, newResult);

            return newResult;
        }

        private Stack<IntervalEntry> GetRecalculationStack(IntervalEntry entry)
        {
            var recalculationStack = new Stack<IntervalEntry>();

            var lastInterval = _intervalsStack.Peek().Item1;
            while (!lastInterval.HasSameBoundsAs(entry))
            {
                // add the interval to the recalculation stack
                recalculationStack.Push(lastInterval);

                // get rid of it from the intervals stack 
                _intervalsStack.Pop();

                // Check the next one
                if (_intervalsStack.Any())
                {
                    lastInterval = _intervalsStack.Peek().Item1;
                }
                else
                {
                    Console.WriteLine("The item you are trying to remove is not there.");
                    break;
                }
            }

            // Get rid of this last one from the intervals stack
            // as this is the one we're removing
            // this is the one we are removing
            if (_intervalsStack.Any())
            {
                _intervalsStack.Pop();
            }

            return recalculationStack;
        }

        private IEnumerable<Interval> GetCurrentIntervals()
        {
            return _intervalsStack.Any()
                ? _intervalsStack.Peek().Item2
                : new List<Interval>();
        }

        private void PushLatestResultsToStack(IntervalEntry entry, IEnumerable<Interval> newResult)
        {
            // push the result to the stack
            var newStackEntry = new Tuple<IntervalEntry, IEnumerable<Interval>>(entry, newResult);
            _intervalsStack.Push(newStackEntry);
        }
    }
}