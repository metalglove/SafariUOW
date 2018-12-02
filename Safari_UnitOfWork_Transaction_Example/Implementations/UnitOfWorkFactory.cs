using Safari_UnitOfWork_Transaction_Example.Interfaces;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UnitOfWorkFactory : IFactory<IUnitOfWork>
    {
        public IUnitOfWork Create()
        {
            return new UnitOfWork(
                new RepositoryFactory<UserRepository>(),
                new ConnectionFactory());
        }
    }
}
