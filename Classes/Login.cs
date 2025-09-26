namespace App;

interface Login
{
    public bool TryLogin(string username, string password);
}