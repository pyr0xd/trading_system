namespace App;

using System;
using System.Collections.Generic;
using System.Linq;

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
    public void ShowMyItems()
    {
        Console.WriteLine("‾‾‾‾‾‾");
        if (Inventory.Count == 0)
        {
            Console.WriteLine("tom");
        }
        else
        {
            foreach (var item in Inventory)
                Console.WriteLine($"{item.IName} x {item.Amount}");
        }
        Console.WriteLine("______");
    }
    // Kontrollerar att användarnamnet och lösenordet stämmer och finns
    public bool TryLogin(string name, string password)
    {
        return name == Name && password == Password;
    }
    //lägger till item i invetory
    // lägger till item i invetory (slår ihop med samma namn oavsett stora/små bokstäver)
    public void AddItems(string itemName, int amount)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(itemName)) return;
        var existing = Inventory.FirstOrDefault(i =>
            string.Equals(i.IName, itemName));
        if (existing != null) existing.Amount += amount;
        else Inventory.Add(new Item(itemName, amount));
    }

    // snabb koll: har användaren minst 'amount' av 'itemName'?
    public bool HasItems(string itemName, int amount)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(itemName)) return false;
        var item = Inventory.FirstOrDefault(i =>
            string.Equals(i.IName, itemName));
        return item != null && item.Amount >= amount;
    }

    // tar bort item via namn och antal 
    public bool RemoveItems(string itemName, int amount)
    {
        var item = Inventory.FirstOrDefault(i =>
            string.Equals(i.IName, itemName));
        if (item != null && item.Amount >= amount)
        {
            item.Amount -= amount;
            if (item.Amount == 0) Inventory.Remove(item);
            return true;
        }
        return false;
    }






}
