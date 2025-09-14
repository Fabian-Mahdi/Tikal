namespace TikalBackend.PlayerAccount.Domain.Models;

public class Account
{
    public string Id { get; set; }

    public string Username { get; set; }

    public Account(string id, string username)
    {
        Id = id;
        Username = username;
    }
}