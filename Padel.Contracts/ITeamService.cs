using System.Collections.Generic;
using Padel.Domain.Models;

namespace Padel.Contracts
{
    public interface ITeamService
    {
        List<Team> CreateTeams( List<User> players);

        List<Team> GetTeams();


    }
}
