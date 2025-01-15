using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Entities;


namespace Desktop.Repository
{
    public class UserRepository
    {
        private static readonly Lazy<UserRepository> _instance = new(() => new UserRepository());
        public static UserRepository Instance => _instance.Value;

        private static List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Name = "Ivan", Email = "Ivan24@mail.ru", Password = "123456" }
        };

        public UserModel? CurrentUser { get; private set; }

        //private UserRepository() { }

        public UserModel? GetUser(string email, string password)
        {
            foreach (var user in _users)
            {
                if (user.Email == email && user.Password == password)
                {
                    CurrentUser = user;
                    return user;
                }
            }
            return null;
        }

        public bool Register(string name, string email, string password)
        {
            if (_users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var newUser = new UserModel
            {
                Name = name,
                Email = email,
                Password = password
            };
            _users.Add(newUser);

            CurrentUser = newUser;
            return true;
        }
    }
}


