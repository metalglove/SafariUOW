using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFactory<IUserRepository, SqlConnection> _userRepositoryfactory;
        private readonly IFactory<SqlConnection> _connectionFactory;

        #region Repositories
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get => _userRepository ?? 
                (_userRepository = _userRepositoryfactory.Create(_connectionFactory.Create()));
        }
        #endregion Repositories

        public UnitOfWork(
            IFactory<IUserRepository, SqlConnection> userRepositoryfactory,
            IFactory<SqlConnection> connectionFactory)
        {
            _userRepositoryfactory = userRepositoryfactory;
            _connectionFactory = connectionFactory;
        }

        public bool Commit()
        {
            if(_userRepository != null)
                _userRepository.Commit();

            return true;
        }
        public bool Rollback()
        {
            if (_userRepository != null)
                _userRepository.Rollback();

            return true;
        }
        public void Dispose()
        {
            if (_userRepository != null)
                _userRepository.Dispose();
        }
    }
}
