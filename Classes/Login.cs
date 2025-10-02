namespace App;
// en boll så att man kan se om någon är inloggad eller inte
public interface Login
{
    bool TryLogin(string username, string password);
}