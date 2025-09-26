namespace App;

public class Users : Login
{
    public string Name;
    public String Password;
    public Users(string name, string password)
    {
        Name = name;
        Password = password;

    }
    public bool TryLogin(string name, string password)
    {
        return name == Name && password == Password;
    }
}


