using Safari_UnitOfWork_Transaction_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class UserRepository : IUserRepository
    {
        private SqlCommand SqlCommand { get; set; }
        private SqlConnection SqlConnection { get; set; }
        private SqlTransaction SqlTransaction { get; set; }

        public UserRepository(SqlConnection connection)
        {
            SqlConnection = connection;
            SqlCommand = SqlConnection.CreateCommand();
            SqlConnection.Open();
            SqlTransaction = SqlConnection.BeginTransaction();
            SqlCommand.Transaction = SqlTransaction;
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

        public bool Commit()
        {
            try
            {
                SqlTransaction.Commit();
                return true;
            }
            catch (Exception ex) 
            {
                Debug.Write($"{nameof(Commit)}, \nException: {ex.Message}");
                SqlTransaction.Rollback();
                return false;
            }
        }
        public bool Rollback()
        {
            try
            {
                SqlTransaction.Rollback();
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public void Dispose()
        {
            SqlConnection.Close();
            SqlCommand.Dispose();
            SqlConnection.Dispose();
            SqlTransaction.Dispose();
        }
    }
}
