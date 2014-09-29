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

        private Sequence CreateSequence(int increment = 1, int startAt = 0)
        {
            return new Sequence {
             Increment=increment,
             StartAt = startAt
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
            throw new NotImplementedException();
        }

        [ExpectedException(typeof(MaximumValueReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenMaximumValueIsReached()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task NextMethodCyclesWhenMaximumValueIsReached()
        {
            throw new NotImplementedException();
        }


        [TestMethod]
        public async Task NextMethodCyclesWhenMinimumValueIsReached()
        {
            throw new NotImplementedException();
        }

        [ExpectedException(typeof(MinimumValueReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenMinimumValueIsReached()
        {
            throw new NotImplementedException();
        }


       
        [ExpectedException(typeof(MaxRetryAttemptReachedException))]
        [TestMethod]
        public async Task NextMethodThrowsExceptionWhenIfMaxRetryAttemptIsReach()
        {
            throw new NotImplementedException();
        }
    }
}
