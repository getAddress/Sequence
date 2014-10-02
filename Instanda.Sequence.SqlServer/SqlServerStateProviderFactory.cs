

namespace Instanda.Sequence.SqlServer
{
    public class SqlServerStateProviderFactory
    {

        public static IStateProvider Get(string nameOrConnectionStr)
        {
            return new SqlServerStateProvider(nameOrConnectionStr);
        }
    }
}
