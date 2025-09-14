namespace TikalBackend.PlayerAccount.Domain.Commands.CreateAccount;

public class DuplicateAccountId
{
    public string Id { get; set; }

    public DuplicateAccountId(string id)
    {
        Id = id;
    }
}