using Padel.Contracts;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;

namespace Padel.Application
{
    public class SeasonService : ISeasonService
    {
        private readonly IMatchService _matchService;
        private readonly List<Season> _seasons = new List<Season>();

        public SeasonService(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public Season GenerateSeason(string title, string dayOfWeek, DateTime startDate, List<User> players)
        {
            // Convert string dayOfWeek to DayOfWeek enum
            DayOfWeek chosenDay;
            try
            {
                chosenDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek, true);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Invalid day of the week: {dayOfWeek}");
            }

            // Adjust start date to the next occurrence of the chosen day
            while (startDate.DayOfWeek != chosenDay)
            {
                startDate = startDate.AddDays(1);
            }

            // Create a new season
            Season season = new Season(title, startDate, chosenDay);

            // Generate matches using the MatchService
            List<Match> matches = _matchService.GenerateMatches(dayOfWeek, players, startDate);
            season.Matches.AddRange(matches);

            // Set the season's end date
            season.SetEndDate();

            // Add the season to the internal list
            _seasons.Add(season);

            return season;
        }

        public List<Season> GetAllSeasons()
        {
            return _seasons;
        }

        public Season GetSeasonByTitle(string title)
        {
            // Find the season with the given title
            return _seasons.Find(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Season DeleteSeason(string title)
        {
            // Find the season with the given title
            var season = GetSeasonByTitle(title);

            if (season != null)
            {
                _seasons.Remove(season);
                return season;
            }

            return null; // Consider returning a custom exception instead
        }
    }
}
