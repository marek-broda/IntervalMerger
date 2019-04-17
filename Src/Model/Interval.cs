using System;

namespace IntervalMerger.Model
{
    public class Interval
    {
        public DateTime Arrival { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public IntervalAction Action { get; set; }
    }
}