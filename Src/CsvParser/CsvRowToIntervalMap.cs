using System; 
using CsvHelper.Configuration;
using IntervalMerger.Model;

public class CsvRowToIntervalMap : ClassMap<Interval>
{
    public CsvRowToIntervalMap()
    {
        Map(m => m.Arrival).Name("arrival time");
        Map(m => m.Start).Name("start");
        Map(m => m.End).Name("end");
        Map(m => m.Action).Name("action");
    }
}