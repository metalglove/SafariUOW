using Safari_UnitOfWork_Transaction_Example.Implementations;
using Safari_UnitOfWork_Transaction_Example.Interfaces;

namespace Safari_UnitOfWork_Transaction_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            IFactory<IUnitOfWork> unitOfWorkFactory = new UnitOfWorkFactory();
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
            {
                unitOfWork.UserRepository.AddUser(new User("xzxvzx"));
                unitOfWork.UserRepository.AddUser(new User("xcvcxv"));
                unitOfWork.UserRepository.AddUser(new User("jcxvxcbngf"));
                unitOfWork.Commit();
            }
            // refer to the unit test project for the code..
        }
    }
}
