using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class ConnectionFactory : IFactory<SqlConnection>
    {
        private readonly string connectionString = "Server = (localdb)\\mssqllocaldb; Database = SafariDb; Trusted_Connection = True; ConnectRetryCount = 0";

        public SqlConnection Create()
        {
            return new SqlConnection(connectionString);
        }
    }
}
