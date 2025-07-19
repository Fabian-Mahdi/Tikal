using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.DataAccess;

public class UserCreationResult
{
    private readonly List<string> errors = [];

    public bool Succeeded { get; private set; }

    public IEnumerable<string> Errors => errors;

    public static UserCreationResult Success()
    {
        return new UserCreationResult
        {
            Succeeded = true
        };
    }

    public static UserCreationResult Failed(IEnumerable<string> errors)
    {
        UserCreationResult result = new()
        {
            Succeeded = false
        };

        result.errors.AddRange(errors);

        return result;
    }
}

public interface UserDataAccess
{
    Task<UserCreationResult> CreateUser(User user, string password);

    Task<User?> FindByName(string name);
}