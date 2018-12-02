namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        bool Commit();
        bool Rollback();
    }
}
