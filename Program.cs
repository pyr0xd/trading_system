using System.ComponentModel;
using App;

UserManager manager = new UserManager();
manager.TempUser();

bool running = true;
Login? active_user = null;

Console.WriteLine("hello");

while (running == true)
{

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.WriteLine("login");
            string Username = Console.ReadLine();
            Console.WriteLine("login");
            string password = Console.ReadLine();
            foreach (Login user in manager.UserList)
            {
                if (user.TryLogin(Username, password))
                {
                    active_user = user;
                    Console.WriteLine($"välkomen{Username}!");
                }
                break;


            }
            break;
        case "2":
            Console.WriteLine("add User");
            Console.WriteLine("skriv ditt namn ");
            string? TempName = Console.ReadLine();
            Console.WriteLine("skriv ditt lösenord ");
            string? TempPassword = Console.ReadLine();
            manager.AddUser(TempName!, TempPassword!);
            break;
        case "3":
            Console.WriteLine("add item");
            break;
        case "4":
            Console.WriteLine("trade item");
            break;
        case "5":
            Console.WriteLine("show all users");
            manager.ShowUser();
            Console.ReadKey();
            break;
        case "6":
            running = false;
            break;
        default:
            break;
    }
}