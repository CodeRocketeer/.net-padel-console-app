using System;
using System.Collections.Generic;
using Padel.Application;
using Padel.Contracts;
using Padel.Domain.Models;

class Program
{
    static void Main(string[] args)
    {
        IUserService userService = new UserService();
        IMatchService matchService = new MatchService();
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
            players = userService.AddUser(name, sex);
        }

        if (players.Count < 4)
        {
            Console.WriteLine("Not enough players to create a schedule. You need at least 4 players.");
            return;
        }

        Console.Write("Enter the day of the week for the matches (e.g., Monday): ");
        string dayOfWeek = Console.ReadLine();

        Console.Write("Enter the season title: ");
        string seasonTitle = Console.ReadLine();

        Console.Write("Enter the start date for the season (yyyy-mm-dd): ");
        DateTime startDate = DateTime.Parse(Console.ReadLine());

        // Generate the season
        Season season = seasonService.GenerateSeason(seasonTitle, dayOfWeek, startDate, players);

        Console.WriteLine($"\nGenerated Season: {season.Title}");
        Console.WriteLine($"Start Date: {season.StartDate.ToShortDateString()}");
        Console.WriteLine($"End Date: {season.EndDate.ToShortDateString()}");

        // Display the scheduled matches
        Console.WriteLine("\nScheduled Matches:");
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
        Console.WriteLine("\nScheduled Matches:");
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
