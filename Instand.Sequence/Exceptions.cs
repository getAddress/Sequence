using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence
{

    public class MaximumValueReachedException : Exception
    {
        public long MaximumValue { get; set; }

        public MaximumValueReachedException(long maximumValue)
        {
            MaximumValue = maximumValue;
        }
    }


    public class SequenceCouldNotBeFoundException : Exception
    {

    }

    public class MinimumValueReachedException : Exception
    {
        public long MinimumValue { get; set; }

        public MinimumValueReachedException(long minimumValue)
        {
            MinimumValue = minimumValue;
        }
    }

    public class MaxRetryAttemptReachedException : Exception
    {
        public int Attempts { get; set; }

        public MaxRetryAttemptReachedException(int attempts)
        {
            Attempts = attempts;
        }
    }
}
