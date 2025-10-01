using System;
using System.IO;
using App;
// Skapar ett nytt UserManager-objekt som hanterar alla användare och deras lagring.
UserManager manager = new UserManager();
manager.LoadUsers(); // läs in sparade användare 
string ul = "\u203e";




bool running = true;
Users? active_user = null;

Console.WriteLine("hello");

while (running == true)
{
    //Om ingen är inloggad visa huvudmeny
    if (active_user == null)
    {
        Console.WriteLine("välj vad di vii göra");
        Console.WriteLine("1 : login");
        Console.WriteLine("2 lägga till användare");
        Console.WriteLine("6 : stänga av");

        //Läser in val
        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("login");
                Console.WriteLine("username");

                string Username = Console.ReadLine();
                Console.WriteLine("Password");
                string password = Console.ReadLine();
                //Kollar om användaren finns och lösenoret är rätt 
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
                Console.WriteLine(ul + ul + ul + ul + ul);
                Console.WriteLine("your item");
                active_user.ShowMyItems();
                Console.WriteLine("choose item");
                //ska vara en readline som hittar dit item
                Console.WriteLine("choice amount");
                // välj hur många
                Console.WriteLine("choose from who");
                manager.ShowAllUserItems(active_user);

                string Choice = Console.ReadLine();
                Console.Clear();
                manager.ShowSpecificUserItems(active_user, Choice);
                Console.WriteLine("choose item from list");
                Console.ReadKey();


                break;
            case "3":
                Console.WriteLine("remove item");
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
    //sparar innan programet är slut
    manager.SaveUsers();

}