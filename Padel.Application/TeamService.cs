using Padel.Contracts;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application
{
    public class TeamService : ITeamService
    {
        private readonly List<Team> _teams = new List<Team>();

        public List<Team> CreateTeams(List<User> players)
        {
            _teams.Clear(); // Clear any existing teams before creating new ones

            // Split players into male and female
            List<User> malePlayers = players.Where(p => p.Sex == "M").ToList();
            List<User> femalePlayers = players.Where(p => p.Sex == "F").ToList();

            // Create teams by combining players
            _teams.AddRange(CreateBalancedTeams(malePlayers)); // Male-male teams
            _teams.AddRange(CreateBalancedTeams(femalePlayers)); // Female-female teams
            _teams.AddRange(CreateMixedTeams(malePlayers, femalePlayers)); // Mixed teams

            return _teams; // Return the list of teams
        }

        private List<Team> CreateBalancedTeams(List<User> players)
        {
            var teams = new List<Team>();

            // Create balanced teams (same gender)
            for (int i = 0; i < players.Count; i += 2)
            {
                if (i + 1 < players.Count)
                {
                    teams.Add(new Team(players[i], players[i + 1]));
                }
            }

            return teams;
        }

        private List<Team> CreateMixedTeams(List<User> males, List<User> females)
        {
            var teams = new List<Team>();

            int mixedTeamCount = Math.Min(males.Count, females.Count);
            for (int i = 0; i < mixedTeamCount; i++)
            {
                teams.Add(new Team(males[i], females[i]));
            }

            return teams;
        }

        public List<Team> GetTeams()
        {
            return _teams; // Return the stored teams
        }
    }
}
