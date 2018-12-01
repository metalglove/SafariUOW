using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _sqlConnection;
        private readonly TransactionScope _transactionScope;
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get => _userRepository ?? (_userRepository = new UserRepository(_sqlConnection, _transactionScope));
        }

        public UnitOfWork(SqlConnection sqlConnection, IsolationLevel isolationLevel)
        {
            _sqlConnection = sqlConnection;
            _transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                    Timeout = TransactionManager.DefaultTimeout
                });
        }

        public void Commit()
        {
            _transactionScope.Complete();
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
            _transactionScope.Dispose();
        }
    }
}
