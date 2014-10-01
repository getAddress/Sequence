
namespace Instanda.Sequence
{
    public class SequenceOptions
    {

        public SequenceOptions()
        {
       
            StartAt = 0;
            Increment = 1;
            MaxValue = long.MaxValue;
            MinValue = 0;
            Cycle = false;
        
        }

        public long StartAt { get; set; }
        public int Increment { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool Cycle { get; set; }
    }
}
