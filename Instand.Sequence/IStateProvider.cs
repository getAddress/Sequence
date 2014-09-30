
using System.Threading.Tasks;

namespace Instanda.Sequence
{
    public interface IStateProvider
    {

        Task<SequenceKey> AddAsync(ISequence sequence);

        Task<ISequence> GetAsync(SequenceKey sequenceKey);

        Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence);

        Task<ISequence> NewAsync();
    }
}
