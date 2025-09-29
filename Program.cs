using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using App;

UserManager manager = new UserManager();
//ItemManager NewItem = new ItemManager();



manager.TempUser();
bool running = true;
Users? active_user = null;

Console.WriteLine("hello");

while (running == true)
{
    if (active_user == null)
    {
        Console.WriteLine("välj vad di vii göra");
        Console.WriteLine("1 : login");
        Console.WriteLine("2 lägga till användare");
        Console.WriteLine("6 : stänga av");


        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("login");
                Console.WriteLine("username");

                string Username = Console.ReadLine();
                Console.WriteLine("Password");
                string password = Console.ReadLine();
                foreach (Users user in manager.UserList)
                {
                    if (user.TryLogin(Username, password))
                    {
                        active_user = user;
                        Console.WriteLine($"välkomen{Username}!");
                    }



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
    else
    {
        Console.WriteLine("välj vad di vii göra");
        Console.WriteLine("1 : lägga till item");
        Console.WriteLine("2 : Trade");
        Console.WriteLine("3 : remove");
        Console.WriteLine("4 : Show all");
        Console.WriteLine("5 : Show mine");
        Console.WriteLine("6 : log out");
        Console.WriteLine("7 : stänga av");


        string? choice = Console.ReadLine();
        switch (choice)

        {
            case "1":
                Console.WriteLine("add item");
                String? item = Console.ReadLine();
                Console.WriteLine("amount");
                string? amount = Console.ReadLine();
                int.TryParse(amount, out int nAmount);
                active_user.AddItems(item!, nAmount);

                break;
            case "2":
                Console.WriteLine("trade item");
                break;
            case "3":
                Console.WriteLine("remove item");
                item = Console.ReadLine();
                amount = Console.ReadLine();
                int.TryParse(amount, out int Iamount);
                active_user.RemoveItems(item!, Iamount);
                break;
            case "4":
                Console.WriteLine("show items");

                manager.ShowAllUserItems(active_user);
                break;
            case "5":
                active_user.ShowMyItems();
                break;
            case "6":
                active_user = null;
                break;
            case "7":
                running = false;
                break;
        }
    }
}