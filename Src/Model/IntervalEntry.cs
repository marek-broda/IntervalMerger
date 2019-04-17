using System;

namespace IntervalMerger.Model
{
    public class IntervalEntry
    {
        public IntervalEntry()
        {
            Interval = new Interval();
        }
        
        public IntervalEntry(int start, int end)
        {
            Interval = new Interval()
            {
                Start = start,
                End = end
            };
        }

        public DateTime Arrival { get; set; }
        public Interval Interval { get; set; }
        public IntervalAction Action { get; set; }

        public bool HasSameBoundsAs(IntervalEntry otherInterval)
        {
            return this.Interval.Start == otherInterval.Interval.Start
                && this.Interval.End == otherInterval.Interval.End;
        }
    }
}