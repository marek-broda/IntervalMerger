using System; 
using CsvHelper.Configuration;
using IntervalMerger.Model;

public class CsvRowToIntervalMap : ClassMap<IntervalEntry>
{
    public CsvRowToIntervalMap()
    {
        Map(m => m.Arrival).Name("arrival time");
        Map(m => m.Interval.Start).Name("start");
        Map(m => m.Interval.End).Name("end");
        Map(m => m.Action).Name("action");
    }
}