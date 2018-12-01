using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel isolationLevel);
    }
}
