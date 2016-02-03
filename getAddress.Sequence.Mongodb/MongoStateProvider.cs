using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

namespace getAddress.Sequence.Mongodb
{
    public class MongoStateProvider : MongoRepository<Sequences>, IStateProvider
    {
        public MongoStateProvider(string connectionString)
            : base(connectionString)
        {

        }

        public async Task<SequenceKey> AddAsync(ISequence sequence)
        {
            //GenerateId temp = new GenerateId(1, 5, 5, 0);

            //long time = (DateTime.Now - new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Local)).Ticks / GenerateId.TICKPERSEC;

            //var rowKey = temp.GetId(time);

            var rowKey = ObjectId.GenerateNewId();
            var sequenceEntity = sequence as Sequences;
            sequenceEntity.Id = rowKey.ToString();

            base.Add(sequenceEntity);

            return new SequenceKey { Value = rowKey.ToString() };
        }

        public async Task<ISequence> GetAsync(SequenceKey sequenceKey)
        {
            var sequenceEntity = base.GetById(sequenceKey.Value);
            return sequenceEntity;
        }

        public async Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence)
        {
            var sequenceEntity = sequence as Sequences;
            sequenceEntity.Id = sequenceKey.Value;

            var query = Query.And(Query.EQ("_id", ObjectId.Parse(sequenceKey.Value)));
            var update = MongoDB.Driver.Builders.Update < Sequences>.Set( c => c.CurrentValue , sequenceEntity.CurrentValue);

            var updatedSequenceEntity = this.Collection.FindAndModify(new FindAndModifyArgs() {  Query = query, Update = update, VersionReturned = FindAndModifyDocumentVersion.Original, SortBy = null });
            var doc = updatedSequenceEntity.ModifiedDocument;
            return doc != null;
     
        }

        public async Task<ISequence> NewAsync(SequenceOptions options)
        {
            var sequence = new Sequences(options);

            return await Task.FromResult(sequence);
        }
    }
}
