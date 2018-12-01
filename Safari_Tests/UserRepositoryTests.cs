using Microsoft.VisualStudio.TestTools.UnitTesting;
using Safari_UnitOfWork_Transaction_Example.Implementations;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Safari_Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IUnitOfWorkFactory UOWFactory { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            UOWFactory = new UnitOfWorkFactory();
            UnitOfWork = UOWFactory.Create(IsolationLevel.ReadCommitted);
        }

        [TestMethod]
        public void AddNewUser()
        {
            User user = new User("Username");

            IEnumerable<User> users = UnitOfWork.UserRepository.GetAllUsers();
            Assert.AreEqual(0, users.Count());

            UnitOfWork.UserRepository.AddUser(user);
            UnitOfWork.Commit();
            UnitOfWork.Dispose();

            UnitOfWork = UOWFactory.Create(IsolationLevel.Serializable);

            users = UnitOfWork.UserRepository.GetAllUsers();
            UnitOfWork.Commit();
            UnitOfWork.Dispose();
            Assert.AreEqual(1, users.Count());

            User recievedUser = users.First();
            Assert.AreEqual(user.UserName, recievedUser.UserName);
        }
        [TestMethod]
        public void AddNewUserWithUsingStatements()
        {
            User user = new User("secondTestUser");

            IEnumerable<User> users;
            using (IUnitOfWork unitOfWork = UOWFactory.Create(IsolationLevel.ReadCommitted))
            {
                users = unitOfWork.UserRepository.GetAllUsers();
                unitOfWork.Commit();
            }
            Assert.AreEqual(1, users.Count());

            using (IUnitOfWork unitOfWork = UOWFactory.Create(IsolationLevel.ReadCommitted))
            {
                unitOfWork.UserRepository.AddUser(user);
                unitOfWork.Commit();
            }

            using (IUnitOfWork unitOfWork = UOWFactory.Create(IsolationLevel.Serializable))
            {
                users = unitOfWork.UserRepository.GetAllUsers();
                unitOfWork.Commit();
            }

            Assert.AreEqual(2, users.Count());

            User recievedUser = users.Last();
            Assert.AreEqual(user.UserName, recievedUser.UserName);
        }
    }
}
