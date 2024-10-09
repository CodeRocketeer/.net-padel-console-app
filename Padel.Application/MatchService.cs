using Padel.Contracts;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application
{
    public class MatchService : IMatchService
    {
        private readonly ITeamService _teamService;

        public MatchService(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public List<Match> CreateMatches(string dayOfWeek, List<User> players, DateTime startDate, int amountOfMatches)
        {
            List<Match> matches = new List<Match>();
            Dictionary<User, int> playerParticipation = players.ToDictionary(player => player, player => 0);
            Dictionary<User, DateTime?> lastPlayed = players.ToDictionary(player => player, player => (DateTime?)null);

            // Start generating matches until we reach the specified amount
            while (matches.Count < amountOfMatches)
            {
                // Calculate the next match date
                DateTime matchDate = GetNextMatchDate(startDate, dayOfWeek, matches.Count);

                // Shuffle the players for random team selection based on last played date
                var availablePlayers = players
                    .Where(p => !lastPlayed[p].HasValue || (matchDate - lastPlayed[p].Value).TotalDays > 7) // Ensure no consecutive matches
                    .OrderBy(p => Guid.NewGuid()) // Randomize the order
                    .ToList();

                // Ensure we have enough players to form at least two teams
                if (availablePlayers.Count < 4)
                {
                    // If there aren't enough available players, randomly select from all players
                    availablePlayers = players
                        .OrderBy(p => Guid.NewGuid()) // Randomize the order
                        .ToList();
                }

                // Ensure we still have enough players after the random selection
                if (availablePlayers.Count < 4)
                {
                    throw new InvalidOperationException("Not enough players to create teams.");
                }

                // Create teams from the available players
                var team1 = new Team(availablePlayers[0], availablePlayers[1]); // Team 1
                var team2 = new Team(availablePlayers[2], availablePlayers[3]); // Team 2

                // Check if the teams are balanced
                if (AreTeamsBalanced(team1, team2))
                {
                    // Create and add the match to the list
                    matches.Add(new Match(dayOfWeek, matchDate, team1, team2));

                    // Update participation count and last played date
                    UpdateParticipationCount(playerParticipation, lastPlayed, team1, matchDate);
                    UpdateParticipationCount(playerParticipation, lastPlayed, team2, matchDate);
                }
            }

            return matches;
        }


        private DateTime GetNextMatchDate(DateTime startDate, string dayOfWeek, int matchIndex)
        {
            // Get the next match date based on the start date and the specified day of the week
            DateTime nextMatchDate = startDate;

            while (nextMatchDate.DayOfWeek.ToString() != dayOfWeek)
            {
                nextMatchDate = nextMatchDate.AddDays(1);
            }

            // Move to the next week for subsequent matches
            return nextMatchDate.AddDays(matchIndex * 7);
        }

        private bool AreTeamsBalanced(Team team1, Team team2)
        {
            // Check the gender combinations to ensure they are balanced
            bool team1Male = team1.Player1.Sex == "M" && team1.Player2.Sex == "M";
            bool team2Male = team2.Player1.Sex == "M" && team2.Player2.Sex == "M";
            bool team1Female = team1.Player1.Sex == "F" && team1.Player2.Sex == "F";
            bool team2Female = team2.Player1.Sex == "F" && team2.Player2.Sex == "F";

            // Check for mixed-gender teams
            bool team1Mixed = (team1.Player1.Sex == "M" && team1.Player2.Sex == "F") || (team1.Player1.Sex == "F" && team1.Player2.Sex == "M");
            bool team2Mixed = (team2.Player1.Sex == "M" && team2.Player2.Sex == "F") || (team2.Player1.Sex == "F" && team2.Player2.Sex == "M");

            // Return true if both teams are either MM vs MM, FF vs FF, or MF vs MF
            return (team1Male && team2Male) || (team1Female && team2Female) || (team1Mixed && team2Mixed);
        }

        private void UpdateParticipationCount(Dictionary<User, int> playerParticipation, Dictionary<User, DateTime?> lastPlayed, Team team, DateTime matchDate)
        {
            playerParticipation[team.Player1]++;
            playerParticipation[team.Player2]++;
            lastPlayed[team.Player1] = matchDate; // Update last played date
            lastPlayed[team.Player2] = matchDate; // Update last played date
        }
    }
}
