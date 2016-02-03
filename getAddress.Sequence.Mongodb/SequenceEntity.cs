using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getAddress.Sequence.Mongodb
{
    public class Sequences : Entity, ISequence
    {
        public Sequences()
        {

        }
        public Sequences(SequenceOptions options)
            : this()
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
    
