using Safari_UnitOfWork_Transaction_Example.Implementations;
using System;
using System.Collections.Generic;

namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUserRepository : IUnitOfWorkRepository, IDisposable
    {
        int AddUser(User user);
        IEnumerable<User> GetAllUsers();
    }
}
