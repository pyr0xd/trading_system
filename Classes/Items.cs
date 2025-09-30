namespace App;

//class för items
public class Item

{
    // namn på item
    public string? IName;
    //hur många av det itemet
    public int Amount;

    //konstruktor för item
    public Item(string item, int amount)
    {
        IName = item;
        Amount = amount;
    }


}
