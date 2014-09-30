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

        public bool UpdateValue
        {
            get;
            set;
        }

        public InMemoryStateProvider()
        {
            dictionary = new Dictionary<string, Sequence>();
            UpdateValue = true;
        }

        public Task<SequenceKey> AddAsync(Sequence sequence)
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

        public Task<Sequence> GetAsync(SequenceKey sequenceKey)
        {
            try
            {
                return Task.FromResult(dictionary[sequenceKey.Value]);
            }
            catch (Exception)
            {
                return Task.FromResult(default(Sequence));
            }
        }

        public Task<bool> UpdateAsync(SequenceKey sequenceKey, Sequence sequence)
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
