using Microsoft.EntityFrameworkCore;

namespace Logs.Migrations
{
    internal class Program
    {
        #region Fields
        delegate void Operation();
        #endregion
        static void Main(string[] args)
        {
            EventLoop();
        }
        static void EventLoop()
        {
            do
            {
                DisplayPrompt();
                char option = GetOption();
                switch (option)
                {
                    case 'c': // Create: create a new object and save to the database
                        DisplayPrompt($"{option}");
                        CRUDAnObject(option: GetOption(), UserOperation: CreateUser, EntryOperation: CreateEntry);
                        break;
                    case 'r': // Read: query the database for an object
                        DisplayPrompt($"{option}");
                        CRUDAnObject(option: GetOption(), UserOperation: ReadUser, EntryOperation: ReadEntry);
                        break;
                    case 'u': // Update: update an object and save to the database
                        DisplayPrompt($"{option}");
                        CRUDAnObject(option: GetOption(), UserOperation: UpdateUser, EntryOperation: UpdateEntry);
                        break;
                    case 'd': // Delete: delete an object from the database
                        DisplayPrompt($"{option}");
                        CRUDAnObject(option: GetOption(), UserOperation: DeleteUser, EntryOperation: DeleteEntry);
                        break;
                    case 'q': // Quit: quit the application
                        DisplayPrompt($"{option}");
                        return;
                    case 'g': // God-mode: display all entities in the database
                        DisplayDatabaseTables();
                        break;
                    default: // Catch-all: invalid selection case
                        RespondToInvalidInput($"{option}");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            while (true);
        }
        static void CRUDAnObject(char option, Operation UserOperation, Operation EntryOperation)
        {
            switch (option)
            {
                case 'a': // CRUD a User
                    DisplayPrompt($"{option}");
                    UserOperation();
                    break;
                case 'b': // CRUD an Entry
                    DisplayPrompt($"{option}");
                    EntryOperation();
                    break;
                default: // Catch-all for invalid option
                    RespondToInvalidInput($"{option}");
                    break;
            }
        }
        private static void CreateUser()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    string name = PromptUserForInput("Name");
                    DateOnly dob = DateOnly.Parse(PromptUserForInput("Date of birth"));
                    User user = new(name: name, dateOfBirth: dob);
                    Console.WriteLine(user);
                    db.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void CreateEntry()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    DateOnly date = DateOnly.FromDateTime(now);
                    TimeOnly time = TimeOnly.FromDateTime(now);
                    Guid userId = Guid.Parse(PromptUserForInput("User Id"));
                    BristolStoolScale bristolStoolScale = BristolStoolScale.Type4;
                    Entry entry = new(userId: userId, date: date, time: time, bristolStoolScale: bristolStoolScale);
                    Console.WriteLine(entry);
                    db.Add(entry);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void ReadUser()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    Guid userId = Guid.Parse(PromptUserForInput("Id"));
                    User user = db.Users.Where(u => u.Id == userId).First();
                    Console.WriteLine(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void ReadEntry()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    int entryId = int.Parse(PromptUserForInput("Id"));
                    Entry entry = db.Entries.Where(e => e.Id == entryId).First();
                    Console.WriteLine(entry);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void UpdateUser()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    Guid userId = Guid.Parse(PromptUserForInput("Id"));
                    User user = db.Users.Where(u => u.Id == userId).First();
                    string name = PromptUserForInput("Name");
                    DateOnly dob = DateOnly.Parse(PromptUserForInput("Date of birth"));
                    user.Name = name;
                    user.DateOfBirth = dob;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void UpdateEntry()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    int entryId = int.Parse(PromptUserForInput("Id"));
                    Entry entry = db.Entries.Where(e => e.Id == entryId).First();
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
            }
        }
        private static void DeleteUser()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    Guid userId = Guid.Parse(PromptUserForInput("Id"));
                    User user = db.Users.Where(u => u.Id == userId).First();
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
            }
        }
        private static void DeleteEntry()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    int entryId = int.Parse(PromptUserForInput("Id"));
                    Entry entry = db.Entries.Where(e => e.Id == entryId).First();
                    db.Remove(entry);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void DisplayDatabaseTables()
        {
            using (var db = new AppDbContext())
            {
                Console.Clear();
                Console.WriteLine("----------- Users -----------");
                db.Users.ForEachAsync(Console.WriteLine);
                Console.WriteLine("----------- Entries -----------");
                db.Entries.ForEachAsync(Console.WriteLine);
            }
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
    }
}