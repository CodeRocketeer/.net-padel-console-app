using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Tennis/Padel Season Planner!");
        Console.WriteLine("1. Generate a new season.");
        Console.WriteLine("2. Load an existing season.");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            GenerateNewSeason();
        }
        else if (choice == "2")
        {
            LoadExistingSeason();
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    static void GenerateNewSeason()
    {
        List<string> players = new List<string>
        {
            "Player 1", "Player 2", "Player 3", "Player 4",
            "Player 5", "Player 6", "Player 7", "Player 8",
            "Player 9", "Player 10", "Player 11", "Player 12"
            };

        Console.WriteLine("Please select a day of the week for the matches (e.g., Monday):");
        string dayOfWeek = Console.ReadLine();

        List<Match> matches = ScheduleMatches(players, dayOfWeek);

        string seasonFileName = $"Season_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        SaveSeasonToFile(seasonFileName, matches);

        Console.WriteLine($"New season generated with {matches.Count} matches!");
        Console.WriteLine($"Matches scheduled for every {dayOfWeek}.");
        Console.WriteLine($"Season saved to {seasonFileName}");
    }

    static List<Match> ScheduleMatches(List<string> players, string dayOfWeek)
    {
        List<Match> matches = new List<Match>();
        int totalMatches = 20;
        DateTime startDate = DateTime.Now;

        // Generate list of players with exactly 2 matches per season
        Dictionary<string, int> playerMatches = players.ToDictionary(p => p, p => 0);

        Random rnd = new Random();
        while (matches.Count < totalMatches)
        {
            List<string> availablePlayers = playerMatches.Where(p => p.Value < 2).Select(p => p.Key).ToList();

            

            // Randomly select 4 players for the match
            var selectedPlayers = availablePlayers.OrderBy(x => rnd.Next()).Take(4).ToList();

            // Split into two teams
            var team1 = new Team(selectedPlayers[0], selectedPlayers[1]);
            var team2 = new Team(selectedPlayers[2], selectedPlayers[3]);

            // Ensure no player plays in consecutive matches
            if (matches.Count >= 1 && matches.Last().IsPlayerInvolved(selectedPlayers))
            {
                continue; // Skip if a player from the last match is involved
            }

            // Create match
            var match = new Match(dayOfWeek, startDate.AddDays(7 * matches.Count), team1, team2);
            matches.Add(match);

            // Update player match count
            foreach (var player in selectedPlayers)
            {
                playerMatches[player]++;
            }
        }

        return matches;
    }

    static void SaveSeasonToFile(string fileName, List<Match> matches)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var match in matches)
            {
                writer.WriteLine(match);
            }
        }
    }

    static void LoadExistingSeason()
    {
        Console.WriteLine("Enter the file name of the season to load:");
        string fileName = Console.ReadLine();

        if (File.Exists(fileName))
        {
            Console.WriteLine($"Loading season from {fileName}...");
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }
}

class Match
{
    public string DayOfWeek { get; }
    public DateTime Date { get; }
    public Team Team1 { get; }
    public Team Team2 { get; }

    public Match(string dayOfWeek, DateTime date, Team team1, Team team2)
    {
        DayOfWeek = dayOfWeek;
        Date = date;
        Team1 = team1;
        Team2 = team2;
    }

    public bool IsPlayerInvolved(List<string> players)
    {
        return Team1.Players.Intersect(players).Any() || Team2.Players.Intersect(players).Any();
    }

    public override string ToString()
    {
        return $"{Date.ToString("dd/MM/yyyy")} ({DayOfWeek}): {Team1} vs {Team2}";
    }
}

class Team
{
    public string Player1 { get; }
    public string Player2 { get; }

    public List<string> Players => new List<string> { Player1, Player2 };

    public Team(string player1, string player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public override string ToString()
    {
        return $"{Player1} & {Player2}";
    }
}
