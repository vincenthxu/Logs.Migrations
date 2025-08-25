using Microsoft.EntityFrameworkCore;

namespace Logs.Migrations
{
    internal class Program
    {
        #region Fields
        static AppDbContext db;
        static User user;
        static Entry entry;
        #endregion
        static void Main(string[] args)
        {
            db = new();
            EventLoop();
        }
        static void EventLoop()
        {
            do
            {
                DisplayPrompt();
                char c = GetOption();
                switch (c)
                {
                    case 'c': // Create: create a new object and save to the database
                        DisplayPrompt($"{c}");
                        Create(option: GetOption());
                        break;
                    case 'r': // Read: query the database for an object
                        DisplayPrompt($"{c}");
                        Read(option: GetOption());
                        break;
                    case 'u': // Update: update an object and save to the database
                        DisplayPrompt($"{c}");
                        Update(option: GetOption());
                        break;
                    case 'd': // Delete: delete an object from the database
                        DisplayPrompt($"{c}");
                        Delete(option: GetOption());
                        break;
                    case 'q': // Quit: quit the application
                        DisplayPrompt($"{c}");
                        return;
                    case 'g': // God-mode: display all entities in the database
                        DisplayDatabaseTables();
                        break;
                    default: // Catch-all: invalid selection case
                        RespondToInvalidInput($"{c}");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            while (true);
        }
        static void Create(char option)
        {
            switch (option)
            {
                case 'a':
                    DisplayPrompt($"{option}");
                    try
                    {
                        user = new(name: PromptUserForInput("Name"), dateOfBirth: DateOnly.Parse(PromptUserForInput("Date of birth")));
                        Console.WriteLine(user);
                        db.Add(user);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    DisplayPrompt($"{option}");
                    try
                    {
                        DateTime now = DateTime.Now;
                        DateOnly date = DateOnly.FromDateTime(now);
                        TimeOnly time = TimeOnly.FromDateTime(now);
                        Guid userId = Guid.Parse(PromptUserForInput("Id"));
                        entry = new(userId: userId, date: date, time: time, bristolStoolScale: BristolStoolScale.Type4);
                        Console.WriteLine(entry);
                        db.Add(entry);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    RespondToInvalidInput($"{option}");
                    break;
            }
        }
        static void Read(char option)
        {
            switch (option)
            {
                case 'a':
                    DisplayPrompt($"{option}");
                    try
                    {
                        user = db.Users.Where(u => u.Id == Guid.Parse(PromptUserForInput("Id"))).First();
                        Console.WriteLine(user);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    DisplayPrompt($"{option}");
                    try
                    {
                        entry = db.Entries.Where(e => e.Id == int.Parse(PromptUserForInput("Id"))).First();
                        Console.WriteLine(entry);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    RespondToInvalidInput($"{option}");
                    break;
            }
        }
        static void Update(char option)
        {
            switch (option)
            {
                case 'a':
                    DisplayPrompt($"{option}");
                    try
                    {
                        user = db.Users.Where(u => u.Id == Guid.Parse(PromptUserForInput("Id"))).First();
                        string name = PromptUserForInput("Name");
                        DateOnly date = DateOnly.Parse(PromptUserForInput("Date of birth"));
                        user.Name = name;
                        user.DateOfBirth = date;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    DisplayPrompt($"{option}");
                    try
                    {
                        entry = db.Entries.Where(e => e.Id == int.Parse(PromptUserForInput("Id"))).First();
                        DateOnly date = DateOnly.Parse(PromptUserForInput("Date"));
                        TimeOnly time = TimeOnly.Parse(PromptUserForInput("Time"));
                        BristolStoolScale bss = (BristolStoolScale)int.Parse(PromptUserForInput("Bristol stool scale"));
                        string? notes = PromptUserForInput("Notes");
                        entry.Date = date;
                        entry.Time = time;
                        entry.BristolStoolScale = bss;
                        entry.Notes = notes;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e}");
                    }
                    break;
                default:
                    RespondToInvalidInput($"{option}");
                    break;
            }
        }
        static void Delete(char option)
        {
            switch (option)
            {
                case 'a':
                    DisplayPrompt($"{option}");
                    try
                    {
                        user = db.Users.Where(u => u.Id == Guid.Parse(PromptUserForInput("Id"))).First();
                        foreach (var item in db.Entries.Where(e => e.UserId == user.Id))
                        {
                            db.Remove(item);
                        }
                        db.Remove(user);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    DisplayPrompt($"{option}");
                    try
                    {
                        entry = db.Entries.Where(e => e.Id == int.Parse(PromptUserForInput("Id"))).First();
                        db.Remove(entry);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    RespondToInvalidInput($"{option}");
                    break;
            }
        }
        static void DisplayDatabaseTables()
        {
            Console.Clear();
            Console.WriteLine("----------- Users -----------");
            db.Users.ForEachAsync(Console.WriteLine);
            Console.WriteLine("----------- Entries -----------");
            db.Entries.ForEachAsync(Console.WriteLine);
        }
        static void RespondToInvalidInput(string input)
        {
            Console.WriteLine($"\"{input}\" is not a valid choice.");
        }
        static string? PromptUserForInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine();
        }
        static void DisplayPrompt(string option = "") // TODO: fix menu prompting for code reusability
        {
            Console.Clear();
            string[] choices = ["a) User", "b) Entry"];
            Dictionary<string, string[]> prompt = new()
            {
                { "" , [ "Choose one of the following options:", "c) Create", "r) Read", "u) Update", "d) Delete", "g) God mode", "q) Quit" ] },
                { "c", [ "Create:", choices[0], choices[1] ] },
                { "r", [ "Query:" , choices[0], choices[1] ] },
                { "u", [ "Update:", choices[0], choices[1] ] },
                { "d", [ "Delete:", choices[0], choices[1] ] },
                { "a", [ "User"] },
                { "b", [ "Entry"] },
                { "q", [ "Goodbye" ] },
            };
            foreach (var item in prompt[option])
            {
                Console.WriteLine(item);
            }
        }
        static char GetOption()
        {
            char option;
            do
            {
                option = Console.ReadKey(true).KeyChar;
            }
            while (!('a' <= option && option <= 'z'));
            return option;
        }
    }
}