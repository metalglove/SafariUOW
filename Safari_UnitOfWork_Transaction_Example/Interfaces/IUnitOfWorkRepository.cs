using System;

namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        bool Commit();
        bool Rollback();
    }
}
