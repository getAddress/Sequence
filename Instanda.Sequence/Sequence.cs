using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence
{
    public class Sequence
    {
        public Sequence()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            StartAt = 0;
            Increment = 1;
            MaxValue = long.MaxValue;
            MinValue = 0;
            Cycle = false;
            CurrentValue = 0;
           
        }


        private long startAt;
        public long StartAt {
            get { return startAt; }
            set 
            { 
            startAt = value;
            CurrentValue = value;
            }
        }
        public int Increment { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public bool Cycle { get; set; }
        public long CurrentValue { get; set; }   
    }

    public class SequenceKey
    {
        public SequenceKey()
        {
            
        }
        public string Value { get; set; }
    }

}
