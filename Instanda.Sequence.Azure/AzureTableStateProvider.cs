using System;

using System.Threading.Tasks;

namespace Instanda.Sequence.Azure
{
    public class AzureTableStateProvider : TableEntityRepository<SequenceTableEntity>, IStateProvider
    {
        private const string PartitionKey = "Sequences";

        public AzureTableStateProvider(string connectionStr, string tableName)
            : base(connectionStr, tableName, false)
        {
            
        }

        public async Task<SequenceKey> AddAsync(Sequence sequence)
        {
            var rowKey = Guid.NewGuid().ToString();

            var sequenceEntity = SequenceTableEntity.FromSequence(sequence);

            sequenceEntity.RowKey = rowKey;
            sequenceEntity.PartitionKey = PartitionKey;

            await base.Add(sequenceEntity);

            return new SequenceKey { Value = rowKey };
        }

        public async Task<Sequence> GetAsync(SequenceKey sequenceKey)
        {
            var sequenceEntity = await base.Get(PartitionKey, sequenceKey.Value);

            return sequenceEntity.ToSequence();
        }

        public async  Task<bool> UpdateAsync(SequenceKey sequenceKey, Sequence sequence)
        {
            var sequenceEntity = SequenceTableEntity.FromSequence(sequence);

            sequenceEntity.RowKey = sequenceKey.Value;
            sequenceEntity.PartitionKey = PartitionKey;

           var updatedSequenceEntity  = await UpdateAsync(sequenceEntity);

           return updatedSequenceEntity != null;
        }
    }
}
