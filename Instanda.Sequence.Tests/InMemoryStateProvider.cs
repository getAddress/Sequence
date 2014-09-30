using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence.Tests
{
    internal class InMemoryStateProvider : IStateProvider
    {
        private readonly Dictionary<string, ISequence> dictionary;

        public bool UpdateValue
        {
            get;
            set;
        }

        public InMemoryStateProvider()
        {
            dictionary = new Dictionary<string, ISequence>();
            UpdateValue = true;
        }

        public Task<SequenceKey> AddAsync(ISequence sequence)
        {
            try
            {
                var key = new SequenceKey { Value = Guid.NewGuid().ToString() };

                dictionary.Add(key.Value, sequence);

                return Task.FromResult(key);
            }
            catch (Exception)
            {
                return Task.FromResult(default(SequenceKey));
            }
        }

        public Task<ISequence> GetAsync(SequenceKey sequenceKey)
        {
            try
            {
                return Task.FromResult(dictionary[sequenceKey.Value]);
            }
            catch (Exception)
            {
                return Task.FromResult(default(ISequence));
            }
        }

        public async Task<ISequence> NewAsync()
        {
            return await  Task.FromResult(new Sequence());
        }

        public Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence)
        {
            try
            {
                dictionary[sequenceKey.Value] = sequence;

                return Task.FromResult(UpdateValue);
            }
            catch (Exception)
            {
                return Task.FromResult(UpdateValue);
            }
        }
    }
}
