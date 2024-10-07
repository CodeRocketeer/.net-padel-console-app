using System.Collections.Generic;
using Padel.Domain.Models;

namespace Padel.Contracts
{
    public interface IMatchService
    {
        List<Match> GenerateMatches(string dayOfWeek, List<User> players, DateTime startDate);

    }
}
