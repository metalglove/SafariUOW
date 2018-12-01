using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string connectionString = "Server = (localdb)\\mssqllocaldb; Database = Safari; Trusted_Connection = True; ConnectRetryCount = 0";

        public IUnitOfWork Create(IsolationLevel isolationLevel)
        {
            return new UnitOfWork(new SqlConnection(connectionString), isolationLevel);
        }
    }
}
