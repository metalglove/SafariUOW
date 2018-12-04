using Microsoft.VisualStudio.TestTools.UnitTesting;
using Safari_Tests.Utilities;
using Safari_UnitOfWork_Transaction_Example.Implementations;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Safari_Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IFactory<IUnitOfWork> UOWFactory { get; set; }

        [ClassInitialize]
        public static void DbCleanReset(TestContext testContext)
        {
            MsSqlDatabaseCreator databaseCreator = new MsSqlDatabaseCreator();
            databaseCreator.Create(
                "Server = (localdb)\\mssqllocaldb; Database = SafariDb; Trusted_Connection = True; ConnectRetryCount = 0", 
                Properties.Resources.SafariDbCreateBasicUserTable);
        }

        [TestInitialize]
        public void Initialize()
        {
            UOWFactory = new UnitOfWorkFactory();
        }

        [TestMethod]
        public void AddNewUserWithOutUsings()
        {
            User user = new User("Username");

            IUnitOfWork UnitOfWork = UOWFactory.Create();
            int userId = UnitOfWork.UserRepository.AddUser(user);
            UnitOfWork.Commit();
            UnitOfWork.Dispose();

            IUnitOfWork UnitOfWork2 = UOWFactory.Create();
            IEnumerable<User> users = UnitOfWork2.UserRepository.GetAllUsers();
            UnitOfWork2.Commit();
            UnitOfWork2.Dispose();

            User recievedUser = users.Single(usr => usr.Id.Equals(userId));
            Assert.AreEqual(user.UserName, recievedUser.UserName);
        }
        [TestMethod]
        public void AddNewUserWithUsingStatements()
        {
            User user = new User("randomUsername");

            int userId;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                userId = unitOfWork.UserRepository.AddUser(user);
                unitOfWork.Commit();
            }

            IEnumerable<User> users;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                users = unitOfWork.UserRepository.GetAllUsers();
                unitOfWork.Commit();
            }

            User recievedUser = users.Single(usr => usr.Id.Equals(userId));
            Assert.AreEqual(user.UserName, recievedUser.UserName);
        }
        [TestMethod]
        public void AddNewUsersAndRollBack()
        {
            int userId1, userId2, userId3;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                userId1 = unitOfWork.UserRepository.AddUser(new User("user1"));
                userId2 = unitOfWork.UserRepository.AddUser(new User("user2"));
                userId3 = unitOfWork.UserRepository.AddUser(new User("user3"));
                unitOfWork.Rollback();
            }

            IEnumerable<User> users;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                users = unitOfWork.UserRepository.GetAllUsers();
                unitOfWork.Commit();
            }

            Assert.IsFalse(users.Any(user => user.Id.Equals(userId1) || user.Id.Equals(userId2) || user.Id.Equals(userId3)));
        }
        [TestMethod]
        public void AddNewUsersAndCommit()
        {
            int userId1, userId2, userId3;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                userId1 = unitOfWork.UserRepository.AddUser(new User("user11"));
                userId2 = unitOfWork.UserRepository.AddUser(new User("user22"));
                userId3 = unitOfWork.UserRepository.AddUser(new User("user33"));
                unitOfWork.Commit();
            }

            IEnumerable<User> users;
            using (IUnitOfWork unitOfWork = UOWFactory.Create())
            {
                users = unitOfWork.UserRepository.GetAllUsers();
                unitOfWork.Commit();
            }
            IEnumerable<User> actualUsers = users.Where(user => user.Id.Equals(userId1) || user.Id.Equals(userId2) || user.Id.Equals(userId3));
            Assert.IsTrue(actualUsers.Count().Equals(3));
        }
    }
}
