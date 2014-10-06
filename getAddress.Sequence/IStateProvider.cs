
using System.Threading.Tasks;


namespace getAddress.Sequence
{
    public interface IStateProvider
    {

        Task<SequenceKey> AddAsync(ISequence sequence);

        Task<ISequence> GetAsync(SequenceKey sequenceKey);

        Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence);

        Task<ISequence> NewAsync(SequenceOptions options);
    }
}
