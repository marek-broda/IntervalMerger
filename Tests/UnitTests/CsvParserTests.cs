using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using IntervalMerger.CsvParser;
using IntervalMerger.Model;

namespace IntervalMerger.Tests.UnitTests
{
    public class CsvParserTests : IDisposable
    {
        [Fact]
        public void Parses_single_row_correctly()
        {
            // arrange
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("Arrival time, Start, End, Action");
            strBuilder.AppendLine("2019-04-17 18:28:00, 10, 15, ADDED");

            var stringReader = new StringReader(strBuilder.ToString());

            // act 
            var csvParser = new CsvParserService(stringReader);
            var rows = csvParser.ReadCsv().ToList();

            // assert 
            var expectedDate = new DateTime(2019, 04, 17, 18, 28, 00);

            var expectedResult = new IntervalEntry()
            {
                Arrival = expectedDate,
                Interval = new Interval(10, 15),
                Action = IntervalAction.Added
            };

            var row = rows.Single();
            Assert.Equal(expectedResult.Arrival, actual: row.Arrival);
            Assert.Equal(expectedResult.Interval.Start, actual: row.Interval.Start);
            Assert.Equal(expectedResult.Interval.End, actual: row.Interval.End);
            Assert.Equal(expectedResult.Action, actual: row.Action);

        }

        [Fact]
        public void Parses_multiple_rows_correctly()
        {
            // arrange
            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("Arrival time, Start, End, Action");
            strBuilder.AppendLine("2019-04-17 18:28:00, 10, 15, ADDED");
            strBuilder.AppendLine("2019-04-17 18:29:00, 20, 25, REMOVED");

            var stringReader = new StringReader(strBuilder.ToString());

            // act 
            var csvParser = new CsvParserService(stringReader);
            var rows = csvParser.ReadCsv().ToList();

            // assert 
            Assert.Equal(expected: 2, actual: rows.Count());

            // first row
            var firstRow = rows.First();
            var expectedDate1 = new DateTime(2019, 04, 17, 18, 28, 00);
            Assert.Equal(expected: expectedDate1, actual: firstRow.Arrival);
            Assert.Equal(expected: 10, actual: firstRow.Interval.Start);
            Assert.Equal(expected: 15, actual: firstRow.Interval.End);
            Assert.Equal(expected: IntervalAction.Added, actual: firstRow.Action);

            var secondRow = rows.Last();
            var expectedDate2 = new DateTime(2019, 04, 17, 18, 29, 00);
            Assert.Equal(expected: expectedDate2, actual: secondRow.Arrival);
            Assert.Equal(expected: 20, actual: secondRow.Interval.Start);
            Assert.Equal(expected: 25, actual: secondRow.Interval.End);
            Assert.Equal(expected: IntervalAction.Removed, actual: secondRow.Action);
        }

        [Fact]
        public void Returns_empty_for_no_rows()
        {
            // arrange
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("Arrival time, Start, End, Action");

            var stringReader = new StringReader(strBuilder.ToString());

            // act 
            var csvParser = new CsvParserService(stringReader);
            var rows = csvParser.ReadCsv();

            // assert 
            Assert.Empty(rows);
        }

        public void Dispose()
        {

        }
    }
}