using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Abstractions
{
    public abstract class RepositoryBase : IUnitOfWorkRepository
    {
        protected readonly SqlConnection _sqlConnection;

        protected RepositoryBase(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public abstract bool Commit();
    }
}
