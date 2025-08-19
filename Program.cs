using Microsoft.EntityFrameworkCore;

namespace Logs.Migrations
{
    internal class Program
    {
        static AppDbContext db;
        static User user;
        static Entry entry;
        static void Main(string[] args)
        {
            db = new();
            Loop();
        }

        static void Loop()
        {
            do
            {
                Prompt();
                char c = GetChar();

                switch (c)
                {
                    case 'c':
                        Prompt($"{c}");
                        // Create: create a new object and save to the database
                        c = GetChar();
                        switch (c)
                        {
                            case 'a':
                                Prompt($"{c}");

                                Console.Write("Name: ");
                                string? name = Console.ReadLine();

                                Console.Write("Date of birth: ");
                                string? dob = Console.ReadLine();

                                user = new(name: name, dateOfBirth: DateOnly.Parse(dob));

                                Console.WriteLine(user);
                                db.Add(user);
                                db.SaveChanges();

                                break;
                            case 'b':
                                Prompt($"{c}");

                                DateTime now = DateTime.Now;
                                DateOnly date = DateOnly.FromDateTime(now);
                                TimeOnly time = TimeOnly.FromDateTime(now);

                                Console.Write("User name: ");
                                Guid userId = db.Users.Where(u => u.Name == Console.ReadLine()).First().UserId;

                                Entry entry = new(userId: userId, date: date, time: time, bristolStoolScale: BristolStoolScale.Type4);

                                Console.WriteLine(entry);
                                db.Add(entry);
                                db.SaveChanges();

                                break;
                        }
                        break;
                    case 'r':
                        Prompt($"{c}");
                        // Read: query the database for an object
                        c = GetChar();
                        switch (c)
                        {
                            case 'a':
                                Prompt($"{c}");
                                Console.Write("Name: ");
                                string? name = Console.ReadLine();

                                user = db.Users.Where(u => u.Name == name).First();
                                Console.WriteLine(user);

                                break;
                            case 'b':
                                Prompt($"{c}");
                                Console.Write("Name: ");
                                name = Console.ReadLine();

                                user = db.Users.Where(u => u.Name == name).First();
                                foreach (var item in db.Entries.Where(e => e.UserId == user.UserId))
                                    Console.WriteLine(item);

                                break;
                        }
                        break;
                    case 'u':
                        Prompt($"{c}");
                        // Update: update an object and save to the database
                        c = GetChar();
                        switch (c)
                        {
                            case 'a':
                                Console.WriteLine("Cannot update users");
                                break;
                            case 'b':
                                Console.Write("Name: ");
                                string? name = Console.ReadLine();

                                Console.Write("Date: ");
                                string? date = Console.ReadLine();

                                user = db.Users.Where(u => u.Name == name).First();
                                entry = db.Entries.Where(e => e.Date == DateOnly.Parse(date) && e.UserId == user.UserId).First();

                                Console.Write("Notes: ");
                                entry.Notes = Console.ReadLine();

                                db.SaveChanges();

                                break;
                        }
                        break;
                    case 'd':
                        Prompt($"{c}");
                        // Delete: delete an object from the database
                        c = GetChar();
                        switch (c)
                        {
                            case 'a':
                                Console.Write("Name: ");
                                string? name = Console.ReadLine();

                                user = db.Users.Where(u => u.Name == name).First();
                                db.Remove(user);
                                db.SaveChanges();

                                break;
                            case 'b':
                                Console.Write("Name: ");
                                name = Console.ReadLine();
                                Console.Write("Date: ");
                                string? date = Console.ReadLine();
                                user = db.Users.Where(u => u.Name == name).First();

                                foreach (var item in db.Entries.Where(e => e.UserId == user.UserId))
                                {
                                    db.Remove(item);
                                }
                                db.SaveChanges();

                                break;
                        }
                        break;
                    case 'q':
                        return;
                    default:
                        Console.WriteLine($"{c} is not a valid choice.");
                        break;
                }
                Console.ReadKey();
            }
            while (true);
        }

        static void Prompt(string option = "")
        {
            Console.Clear();
            string[] choices = ["a) User", "b) Entry"];
            Dictionary<string, string[]> prompt = new() {
                { "", [ "Choose one of the following options:", "c) Create", "r) Read", "u) Update", "d) Delete", "q) Quit" ] },
                { "c",  [ "Create:", choices[0], choices[1] ] },
                { "r",  [ "Query:" , choices[0], choices[1] ] },
                { "u",  [ "Update:", choices[0], choices[1] ] },
                { "d",  [ "Delete:", choices[0], choices[1] ] },
                { "a",  [ "User"] },
                { "b",  [ "Entry"] },
                { "q",  [ "Goodbye" ] },
            };
            foreach (var item in prompt[option])
                Console.WriteLine(item);
        }

        static char GetChar()
        {
            char c;
            while (!char.TryParse(Console.ReadLine(), out c)) ;
            return c;
        }
    }
}