using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using IntervalMerger.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace IntervalMerger.CsvParser
{
    public class CsvParserService : IDisposable
    {
        private CsvReader _csvReader { get; set; }

        public CsvParserService(TextReader textReader)
        {
            _csvReader = new CsvReader(textReader);

            _csvReader.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower().Trim();
            _csvReader.Configuration.HasHeaderRecord = true;
            _csvReader.Configuration.RegisterClassMap<CsvRowToIntervalMap>();
        }

        public IEnumerable<IntervalEntry> ReadCsv()
        {
            return _csvReader.GetRecords<IntervalEntry>();
        }

        public void Dispose()
        {
            _csvReader.Dispose();
        }
    }
}