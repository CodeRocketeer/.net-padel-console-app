using System.Collections.Generic;
using Padel.Domain.Models;

namespace Padel.Contracts
{
    public interface ISeasonService
    {
        Season GenerateSeason(string title, string dayOfWeek, DateTime startDate, List<User> players);
        List<Season> GetAllSeasons();
        Season GetSeasonByTitle(string title);
        Season DeleteSeason(string title);

    }
}
