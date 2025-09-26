using System.ComponentModel;
using App;

UserManager manager = new UserManager();
manager.TempUser();




Console.WriteLine("hello");
string? choice = Console.ReadLine();

switch (choice)
{
    case "1":
        Console.WriteLine("login");
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
    default:
        break;
}
