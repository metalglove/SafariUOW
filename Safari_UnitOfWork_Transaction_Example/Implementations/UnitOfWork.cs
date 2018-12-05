using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFactory<IUserRepository, SqlConnection> _userRepositoryFactory;
        private readonly IFactory<SqlConnection> _connectionFactory;

        #region Repositories
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get => _userRepository ?? 
                (_userRepository = _userRepositoryFactory.Create(_connectionFactory.Create()));
        }
        #endregion Repositories

        public UnitOfWork(
            IFactory<IUserRepository, SqlConnection> userRepositoryFactory,
            IFactory<SqlConnection> connectionFactory)
        {
            _userRepositoryFactory = userRepositoryFactory;
            _connectionFactory = connectionFactory;
        }

        public bool Commit()
        {
            _userRepository?.Commit();

            return true;
        }
        public bool Rollback()
        {
            _userRepository?.Rollback();

            return true;
        }
        public void Dispose()
        {
            _userRepository?.Dispose();
        }
    }
}
