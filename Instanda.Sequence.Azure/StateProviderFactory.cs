using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instanda.Sequence.Azure
{
    public class StateProviderFactory
    {
        public static IStateProvider Get(string connectionStr, string tableName)
        {
            return new AzureTableStateProvider(connectionStr,tableName);
      
        }

    }
}
