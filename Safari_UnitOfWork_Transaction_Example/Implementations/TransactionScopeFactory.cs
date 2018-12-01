using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class TransactionScopeFactory : IFactory<TransactionScope, IsolationLevel>
    {
        public TransactionScope Create(IsolationLevel isolationLevel)
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                    Timeout = TransactionManager.DefaultTimeout
                });
        }
    }
}
