using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using IntervalMerger.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace IntervalMerger.CsvParser
{
    public class CsvParserService
    {
        public IEnumerable<Interval> ReadCsv(TextReader textReader)
        {
            var list = new List<Interval>();

            using (var csv = new CsvReader(textReader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower().Trim();
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<CsvRowToIntervalMap>();

                list = csv.GetRecords<Interval>().ToList();
            }

            return list; 
        }
    }
}