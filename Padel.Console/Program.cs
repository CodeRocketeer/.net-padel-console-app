using System;
using System.Collections.Generic;
using Padel.Application;
using Padel.Contracts;
using Padel.Domain.Models;

class Program
{

    static bool IsHardcodedProperties = false; // Set to true to use defaults, false to take user input


    static void Main(string[] args)
    {
        IUserService userService = new UserService(); 
        ITeamService teamService = new TeamService();
        IMatchService matchService = new MatchService(teamService);
        ISeasonService seasonService = new SeasonService(matchService);

        bool running = true;

        // Command loop
        while (running)
        {
            DisplayMainCommands();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateNewSeason(userService, seasonService);
                    break;
                case "2":
                    DisplayAllUsers(userService);
                    break;
                case "3":
                    DisplayAllSeasons(seasonService);
                    break;
                case "4":
                    running = false;
                    Console.WriteLine("Exiting program...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    break;
            }
        }
    }

    static void GenerateNewSeason(IUserService userService, ISeasonService seasonService)
    {
        List<User> players = new List<User>();
        int minPlayers = 4;


        if (IsHardcodedProperties)
        {
            // Populate users table with test users
            players = [
                 new User { Name = "John", Sex = "M" },
                new User { Name = "Jane", Sex = "F" },
                new User { Name = "Bob", Sex = "M" },
                new User { Name = "Alice", Sex = "F" },
                new User { Name = "David", Sex = "M" },
                new User { Name = "Sarah", Sex = "F" },
                new User { Name = "Tom", Sex = "M" },
                new User { Name = "Emily", Sex = "F" },
                new User { Name = "Michael", Sex = "M" },
                new User { Name = "Olivia", Sex = "F" },
           ];

                for (int i = 0; i < players.Count; i++)
            {
                userService.CreateUser(players[i].Name, players[i].Sex);
            }
                

            
        }
        else
        {
            Console.WriteLine("\n--- Create New Schedule ---");
            Console.WriteLine("Enter player information (type 'done' to finish):");

            // Add users
            while (true)
            {
                Console.Write("Enter player name: ");
                string name = Console.ReadLine();
                if (name.ToLower() == "done") break;

                Console.Write("Enter player sex (M/F): ");
                string sex = Console.ReadLine().ToUpper();
                while (sex != "M" && sex != "F")
                {
                    Console.WriteLine("Invalid input. Please enter 'M' for male or 'F' for female.");
                    sex = Console.ReadLine().ToUpper();
                }

                // Add the player using the UserService
                players = userService.CreateUser(name, sex);
            }
        }

        if (players.Count < minPlayers)
        {
            Console.WriteLine("Not enough players to create a schedule. You need at least 4 players.");
            return;
        }

        string dayOfWeek;
        string seasonTitle;
        DateTime startDate;
        int amountOfMatches;

        if (IsHardcodedProperties)
        {
            
            dayOfWeek = "Monday"; 
            seasonTitle = "Default Season Title"; 
            startDate = DateTime.Now; 
            amountOfMatches = 30;
        }
        else
        {
            // Prompt user for input
            Console.Write("Enter the day of the week for the matches (e.g., Monday): ");
            dayOfWeek = Console.ReadLine();
            // first letter should be capital
            dayOfWeek = dayOfWeek.Substring(0, 1).ToUpper() + dayOfWeek.Substring(1);

            Console.Write("Enter the season title: ");
            seasonTitle = Console.ReadLine();

            Console.Write("Enter the amount of matches to be played in the season: ");
            amountOfMatches = int.Parse(Console.ReadLine());

            Console.Write("Enter the start date for the season (yyyy-mm-dd): ");
            startDate = DateTime.Parse(Console.ReadLine());
        }

        Season season = seasonService.CreateSeason(seasonTitle, dayOfWeek, startDate, players, amountOfMatches);

        Console.WriteLine($"\nGenerated Season: {season.Title}");
        Console.WriteLine($"Start Date: {season.StartDate.ToShortDateString()}");
        Console.WriteLine($"End Date: {season.EndDate.ToShortDateString()}");

        Console.WriteLine($"\nScheduled Matches ({season.Matches.Count}):");
        foreach (var match in season.Matches)
        {
            Console.WriteLine(match);
        }
    }

    static void DisplayAllUsers(IUserService userService)
    {
        Console.WriteLine("\n--- Displaying All Users ---");
        var users = userService.GetAllUsers();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
        }
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Name}, Sex: {user.Sex}, ID: {user.Id}");
            }
        }
    }

    static void DisplayAllSeasons(ISeasonService seasonService)
    {
        Console.WriteLine("\n--- Displaying All Seasons ---");
        var seasons = seasonService.GetAllSeasons();

        if (seasons.Count == 0)
        {
            Console.WriteLine("No seasons found.");
            return;
        }

        // Display seasons with options
        foreach (var season in seasons)
        {
            Console.WriteLine($"Season: {season.Title}, Start Date: {season.StartDate.ToShortDateString()}, End Date: {season.EndDate.ToShortDateString()}");
        }

        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Edit a season");
        Console.WriteLine("2. Delete a season");
        Console.WriteLine("3. Cancel");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter the season title to edit: ");
                string seasonTitleToEdit = Console.ReadLine();
                DisplayEditSeason(seasonService, seasonTitleToEdit);
                break;
            case "2":
                Console.Write("Enter the season title to delete: ");
                string seasonTitleToDelete = Console.ReadLine();
                seasonService.DeleteSeason(seasonTitleToDelete); // You need to implement this method
                Console.WriteLine($"Season '{seasonTitleToDelete}' has been deleted.");
                break;
            case "3":
                Console.WriteLine("Canceled.");
                break;
            default:
                Console.WriteLine("Invalid option. Returning to main menu.");
                break;
        }
    }

    static void DisplayEditSeason(ISeasonService seasonService, string seasonTitle)
    {
        var season = seasonService.GetSeasonByTitle(seasonTitle);
        if (season == null)
        {
            Console.WriteLine("Season not found.");
            return;
        }

        Console.WriteLine($"\nSeason: {season.Title}, Start Date: {season.StartDate.ToShortDateString()}, End Date: {season.EndDate.ToShortDateString()}");

        // Display the scheduled matches
        Console.WriteLine($"\nScheduled Matches ({season.Matches.Count}):");
        foreach (var match in season.Matches)
        {
            Console.WriteLine(match);
        }

        // Here you could add additional options to update the season details
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("1. Update season details");
        Console.WriteLine("2. Cancel");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                // Logic for updating season details would go here
                Console.WriteLine("Updating season...");
                // For example: UpdateSeasonDetails(season);
                break;
            case "2":
                Console.WriteLine("Cancelled.");
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    static void DisplayMainCommands()
    {
        Console.WriteLine("\n--- Main Menu ---");
        Console.WriteLine("1. Create a new schedule");
        Console.WriteLine("2. Display all users");
        Console.WriteLine("3. Display all seasons");
        Console.WriteLine("4. Exit");
        Console.Write("Choose an option: ");
    }
}
