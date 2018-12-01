using Safari_UnitOfWork_Transaction_Example.Abstractions;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class RepositoryFactory<TRepository> : IFactory<TRepository, SqlConnection, IFactory<TransactionScope, IsolationLevel>> where TRepository : RepositoryBase
    {
        public TRepository Create(SqlConnection sqlConnection, IFactory<TransactionScope, IsolationLevel> transactionScopeFactory)
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), sqlConnection, transactionScopeFactory);
        }
    }
}
