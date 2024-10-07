using System.Collections.Generic;
using Padel.Domain.Models;

namespace Padel.Contracts
{
    public interface IUserService
    {
        List<User> AddUser(string name, string sex);
        List<User> GetAllUsers();

    }
}
