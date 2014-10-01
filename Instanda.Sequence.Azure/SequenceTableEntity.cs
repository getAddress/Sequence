
using Microsoft.WindowsAzure.Storage.Table;

namespace Instanda.Sequence.Azure
{
    public class SequenceTableEntity:TableEntity,ISequence
    {
        public SequenceTableEntity()
        {
            
        }
        public SequenceTableEntity(SequenceOptions options):this()
        {
            StartAt = options.StartAt;
            CurrentValue = StartAt;
            Increment = options.Increment;
            MaxValue = options.MaxValue;
            MinValue = options.MinValue;
            Cycle = options.Cycle;
        }


        public long StartAt { get; set; }
        public int Increment { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool Cycle { get; set; }
        public long CurrentValue { get; set; }

    }


}
