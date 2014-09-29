using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence
{
    public class SequenceGenerator
    {
        public IStateProvider StateProvider { get; private set; }
        public int MaxNumberOfAttempts = 100;


        public SequenceGenerator(IStateProvider stateProvider)
        {
            if (stateProvider == null) throw new ArgumentNullException("stateProvider");

            StateProvider = stateProvider;
        }

        /// <exception cref="SequenceCouldNotBeFoundException">Sequence could not be found</exception>
        /// <exception cref="MaximumValueReachedException">The maximum sequence value has been reached and cycle is false</exception>
        /// <exception cref="MinimumValueReachedException">The minimum sequence value has been reached and cycle is false</exception>
        /// <exception cref="MaxRetryAttemptReachedException">The maximum number of retries has been reached</exception>
        public async Task<long> NextAsync(SequenceKey sequenceKey)
        {
            if (sequenceKey == null) throw new ArgumentNullException("sequenceKey");
            if (sequenceKey.Value == null) throw new ArgumentNullException("sequenceKey");

            return await ExecAsync(sequenceKey, 0);
            
        }

        private async Task<long> ExecAsync(SequenceKey sequenceKey, int retryAttempt)
        {
            var result = await TryGetSequenceValue(sequenceKey);

            if (!result.Result)
            {
                if (retryAttempt < MaxNumberOfAttempts)
                {
                    retryAttempt++;

                    result.Value = await ExecAsync(sequenceKey, retryAttempt);
                }
                else
                {
                    throw new MaxRetryAttemptReachedException(MaxNumberOfAttempts);
                }
            }

            return result.Value;
        }


        private async Task<TryGetSequenceValueResult> TryGetSequenceValue(SequenceKey sequenceKey)
        {
            var sequence = await StateProvider.GetAsync(sequenceKey);

            if (sequence == null)
            {
                throw new SequenceCouldNotBeFoundException();
            }

            var result = new TryGetSequenceValueResult
            {
                Value = sequence.StartAt
            };


            var sequenceValue = sequence.CurrentValue  + sequence.Increment;

            sequenceValue = CycleOrFailIfGreaterThanMaximum(sequence, sequenceValue);

            sequenceValue = CycleOrFailIfLessThanMinimum(sequence, sequenceValue);

            sequence.CurrentValue = sequenceValue;

            var updateResult = await StateProvider.UpdateAsync(sequenceKey, sequence);

            if (!updateResult)
            {
                return result;
            }

            result.Value += sequence.CurrentValue;

            result.Result = true;

            return result;

        }

       

        private class TryGetSequenceValueResult
        {
            public bool Result { get; set; }
            public long Value { get; set; }
        }

        private static long CycleOrFailIfGreaterThanMaximum(Sequence sequence, long newValue)
        {
            if ((newValue + sequence.Increment) > sequence.MaxValue)
            {
                if (sequence.Cycle)
                {
                    return sequence.StartAt;
                }

                throw new MaximumValueReachedException(sequence.MaxValue);

            }
            return newValue;
        }

        private static long CycleOrFailIfLessThanMinimum(Sequence sequence, long newValue)
        {
            if ((newValue + sequence.Increment) < sequence.MinValue)
            {
                if (sequence.Cycle)
                {
                    return sequence.StartAt;

                }

                throw new MinimumValueReachedException(sequence.MinValue);

            }

            return newValue;
        }



    }


}
