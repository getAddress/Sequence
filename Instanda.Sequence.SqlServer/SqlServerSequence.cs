using System;


namespace Instanda.Sequence.SqlServer
{
    internal class SqlServerSequence : ISequence
    {

        public SqlServerSequence()
        {
            
        }
        public SqlServerSequence(SequenceOptions options):this()
        {
            StartAt = options.StartAt;
            CurrentValue = StartAt;
            Increment = options.Increment;
            MaxValue = options.MaxValue;
            MinValue = options.MinValue;
            Cycle = options.Cycle;

        }

        public String Key { get; set; }
        public long StartAt { get;  set; }
        public int Increment { get;  set; }
        public long MaxValue { get;  set; }
        public long MinValue { get;  set; }
        public bool Cycle { get;  set; }
        public long CurrentValue { get; set; }


        public byte[] RowVersion { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
