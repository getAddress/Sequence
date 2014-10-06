
using System.Data.Entity.ModelConfiguration;


namespace getAddress.Sequence.SqlServer
{
    internal class SequenceConfiguration : EntityTypeConfiguration<SqlServerSequence>
    {
        public SequenceConfiguration()
        {
            
            Property(c => c.RowVersion).IsRowVersion();
            HasKey(s => s.Key);
        }
    }
}
