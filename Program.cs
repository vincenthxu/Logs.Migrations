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
            Loop();
        }
        static void Loop()
        {
            do
            {
                DisplayOption();
                char c = GetOption();
                switch (c)
                {
                    case 'c': // Create: create a new object and save to the database
                        DisplayOption($"{c}");
                        Create(option: GetOption());
                        break;
                    case 'r': // Read: query the database for an object
                        DisplayOption($"{c}");
                        Read(option: GetOption());
                        break;
                    case 'u': // Update: update an object and save to the database
                        DisplayOption($"{c}");
                        Update(option: GetOption());
                        break;
                    case 'd': // Delete: delete an object from the database
                        DisplayOption($"{c}");
                        Delete(option: GetOption());
                        break;
                    case 'q': // Quit: quit the application
                        DisplayOption($"{c}");
                        return;
                    case 'g': // Display all entites in the database
                        db.Users.ForEachAsync(Console.WriteLine);
                        db.Entries.ForEachAsync(Console.WriteLine);
                        break;
                    default: // Catch-all: invalid selection case
                        PromptInvalidInput($"{c}");
                        break;
                }
                Console.ReadKey();
            }
            while (true);
        }
        static void Create(char option)
        {
            Console.Write("Create ");
            switch (option)
            {
                case 'a':
                    try
                    {
                        user = new(name: PromptUserInput("Name"), dateOfBirth: DateOnly.Parse(PromptUserInput("Date of birth")));
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
                    try
                    {
                        DateTime now = DateTime.Now;
                        DateOnly date = DateOnly.FromDateTime(now);
                        TimeOnly time = TimeOnly.FromDateTime(now);
                        Guid userId = db.Users.Where(u => u.Name == PromptUserInput("Name")).First().Id;
                        Entry entry = new(userId: userId, date: date, time: time, bristolStoolScale: BristolStoolScale.Type4);
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
                    PromptInvalidInput($"{option}");
                    break;
            }
        }
        static void Read(char option)
        {
            Console.Write("Read ");
            switch (option)
            {
                case 'a':
                    try
                    {
                        user = db.Users.Where(u => u.Name == PromptUserInput("Name")).First();
                        Console.WriteLine(user);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    try
                    {
                        int id = int.Parse(PromptUserInput("Id"));
                        entry = db.Entries.Where(e => e.Id == id).First();
                        Console.WriteLine(entry);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    PromptInvalidInput($"{option}");
                    break;
            }
        }
        static void Update(char option)
        {
            Console.Write("Update ");
            switch (option)
            {
                case 'a':
                    try
                    {
                        throw new NotImplementedException("User update feature not implemented!");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                case 'b':
                    try
                    {
                        int id = int.Parse(PromptUserInput("Id"));
                        entry = db.Entries.Where(e => e.Id == id).First();
                        entry.Date = DateOnly.Parse(PromptUserInput("Date"));
                        entry.Time = TimeOnly.Parse(PromptUserInput("Time"));
                        entry.BristolStoolScale = (BristolStoolScale)int.Parse(PromptUserInput("Bristol stool scale"));
                        entry.Notes = PromptUserInput("Notes");
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e}");
                    }
                    break;
                default:
                    PromptInvalidInput($"{option}");
                    break;
            }
        }
        static void Delete(char option)
        {
            Console.Write("Delete ");
            switch (option)
            {
                case 'a':
                    try
                    {
                        user = db.Users.Where(u => u.Name == PromptUserInput("Name")).First();
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
                    try
                    {
                        int id = int.Parse(PromptUserInput("Id"));
                        entry = db.Entries.Where(e => e.Id == id).First();
                        db.Remove(entry);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                default:
                    PromptInvalidInput($"{option}");
                    break;
            }
        }
        static void PromptInvalidInput(string input)
        {
            Console.WriteLine($"\"{input}\" is not a valid choice.");
        }
        static string? PromptUserInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine();
        }
        static void DisplayOption(string option = "") // TODO: fix menu prompting for code reusability
        {
            Console.Clear();
            string[] choices = ["a) User", "b) Entry"];
            Dictionary<string, string[]> prompt = new()
            {
                { "" , [ "Choose one of the following options:", "c) Create", "r) Read", "u) Update", "d) Delete", "q) Quit", "g) Display all" ] },
                { "c", [ "Create:", choices[0], choices[1] ] },
                { "r", [ "Query:" , choices[0], choices[1] ] },
                { "u", [ "Update:", choices[0], choices[1] ] },
                { "d", [ "Delete:", choices[0], choices[1] ] },
                { "a", [ "User"] },
                { "b", [ "Entry"] },
                { "q", [ "Goodbye" ] },
                { "g", [ "Display all entities in the database" ] },
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