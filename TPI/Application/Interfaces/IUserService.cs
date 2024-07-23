using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserService
    {
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserByName(string name);
        User GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}

