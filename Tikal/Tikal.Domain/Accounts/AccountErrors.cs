namespace Tikal.Domain.Accounts;

public class DuplicateAccountId
{
    public string Id { get; set; }

    public DuplicateAccountId(string id)
    {
        Id = id;
    }
}