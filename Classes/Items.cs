namespace App;

public class Item
{
    public string? Items;
    public int Amount;


    public Item(string item, int amount)
    {
        Items = item;
        Amount = amount;
    }


}
class ItemManager
{
    public List<Item> ItemsList = new List<Item>();

    public void AddItems(string item, int amount)
    {
        ItemsList.Add(new Item(item, amount));
    }
    public void ShowItems()
    {
        foreach (Item item in ItemsList)
        {
            Console.WriteLine($"{item.Items} {item.Amount}");
        }
    }


}