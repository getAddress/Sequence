using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Instanda.Sequence.Azure
{
    public class SequenceTableEntity:TableEntity
    {

      

        public long StartAt { get; set; }
        public int Increment { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool Cycle { get; set; }

        public long CurrentValue { get; set; }


        public  Sequence ToSequence()
        {
            return new Sequence { 
             CurrentValue = CurrentValue,
              Cycle = Cycle,
               Increment = Increment,
                MaxValue = MaxValue,
                 MinValue = MinValue,
                  StartAt = StartAt
            };
        }

        public static SequenceTableEntity FromSequence( Sequence sequence)
        {

            return new SequenceTableEntity
            {
                CurrentValue = sequence.CurrentValue,
                Cycle = sequence.Cycle,
                Increment = sequence.Increment,
                MaxValue = sequence.MaxValue,
                MinValue = sequence.MinValue,
                StartAt = sequence.StartAt
            };
        }
    }

    

}
