using System;

using System.Data.Entity;
using System.Linq;

using System.Threading.Tasks;

namespace Instanda.Sequence.SqlServer
{
    public class SqlServerStateProvider:IStateProvider
    {

        private readonly SequenceDbContext DbContext;
        private readonly IDbSet<SqlServerSequence> DbSet;

        public SqlServerStateProvider(string nameOrConnectionStr)
        {
            DbContext = new SequenceDbContext(nameOrConnectionStr);

            DbSet = DbContext.Set<SqlServerSequence>();

        }

        public async Task<SequenceKey> AddAsync(ISequence sequence)
        {
            var sqlServerSequence = (SqlServerSequence)sequence;

            sqlServerSequence.Key = Guid.NewGuid().ToString();
            sqlServerSequence.DateCreated = DateTime.UtcNow;


            DbSet.Add(sqlServerSequence);

           await SaveChangesAsync();

           return new SequenceKey { Value = sqlServerSequence.Key.ToString() };
        }

        public async Task<ISequence> GetAsync(SequenceKey sequenceKey)
        {
            var sequence = await DbSet.FirstOrDefaultAsync(s => s.Key == sequenceKey.Value);


            return sequence;
        }


        private async Task<int> SaveChangesAsync(bool handleConcurrencyExceptions = true)
        {
            try
            {
                return await Task.FromResult(DbContext.SaveChanges());
            }
            catch (System.Data.Entity.Core.OptimisticConcurrencyException)
            {
                if (handleConcurrencyExceptions)
                {
                    return 0;
                }
                throw;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                if (handleConcurrencyExceptions)
                {
                    return 0;
                }
                throw;
            }

        }

        public async Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence)
        {
            var updateResult = await SaveChangesAsync();

            if (updateResult != 1)
            {
                Reload(sequence);

                return false;
            }
            return true;
        }

        private void Reload(ISequence sequence)
        {
            DbContext.Entry(sequence).Reload();
        }

        public async Task<ISequence> NewAsync(SequenceOptions options)
        {
            return await Task.FromResult(new SqlServerSequence(options));
        }
    }
}
