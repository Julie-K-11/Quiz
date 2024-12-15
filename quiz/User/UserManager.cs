using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class UserManager : IUser
    {
        private List<User> users = new List<User>();
        private string filePath = "users.txt";

        public UserManager()
        {
            LoadUsers();
        }

        public User Register(string login, string password, DateTime dateOfBirth)
        {
            foreach (var user in users)
            {
                if (user.Login == login)
                {
                    throw new Exception("Логін зайнятий іншим користувачем");
                }
            }

            var newUser = new User { Login = login, Password = password, DateOfBirth = dateOfBirth };
            users.Add(newUser);
            SaveUsers();
            return newUser;
        }

        public User Login(string login, string password)
        {
            var user = users.FirstOrDefault(user => user.Login == login && user.Password == password);
            if (user == null)
            {
                throw new Exception("Невірний логін або пароль");
            }
            return user;
        }

        public void UpdateUser(User user)
        {
            var existingUser = users.FirstOrDefault(user => user.Login == user.Login);
            if (existingUser != null)
            {
                existingUser.Password = user.Password;
                existingUser.DateOfBirth = user.DateOfBirth;
                SaveUsers();
            }
        }

        private void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    users.Add(new User
                    {
                        Login = parts[0],
                        Password = parts[1],
                        DateOfBirth = DateTime.Parse(parts[2])
                    });
                }
            }
        }

        private void SaveUsers()
        {
            var lines = users.Select(user => $"{user.Login},{user.Password},{user.DateOfBirth:dd.MM.yyyy}");
            File.WriteAllLines(filePath, lines);
        }
    }
}