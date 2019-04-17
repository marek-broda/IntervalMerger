using System;
using System.Linq;
using System.Collections.Generic;
using IntervalMerger.Model;

namespace IntervalMerger.IntervalActions
{
    public class IntervalAdder
    {
        private int _mergeDistance {get;set;}

        public IntervalAdder(int mergeDistance)
        {
            if (mergeDistance < 0)
                throw new ArgumentOutOfRangeException(); 
            _mergeDistance = mergeDistance;
        }

        public IEnumerable<Interval> MergeIntervalIn(
            IEnumerable<Interval> existingIntervals,
            Interval mergeInterval)
        {
            // set up the intervals to merge
            var intervals = existingIntervals.ToList();
            intervals.Add(mergeInterval);
            intervals = intervals.OrderBy(x => x.Start).ToList();

            // our starting point is this
            var possibleInterval = intervals.First();  
            foreach (var interval in intervals.Skip(1))
            {
                if (IsOverlap(possibleInterval, interval))
                {
                    possibleInterval = new Interval()
                    {
                        Start = possibleInterval.Start,
                        End = Math.Max(possibleInterval.End, interval.End)
                    };
                }
                else 
                {
                    yield return possibleInterval;
                    possibleInterval = interval;
                }
            } 

            yield return possibleInterval;
        }

        private bool IsOverlap(Interval first, Interval second)
        {
            return second.Start - first.End < _mergeDistance;
        }
    }
}