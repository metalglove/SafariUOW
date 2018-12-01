using System;

namespace Safari_UnitOfWork_Transaction_Example.Implementations
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegisterDate { get; set; }

        public User(string userName)
        {
            UserName = userName;
        }

        public User(int id, string userName, DateTime lastLogin, DateTime registerDate)
        {
            Id = id;
            UserName = userName;
            LastLogin = lastLogin;
            RegisterDate = registerDate;
        }
    }
}
