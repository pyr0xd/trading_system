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
        foreach (Item item in Inventory)
            Console.WriteLine($"{item.IName} x{item.Amount}");
    }



}


