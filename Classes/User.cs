namespace App;

public class Users : Login
{
    public string Name;
    public String Password;

    public List<Item> Inventory = new List<Item>();

    public Users(string name, string password)
    {
        Name = name;
        Password = password;

    }
    public bool TryLogin(string name, string password)
    {
        return name == Name && password == Password;
    }
    public void AddItems(string itemName, int amount)
    {
        Inventory.Add(new Item(itemName, amount));
    }


    public void ShowMyItems()
    {
        Console.WriteLine($"Du har");
        Console.WriteLine("\u203E\u203E\u203E\u203E\u203E");
        foreach (Item item in Inventory)

            Console.WriteLine($" {item.IName} x{item.Amount}");
    }

    public bool RemoveItems(string itemName, int amount)
    {
        Item? item = Inventory.FirstOrDefault(i => i.IName == itemName);
        if (item != null && item.Amount >= amount)
        {
            item.Amount -= amount;
            if (item.Amount == 0)
                Inventory.Remove(item);
            return true;
        }
        return false;
    }






}