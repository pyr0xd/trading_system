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
    public void TempUser()
    {
        UserList.Add(new Users("kalle", "har1"));
        UserList.Add(new Users("pelle", "har2"));
        UserList.Add(new Users("peter", "har3"));

    }

}