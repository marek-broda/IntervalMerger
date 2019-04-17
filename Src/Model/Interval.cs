using System;

namespace IntervalMerger.Model
{
    public class Interval 
    {
        public Interval() { }
        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; set; }
        public int End { get; set; }

        public override string ToString()
        {
            return string.Format("({0} {1})", Start, End);
        }
    }
}