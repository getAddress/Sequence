using System;
using System.Threading.Tasks;
using Instanda.Sequence.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instanda.Sequence.Tests
{
    [TestClass]
    public class Tests
    {

        private IStateProvider GetStateProvider()
        {
            return StateProviderFactory.Get("DefaultEndpointsProtocol=https;AccountName=developersrv;AccountKey=vpiHFEkTPgWFjsVF9ugc92nw2BJYrPyeiyu3Z5ttO1c0iWY9czWs5mFWcfBs4lk+zBTBkcwIpG9sVWTgbg3mJw==", "SequenceTest");
            // return new InMemoryStateProvider();
        }

        private static async Task<ISequence> CreateSequence(IStateProvider stateProvider, int increment = 1, int startAt = 0, long maxValue = long.MaxValue,
            long minValue = long.MinValue,bool cycle = false )
        {
            var sequence = await stateProvider.NewAsync();

            sequence.Increment = increment;//todo: SequenceOptions in constuctor

            sequence.StartAt = startAt;
            sequence.MaxValue = maxValue;
            sequence.Cycle = cycle;
            sequence.MinValue = minValue;
            return sequence;
        }

        [TestMethod]
        public async Task NextAsyncReturnsExpectedValue()
        {

            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = await CreateSequence(stateProvider);

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

            var sequence = await CreateSequence(stateProvider, increment: -1, startAt: 5, minValue: -100);

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

            var sequence = await CreateSequence(stateProvider,increment: 0, startAt: 5);

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

            var sequence = await CreateSequence(stateProvider,increment: 10);

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

            var sequence = await CreateSequence(stateProvider, startAt: 20);

            var sequenceKey = await stateProvider.AddAsync(sequence);

            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);

            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.IsTrue(nextValue1 == 21);

            Assert.IsTrue(nextValue2 == 22);
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

            var sequence = await CreateSequence(stateProvider, maxValue: 2);

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

            var sequence = await CreateSequence(stateProvider, maxValue: 2, cycle: true);

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

            var sequence = await CreateSequence(stateProvider, minValue: 2, startAt: 4, increment: -1, cycle: true);

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

            var sequence = await CreateSequence(stateProvider,minValue: 2, startAt: 4, increment: -1, cycle: false);

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
            var stateProvider = new InMemoryStateProvider();
            
            stateProvider.UpdateValue = false;

            var sequenceGenerator = new SequenceGenerator(stateProvider);

            var sequence = await CreateSequence(stateProvider);

            var sequenceKey = await stateProvider.AddAsync(sequence);

             await sequenceGenerator.NextAsync(sequenceKey);
        }
    }
}
