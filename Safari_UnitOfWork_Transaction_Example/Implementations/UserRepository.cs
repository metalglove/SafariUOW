using Safari_UnitOfWork_Transaction_Example.Abstractions;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IFactory<TransactionScope, IsolationLevel> _transactionScopeFactory;
        private TransactionScope _transactionScope;

        public UserRepository() : base(null)
        {
            throw new ArgumentException("Should not be used....");
        }
        public UserRepository(SqlConnection sqlConnection, IFactory<TransactionScope, IsolationLevel> transactionScopeFactory) : base(sqlConnection)
        {
            _transactionScopeFactory = transactionScopeFactory;
        }

        public int AddUser(User user)
        {
            string QueryString = "INSERT INTO BasicUser (Username) OUTPUT Inserted.Id VALUES (@username);";
            int basicUserId = -1;
            _transactionScope = _transactionScopeFactory.Create(IsolationLevel.ReadCommitted);
            using (SqlConnection conn = new SqlConnection(_sqlConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(QueryString, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("username", user.UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    basicUserId = (int)reader["Id"];
                }
                conn.Close();
            }
            return basicUserId;
        }
        public IEnumerable<User> GetAllUsers()
        {
            string QueryString = "SELECT Id, Username, LastLogin, RegisterDate FROM BasicUser;";
            List<User> result = new List<User>();
            _transactionScope = _transactionScopeFactory.Create(IsolationLevel.Serializable);
            using (SqlConnection conn = new SqlConnection(_sqlConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(QueryString, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(CreateBasicUserFromReader(reader));
                }
                conn.Close();
                return result;
            }
        }

        private static User CreateBasicUserFromReader(SqlDataReader reader)
        {
            return new User(
                id: (int)reader["Id"],
                userName: (string)reader["Username"],
                lastLogin: DateTime.Now,
                registerDate: Convert.ToDateTime(reader["RegisterDate"])
            );
        }

        public override bool Commit()
        {
            try
            {
                _transactionScope.Complete();
                return true;
            }
            catch (Exception ex) 
            {
                Debug.Write($"{nameof(Commit)}, \nException: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _transactionScope.Dispose();
        }
    }
}
