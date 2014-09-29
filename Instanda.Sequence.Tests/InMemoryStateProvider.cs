using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence.Tests
{
    class InMemoryStateProvider : IStateProvider
    {
        private readonly Dictionary<string, Sequence> dictionary;


        public InMemoryStateProvider()
        {
            dictionary = new Dictionary<string, Sequence>();
        }

        public Task<SequenceKey> AddAsync(Sequence sequence)
        {
            var key = new SequenceKey {Value = Guid.NewGuid().ToString()};

            dictionary.Add(key.Value, sequence);

            return Task.FromResult(key);
        }

        public Task<Sequence> GetAsync(SequenceKey sequenceKey)
        {
            return Task.FromResult(dictionary[sequenceKey.Value]);
        }

        public Task<bool> UpdateAsync(SequenceKey sequenceKey, Sequence sequence)
        {
            dictionary[sequenceKey.Value] = sequence;

            return Task.FromResult(true);
        }
    }
}
