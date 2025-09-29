namespace App;

using System;
using System.Collections.Generic;

class UserManager //skapa en lista
{
    public List<Users> UserList = new List<Users>();

    public void AddUser(string name, string password)//metod för att lägga till användare
    {
        UserList.Add(new Users(name, password));
    }


    public void ShowUser() //så att jag kan skriva ut användarnamn och password
    {
        foreach (Users user in UserList)
        {
            Console.WriteLine($"Name: {user.Name}, password: {user.Password}");
        }
    }
    // public void TempUser()
    // {
    //     UserList.Add(new Users("kalle", "har1"));
    //     UserList.Add(new Users("pelle", "har2"));
    //     UserList.Add(new Users("peter", "har3"));

    // }

    public void TempUser()
    {
        Users kalle = new Users("kalle", "har1");
        kalle.AddItems("banan", 1);
        kalle.AddItems("Apple", 5);
        UserList.Add(kalle);

        Users pelle = new Users("pelle", "har2");
        pelle.AddItems("stol", 1);
        pelle.AddItems("bord", 3);
        UserList.Add(pelle);

        Users peter = new Users("peter", "har3");
        peter.AddItems("hud", 1);
        peter.AddItems("blood", 20);
        UserList.Add(peter);
    }


    public void ShowAllUserItems(Users activeUsers)
    {
        foreach (Users user in UserList)
        {
            if (user == activeUsers)
                continue;
            Console.WriteLine($"{user.Name}");
            Console.WriteLine("‾‾‾‾‾‾");
            if (user.Inventory.Count == 0)
            {
                Console.WriteLine("tom");
            }
            else
            {
                foreach (Item item in user.Inventory)
                    Console.WriteLine($"{item.IName} x {item.Amount}");
            }

        }
    }

}