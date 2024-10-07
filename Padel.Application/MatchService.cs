using Padel.Contracts;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;

namespace Padel.Application
{
    public class MatchService : IMatchService
    {

        public List<Match> GenerateMatches(string dayOfWeek, List<User> players, DateTime startDate)
        {
            List<Match> matches = new List<Match>();

            // Convert string dayOfWeek to DayOfWeek enum
            DayOfWeek chosenDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek, true);

            // Create teams from players
            List<Team> teams = CreateTeams(players);

            // Generate 20 matches, one for each week starting from startDate
            for (int week = 0; week < 20; week++)
            {
                // Get two opposing teams for each match
                var team1 = teams[0];
                var team2 = teams[1];

                // Create the match for the given week
                var matchDate = startDate.AddDays(week * 7); 
                matches.Add(new Match(dayOfWeek, matchDate, team1, team2));

                // Rotate teams for the next match
                if (teams.Count > 2)
                {
                    teams.Add(teams[0]); // Move the first team to the end
                    teams.RemoveAt(0); // Remove the first team
                }
            }

            return matches;
        }

        private List<Team> CreateTeams(List<User> players)
        {
            var teams = new List<Team>();
            for (int i = 0; i < players.Count; i += 2)
            {
                if (i + 1 < players.Count)
                {
                    teams.Add(new Team(players[i], players[i + 1]));
                }
            }
            return teams;
        }
    }
}
