using System; 
using System.Linq;
using System.Collections.Generic; 
using IntervalMerger.Model;

namespace IntervalMerger.IntervalActions
{
    public class IntervalDeleter
    {   
        public IEnumerable<Interval> DeleteIntervalFrom(
            IEnumerable<Interval> existingIntervals,
            Interval deleteInterval
        )
        {
            // set up the intervals to merge
            var intervals = existingIntervals.ToList();
            
            // loop through and cut out as necesary 
            foreach (var interval in intervals)
            {
                bool completelyEnclosesDelete = 
                    interval.Start < deleteInterval.Start
                 && deleteInterval.End < interval.End; 
                
                if (completelyEnclosesDelete)
                {
                    yield return new Interval(interval.Start, deleteInterval.Start); 
                    yield return new Interval(deleteInterval.End, interval.End);
                    
                    continue; 
                }

                bool deleteOverlapsStart = 
                    interval.Start < deleteInterval.End
                 && deleteInterval.End < interval.End; 
                if (deleteOverlapsStart)
                {
                    yield return new Interval(deleteInterval.End, interval.End);
                    continue; 
                }

                bool deleteOverlapsEnd = 
                    deleteInterval.Start < interval.End
                 && interval.End < deleteInterval.End; 
                if (deleteOverlapsEnd)
                {
                    yield return new Interval(interval.Start, deleteInterval.Start);
                    continue;
                } 

                yield return interval;
            }
        }
    }
}