using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _sqlConnection;
        private readonly TransactionScope _transactionScope;

        public UserRepository(SqlConnection sqlConnection, TransactionScope transactionScope)
        {
            _sqlConnection = sqlConnection;
            _transactionScope = transactionScope;
        }

        public int AddUser(User user)
        {
            string QueryString = "INSERT INTO BasicUser (Username) OUTPUT Inserted.Id VALUES (@username);";
            int basicUserId = -1;
            using (TransactionScope scope = new TransactionScope())
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
                scope.Complete();
            }
            return basicUserId;
        }
        public IEnumerable<User> GetAllUsers()
        {
            string QueryString = "SELECT Id, Username, LastLogin, RegisterDate FROM BasicUser;";
            List<User> result = new List<User>();
            using (SqlConnection conn = new SqlConnection(_sqlConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(QueryString, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(CreateBasicUserFromReader(reader));
                }
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
    }
}
