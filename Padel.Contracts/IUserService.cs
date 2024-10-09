using System.Collections.Generic;
using Padel.Domain.Models;

namespace Padel.Contracts
{
    public interface IUserService
    {
        List<User> CreateUser(string name, string sex);

        List<User> CreateUsers(List<User> users);
        List<User> GetAllUsers();

    }
}
