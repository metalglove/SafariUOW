using Safari_UnitOfWork_Transaction_Example.Abstractions;
using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UserRepository : UnitOfWorkRepositoryBase, IUserRepository
    {
        public UserRepository(SqlConnection connection) : base(connection)
        {
           
        }

        public int AddUser(User user)
        {
            int userId = -1;
            SqlCommand.CommandText = "INSERT INTO BasicUser (Username) OUTPUT Inserted.Id VALUES (@username);";
            SqlCommand.Parameters.Clear();
            SqlCommand.Parameters.AddWithValue("username", user.UserName);
            using (SqlDataReader reader = SqlCommand.ExecuteReader())
            {
                reader.Read();
                userId = (int)reader["Id"];
            }
            return userId;
        }
        public IEnumerable<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            SqlCommand.CommandText = "SELECT Id, Username, LastLogin, RegisterDate FROM BasicUser;";
            using (SqlDataReader reader = SqlCommand.ExecuteReader())
            {
                while (reader.Read())
                    users.Add(CreateUserFromReader(reader));
            }
            return users;
        }

        private static User CreateUserFromReader(SqlDataReader reader)
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
