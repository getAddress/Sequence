using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getAddress.Sequence.Mongodb
{
    public class MongoStateProviderFactory
    {
        public static IStateProvider Get(string nameOrConnectionStr)
        {
            return new MongoStateProvider(nameOrConnectionStr);
        }
    }
}
