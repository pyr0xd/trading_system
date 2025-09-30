namespace App;

// Representerar användare  med namn, lösenord och inventory
public class Users : Login
{
    //användare namn
    public string Name;
    //användare lösenord
    public string Password;

    //lista av items som tillhör användaren
    public List<Item> Inventory = new List<Item>();
    //konstruktor för användare
    public Users(string name, string password)
    {
        Name = name;
        Password = password;

    }
    // Kontrollerar att användarnamnet och lösenordet stämmer och finns
    public bool TryLogin(string name, string password)
    {
        return name == Name && password == Password;
    }
    //lägger till item i invetory
    public void AddItems(string itemName, int amount)
    {
        Inventory.Add(new Item(itemName, amount));
    }

    // Vissar mina items i mitt inventory
    public void ShowMyItems()
    {
        Console.WriteLine($"Du har");
        Console.WriteLine("\u203E\u203E\u203E\u203E\u203E");
        foreach (Item item in Inventory)

            Console.WriteLine($" {item.IName} x{item.Amount}");
    }
    // tar bort item via namn och antal 
    public bool RemoveItems(string itemName, int amount)
    {
        //Letar upp första match för namnet
        Item? item = Inventory.FirstOrDefault(i => i.IName == itemName);
        // Kontrollerar att användaren har så många att ta bort
        if (item != null && item.Amount >= amount)
        {
            // tar bort så många du valde
            item.Amount -= amount;
            // Om du tog alla tar den bort det från listan helt
            if (item.Amount == 0)
                Inventory.Remove(item);
            return true;
        }
        return false;
    }






}