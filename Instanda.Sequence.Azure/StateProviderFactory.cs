

namespace Instanda.Sequence.Azure
{
    public class AzureStateProviderFactory
    {
        public static IStateProvider Get(string connectionStr, string tableName)
        {
            return new AzureTableStateProvider(connectionStr,tableName);
      
        }

    }
}
