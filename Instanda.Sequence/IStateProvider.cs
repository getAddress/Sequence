using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence
{
    public interface IStateProvider
    {

        Task<SequenceKey> AddAsync(Sequence sequence);

        Task<Sequence> GetAsync(SequenceKey sequenceKey);

        Task<bool> UpdateAsync(SequenceKey sequenceKey, Sequence sequence);
    }
}
