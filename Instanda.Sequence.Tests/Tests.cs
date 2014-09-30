using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instanda.Sequence.Tests
{
    [TestClass]
    public class Tests
    {

        private IStateProvider GetStateProvider()
        {
            return new InMemoryStateProvider();
        }

        private static Sequence CreateSequence(int increment = 1, int startAt = 0, long maxValue = long.MaxValue,
            long minValue = long.MinValue,bool cycle = false )
        {
            return new Sequence {
             Increment=increment,
             StartAt = startAt,
             MaxValue = maxValue,
             Cycle = cycle,
             MinValue = minValue
            };
        }

        [TestMethod]
        public async Task NextAsyncReturnsExpectedValue()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence();

            var sequenceKey = await  stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 1);

            Assert.IsTrue(nextValue2 == 2);
        }

        [TestMethod]
        public async Task NextAsyncReturnsExpectedValueForNegativeIncrement()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(increment:-1, startAt:5, minValue:-100);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 4);

            Assert.IsTrue(nextValue2 == 3);
        }

        [TestMethod]
        public async Task NextAsyncReturnsExpectedValueForZeroIncrement()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(increment: 0, startAt: 5);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 5);

            Assert.IsTrue(nextValue2 == 5);
        }

        [TestMethod]
        public async Task NextAsyncReturnsExpectedIncrement()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(increment:10);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 10);

            Assert.IsTrue(nextValue2 == 20);
        }

        [TestMethod]
        public async Task NextAsyncStartsAtExpectedValue()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(startAt: 20);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 21);

            Assert.IsTrue(nextValue2 == 22);
        }


        [TestMethod]
        public async Task SequenceSetsDefaultValues()
        {

            var sequence = new Sequence();


            Assert.IsTrue(sequence.CurrentValue == 0);

            Assert.IsFalse(sequence.Cycle);

            Assert.IsTrue(sequence.Increment  == 1);

            Assert.IsTrue(sequence.MaxValue == long.MaxValue);

            Assert.IsTrue(sequence.MinValue == 0);

            Assert.IsTrue(sequence.StartAt == 0);

           
        }

        [ExpectedException(typeof(SequenceCouldNotBeFoundException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionIfSequencyCanNotBeFound()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);


            var nextValue1 = await sequenceGenerator.NextAsync(new SequenceKey { Value = "1234"});
        }

        [ExpectedException(typeof(MaximumValueReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenMaximumValueIsReached()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(maxValue:2);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 1);

            Assert.IsTrue(nextValue2 == 2);

            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);

           
        }

        [TestMethod]
        public async Task NextMethodCyclesWhenMaximumValueIsReached()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(maxValue: 2, cycle:true);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 1);

            Assert.IsTrue(nextValue2 == 2);

            Assert.IsTrue(nextValue3 == 1);
        }


        [TestMethod]
        public async Task NextMethodCyclesWhenMinimumValueIsReached()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(minValue: 2, startAt:4,increment:-1, cycle: true);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 3);

            Assert.IsTrue(nextValue2 == 2);

            Assert.IsTrue(nextValue3 == 3);
        }

        [ExpectedException(typeof(MinimumValueReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenMinimumValueIsReached()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence(minValue: 2, startAt: 4, increment: -1, cycle: false);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 3);

            Assert.IsTrue(nextValue2 == 2);

           
        }


       
        [ExpectedException(typeof(MaxRetryAttemptReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenIfMaxRetryAttemptIsReach()
        {
            var stateProvider = GetStateProvider();

            ((InMemoryStateProvider)stateProvider).UpdateValue = false;

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = CreateSequence();

            var sequenceKey = await stateProvider.AddAsync(sequence);

             await sequenceGenerator.NextAsync(sequenceKey);
        }
    }
}
