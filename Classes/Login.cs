namespace App;
// en boll så att man kan se om någon är inloggad eller inte
interface Login
{
    public bool TryLogin(string username, string password);
}