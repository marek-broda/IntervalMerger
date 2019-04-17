using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using IntervalMerger.IntervalActions;
using IntervalMerger.Model;

namespace IntervalMerger.Tests.UnitTests
{
    public class IntervalAdderTests
    {
        private Interval _intervalToAdd { get; set; }

        public IntervalAdderTests()
        {
            _intervalToAdd = new Interval(start: 10, end: 20);
        }

        [Fact]
        public void Add_interval_to_empty_collection()
        {
            // arrange 
            var existing = new List<Interval>();

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(existing, _intervalToAdd);

            // assert 
            Assert.Single(results);

            var interval = results.Single();
            Assert.Equal(expected: 10, actual: interval.Start);
            Assert.Equal(expected: 20, actual: interval.End);
        }

        [Fact]
        public void Add_interval_to_collection_with_overlapping_lower_bound()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 3, end: 7));

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            Assert.Single(results);

            var interval = results.Single();
            Assert.Equal(expected: 3, actual: interval.Start);
            Assert.Equal(expected: 20, actual: interval.End);
        }

        [Fact]
        public void Add_interval_to_collection_with_overlapping_upper_bound()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 22, end: 30));

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            Assert.Single(results);

            var interval = results.Single();
            Assert.Equal(expected: 10, actual: interval.Start);
            Assert.Equal(expected: 30, actual: interval.End);
        }

        [Fact]
        public void Add_interval_to_collection_with_overlapping_both_bounds()
        {
            // arrange 
            var collection = new List<Interval>();
            collection.Add(new Interval(start: 3, end: 7));
            collection.Add(new Interval(start: 22, end: 30));

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            Assert.Single(results);

            var interval = results.Single();
            Assert.Equal(expected: 3, actual: interval.Start);
            Assert.Equal(expected: 30, actual: interval.End);
        }

        [Fact]
        public void Add_interval_to_collection_with_no_overlap()
        {
            // arrange 
            var collection = new List<Interval>();
            var lowerInterval = new Interval(start: 1, end: 3);
            collection.Add(lowerInterval);

            var upperInterval = new Interval(start: 27, end: 30);
            collection.Add(upperInterval);

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            // we check there are three intervals, 
            // and that they are the three referenced above 
            Assert.Equal(expected: 3, actual: results.Count());

            Assert.Contains(lowerInterval, results);
            Assert.Contains(upperInterval, results);
            Assert.Contains(_intervalToAdd, results);
        }

        [Fact]
        public void Adding_intervals_with_zero_mergeDistance()
        {
            // arrange 
            var collection = new List<Interval>();
            var lowerInterval = new Interval(start: 1, end: 10);
            collection.Add(lowerInterval);

            var upperInterval = new Interval(start: 27, end: 30);
            collection.Add(upperInterval);

            // act 
            var intervalAdder = new IntervalAdder(mergeDistance: 5);
            var results = intervalAdder.MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            // we check bottom two merge
            Assert.Equal(expected: 2, actual: results.Count());

            var resultantInterval = results.First();
            Assert.Equal(expected: 1, actual: resultantInterval.Start);
            Assert.Equal(expected: 20, actual: resultantInterval.End);

            // This one not merged
            Assert.Contains(upperInterval, results);
        }

        [Fact]
        public void Adding_intervals_with_negative_merge_distance_throws_exception()
        {
            // arrange 
            var collection = new List<Interval>();

            // act 
            Action action = () 
                => new IntervalAdder(mergeDistance: -1)
                        .MergeIntervalIn(collection, _intervalToAdd);

            // assert 
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }
    }
}