using System; 
using System.Collections.Generic;
using Xunit;
using IntervalMerger.Model; 
using IntervalMerger.IntervalActions;

namespace IntervalMerger.Tests.UnitTests
{
    public class DeleteTests
    {
        [Fact]
        public void Remove_interval_within_interval_splits_them()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 3, end: 7));

            var intervalToRemove = new Interval(start : 4, end : 6);

            // act 
            var intervalAdder = new IntervalDeleter();
            var results = intervalAdder.DeleteIntervalFrom(collection, intervalToRemove);

            // assert 
            var expectedResults = new List<Interval>() { 
                new Interval(start: 3, end: 4),
                new Interval(start: 6, end: 7)
            };

            Assert.Equal(expectedResults, actual: results);
        }

        [Fact]
        public void Remove_interval_within_two_intervals_shortens_both()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 3, end: 7));
            collection.Add(new Interval(start: 15, end: 20));

            var intervalToRemove = new Interval(start : 6, end : 17);

            // act 
            var intervalAdder = new IntervalDeleter();
            var results = intervalAdder.DeleteIntervalFrom(collection, intervalToRemove);

            // assert 
            var expectedResults = new List<Interval>() { 
                new Interval(start: 3, end: 6),
                new Interval(start: 17, end: 20)
            };

            Assert.Equal(expectedResults, actual: results);
        }

        [Fact]
        public void Remove_interval_not_within_interval_doesnt_change_it()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 3, end: 7));

            var intervalToRemove = new Interval(start : 8, end : 10);

            // act 
            var intervalAdder = new IntervalDeleter();
            var results = intervalAdder.DeleteIntervalFrom(collection, intervalToRemove);

            // assert 
            var expectedResults = new List<Interval>() { 
                new Interval(start: 3, end: 7)
            };

            Assert.Equal(expectedResults, actual: results);
        }
    }
}