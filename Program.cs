using System;
using System.IO;
using App;
// Skapar ett nytt UserManager-objekt som hanterar alla användare och deras lagring.
UserManager manager = new UserManager();
manager.LoadUsers(); // läs in sparade användare 
string ul = "\u203e";
TradeManager tradeManager = new TradeManager();





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
        Console.WriteLine("8 : Browse trade requests (incoming/outgoing)");
        Console.WriteLine("9 : Accept/Deny a trade request");
        Console.WriteLine("10: Browse completed requests");



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
                Console.WriteLine("Create trade request");
                Console.WriteLine("Your items:");
                active_user.ShowMyItems();

                Console.Write("Offer item name: ");
                string? offerName = Console.ReadLine();
                Console.Write("Offer amount: ");
                int.TryParse(Console.ReadLine(), out int offerAmount);

                Console.WriteLine("Choose a user to trade with:");
                manager.ShowAllUserItems(active_user);
                Console.Write("Username: ");
                string? targetName = Console.ReadLine();
                var targetUser = manager.FindByName(targetName ?? "");

                if (targetUser == null)
                {
                    Console.WriteLine("User not found.");
                    break;
                }

                Console.WriteLine($"Items of {targetUser.Name}:");
                manager.ShowSpecificUserItems(active_user, targetUser.Name);

                Console.Write("Request item name: ");
                string? requestName = Console.ReadLine();
                Console.Write("Request amount: ");
                int.TryParse(Console.ReadLine(), out int requestAmount);

                var tr = tradeManager.Create(
                    active_user,
                    targetUser,
                    offerName ?? "",
                    offerAmount,
                    requestName ?? "",
                    requestAmount
                );

                Console.WriteLine($"Trade request created with id [{tr.Id}] and status '{tr.Status}'.");
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
            case "8":
                Console.WriteLine("Incoming (to you):");
                foreach (var t in tradeManager.Incoming(active_user.Name))
                    Console.WriteLine(t.ToString());

                Console.WriteLine("\nOutgoing (you sent):");
                foreach (var t in tradeManager.Outgoing(active_user.Name))
                    Console.WriteLine(t.ToString());
                break;

            case "9":
                Console.Write("Enter trade id to handle: ");
                if (!int.TryParse(Console.ReadLine(), out int handleId))
                {
                    Console.WriteLine("Invalid id.");
                    break;
                }

                Console.Write("Type 'a' to accept or 'd' to deny: ");
                var action = Console.ReadLine();

                if (action == "a")
                {
                    if (tradeManager.TryAccept(handleId, manager, active_user.Name, out string error))
                    {
                        Console.WriteLine("Trade completed!");
                        manager.SaveUsers(); // spara itemskifte
                    }
                    else
                    {
                        Console.WriteLine($"Could not accept: {error}");
                    }
                }
                else if (action == "d")
                {
                    bool ok = tradeManager.Deny(handleId);
                    Console.WriteLine(ok ? "Trade denied." : "Could not deny (maybe already handled?).");
                }
                else
                {
                    Console.WriteLine("Unknown choice.");
                }
                break;


            case "10":
                Console.WriteLine("Completed/Denied trades involving you:");
                foreach (var t in tradeManager.CompletedFor(active_user.Name))
                    Console.WriteLine(t.ToString());
                break;

        }
    }
    //sparar innan programet är slut
    manager.SaveUsers();

}