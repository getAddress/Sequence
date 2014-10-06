
using System.Data.Entity;


namespace getAddress.Sequence.SqlServer
{
   
    internal class SequenceDbContext : DbContext
    {
        public SequenceDbContext(string nameOrConnectionStr)
            : base(nameOrConnectionStr)
        {

        }


        public DbSet<SqlServerSequence> Sequences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            modelBuilder.Configurations.Add(new SequenceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
