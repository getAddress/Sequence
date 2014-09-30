
using Microsoft.WindowsAzure.Storage.Table;

namespace Instanda.Sequence.Azure
{
    public class SequenceTableEntity:TableEntity,ISequence
    {

      

        public long StartAt { get; set; }
        public int Increment { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool Cycle { get; set; }

        public long CurrentValue { get; set; }


        

       
    }

    

}
