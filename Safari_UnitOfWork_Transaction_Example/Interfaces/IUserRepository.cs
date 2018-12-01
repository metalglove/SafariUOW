using Safari_UnitOfWork_Transaction_Example.Implementations;
using System.Collections.Generic;

namespace Safari_UnitOfWork_Transaction_Example.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(User user);
        IEnumerable<User> GetAllUsers();
    }
}
