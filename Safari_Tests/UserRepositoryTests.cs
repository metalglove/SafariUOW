using Microsoft.VisualStudio.TestTools.UnitTesting;
using Safari_UnitOfWork_Transaction_Example.Implementations;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Safari_Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IFactory<IUnitOfWork> UOWFactory { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }

        public UserRepositoryTests()
        {
            // Reset the database
            using (SqlConnection sqlConnection = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = SafariDb; Trusted_Connection = True; ConnectRetryCount = 0"))
            using (SqlCommand sqlCommand = new SqlCommand(Properties.Resources.SafariDbResetSql, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            UOWFactory = new UnitOfWorkFactory();
        }

        [TestMethod]
        public void AddNewUser()
        {
            User user = new User("Username");

            UnitOfWork = UOWFactory.Create();
            int userId = UnitOfWork.UserRepository.AddUser(user);
            UnitOfWork.Commit();
            UnitOfWork.Dispose();

            UnitOfWork = UOWFactory.Create();
            IEnumerable<User> users = UnitOfWork.UserRepository.GetAllUsers();
            UnitOfWork.Commit();
            UnitOfWork.Dispose();

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
    }
}
