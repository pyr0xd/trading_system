namespace App;

using System;
using System.Collections.Generic;
using System.IO;


class UserManager
{
    //så att jag slipper skriva hela sökvägen till filen
    public static readonly string memoryDir = "./memory/";
    public static readonly string userFile = "user.txt";
    public string userMemory = Path.Combine(memoryDir, userFile);
    //skapa en lista av alla användare.
    public List<Users> UserList = new List<Users>();

    public UserManager()
    {
        //skapar /memory/
        Directory.CreateDirectory(memoryDir);
        //skapar user.txt om det inte finns
        File.Create(userFile);
    }

    //sparar users och deras items till User.txt
    public void SaveUsers()
    {
        //skapar en lista av allt som ska sparas
        List<string> lines = new List<string>();
        //Loppar alla anvcändare i listan
        foreach (Users u in UserList)
        {
            //första raden är User: <användarnamn>;<lösenord>
            lines.Add($"User:{u.Name};{u.Password}");
            //under user sparar den items som Item: <Item>; <amaount>
            foreach (Item it in u.Inventory)
            {
                lines.Add($"Item:{it.IName};{it.Amount}");
            }
            //avslutar med ett sträck för att separera användare
            lines.Add("---");
        }
        //skriver ner alla användare
        File.WriteAllLines(userMemory, lines);
    }

    //läser täxtfilen och skriver in användarna i UserList.
    //rensar först befintlig lista 
    public void LoadUsers()
    {
        //tar bort gammal data i minnet
        UserList.Clear();
        //läser alla rader i User.txt
        string[] lines = File.ReadAllLines(userMemory);
        //håller kåll på vilken användare som skrivs in nu
        Users? current = null;
        //går igenom filen rad för rad och läser den som vi har skrivit den
        foreach (string raw in lines)
        {
            //tar bort onödiga mellanslag
            string line = raw.Trim();
            //hoppar över om raden är tom
            if (line.Length == 0) continue;
            //Om raden börjar med User: 
            if (line.StartsWith("User:"))
            {
                //tar bort User prefixet 
                string payload = line.Substring("User:".Length);
                // delar på lösenord och användare vid ;
                string[] parts = payload.Split(';');
                // Om det finns Användarnamn och Lösenord
                if (parts.Length == 2)
                {
                    // Skapar en ny User
                    current = new Users(parts[0], parts[1]);
                    // Lägger till den i UserList listan
                    UserList.Add(current);
                }
            }
            //Om raden börjar med Item och att current har en user
            else if (line.StartsWith("Item:") && current != null)
            {
                // tar bort prefix Item:
                string payload = line.Substring("Item:".Length);
                // delar på raden vid ;
                string[] parts = payload.Split(';');
                // kräver Item och Antal uppdelat och ser gör antal till int
                if (parts.Length == 2 && int.TryParse(parts[1], out int amount))
                {
                    //Lägger till det i User inventory
                    current.AddItems(parts[0], amount);
                }
            }
            //Om det är en rad --- så är nuvarande inventory slut och den går till nästa användare
            else if (line == "---")
            {
                current = null;
            }
        }
    }


    public void AddUser(string name, string password)//metod för att lägga till användare i UserList
    {
        UserList.Add(new Users(name, password));
    }


    public void ShowUser() //så att jag kan skriva ut användarnamn och password
    {
        //Loppar alla user i UserList
        foreach (Users user in UserList)
        {
            Console.WriteLine($"Name: {user.Name}, password: {user.Password}");
        }
    }



    //Skriver utt alla users och deras inventorys
    public void ShowAllUserItems(Users activeUsers)
    {
        //Loppar alla användare
        foreach (Users user in UserList)
        {
            //om det är nuvarande användare skippar den
            if (user == activeUsers)
                continue;

            Console.WriteLine($"{user.Name}");
            Console.WriteLine("‾‾‾‾‾‾");
            //om dom har ett tomt inventory
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
    public void ShowSpecificUserItems(Users activeUsers, string Choice)
    {
        //Loppar alla användare
        foreach (Users users in UserList)
        {
            if (users == activeUsers)
                continue;

            if (users.Name.Equals(Choice))
            {
                Console.WriteLine($"{users.Name}");
                Console.WriteLine("‾‾‾‾‾‾");

                if (users.Inventory.Count == 0)
                {
                    Console.WriteLine("tom");
                }
                else
                {
                    foreach (Item item in users.Inventory)
                        Console.WriteLine($"{item.IName} x {item.Amount}");
                }
            }
        }
    }
}