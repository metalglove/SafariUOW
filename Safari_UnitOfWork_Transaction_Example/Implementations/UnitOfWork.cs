using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFactory<TransactionScope, IsolationLevel> _transactionScopeFactory;
        private readonly IFactory<IUserRepository, SqlConnection, IFactory<TransactionScope, IsolationLevel>> _userRepositoryfactory;
        private readonly SqlConnection _sqlConnection;

        #region Repositories
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get => _userRepository ?? 
                (_userRepository = _userRepositoryfactory.Create(_sqlConnection, _transactionScopeFactory));
        }
        #endregion

        public UnitOfWork(
            IFactory<IUserRepository, SqlConnection, IFactory<TransactionScope, IsolationLevel>> userRepositoryfactory, 
            IFactory<TransactionScope, IsolationLevel> transactionScopeFactory, 
            SqlConnection sqlConnection)
        {
            _userRepositoryfactory = userRepositoryfactory;
            _transactionScopeFactory = transactionScopeFactory;
            _sqlConnection = sqlConnection;
        }

        public bool Commit()
        {
            if(!UserRepository.Equals(null))
                UserRepository.Commit();

            return true;
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
            if (!UserRepository.Equals(null))
                UserRepository.Dispose();
        }
    }
}
