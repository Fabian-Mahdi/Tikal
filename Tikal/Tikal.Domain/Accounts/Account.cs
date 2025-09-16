namespace Tikal.Domain.Accounts;

public class Account
{
    public string Id { get; set; }

    public string Name { get; set; }

    public Account(string id, string name)
    {
        Id = id;
        Name = name;
    }
}