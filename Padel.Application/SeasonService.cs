using Padel.Contracts;
using Padel.Domain.Models;
using Padel.Shared.Utils;
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

        public Season CreateSeason(string title, string dayOfWeek, DateTime startDate, List<User> players, int amountOfMatches)
        {
            // Use the utility function to parse and validate the day of the week
            DayOfWeek chosenDay = ValidationUtils.ParseDayOfWeek(dayOfWeek);
          
            // Adjust start date to the next occurrence of the chosen day
            while (startDate.DayOfWeek != chosenDay)
            {
                startDate = startDate.AddDays(1);
            }

            // Create a new season
            Season season = new Season(title, startDate, chosenDay);

            // Generate matches using the MatchService
            List<Match> matches = _matchService.CreateMatches(dayOfWeek, players, startDate, amountOfMatches);
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

        // Get the season with the given title, later on this will be replaced by an ID
        public Season GetSeasonByTitle(string title)
        {
            // Find the season with the given title
            return _seasons.Find(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        // Delete the season with the given title, later on this will be replaced by an ID
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
