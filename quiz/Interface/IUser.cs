using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    interface IUser
    {
        User Register(string login, string password, DateTime dateOfBirth);
        User Login(string login, string password);
    }
}
