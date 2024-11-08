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
        private static List<UserModel> _users = new List<UserModel>()
       {
           new UserModel { Name = "Margarita", Email = "margarita.busarova13@gmail.com", Password = "123456" },
           new UserModel { Name = "Sigma", Email = "sigma@example.com", Password = "password" }
       };

        public UserModel? GetUser(string email, string password)
        {
            foreach (var user in _users)
            {
                if (user.Email == email && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }


        // Метод для регистрации пользователя
        public bool Register(string username, string email, string password)
        {
            // Проверка на уникальность email
            if (_users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Email занят
            }

            // Добавление нового пользователя
            _users.Add(new UserModel { Name = username, Email = email, Password = password });

            return true; // Регистрация успешна
        }

    }
}
