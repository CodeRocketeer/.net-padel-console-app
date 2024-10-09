using System;
using System.Collections.Generic;
using System.Linq;
using Padel.Contracts;
using Padel.Domain.Models;

namespace Padel.Application
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();

        public List<User> CreateUser(string name, string sex)
        {
         
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Sex = sex
            };

            _users.Add(user);
            return _users;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public List<User> CreateUsers(List<User> users)
        {
            _users.AddRange(users);

            return _users;
        }
    }
}
