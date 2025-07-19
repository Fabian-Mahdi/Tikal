using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.UseCases;

public class RegisterResult
{
    private readonly List<string> errors = [];

    public bool Succeeded { get; private set; }

    public IEnumerable<string> Errors => errors;

    public static RegisterResult Success()
    {
        return new RegisterResult
        {
            Succeeded = true
        };
    }

    public static RegisterResult Failed(IEnumerable<string> errors)
    {
        RegisterResult result = new()
        {
            Succeeded = false
        };

        result.errors.AddRange(errors);

        return result;
    }
}

public class RegisterUser
{
    private readonly UserDataAccess _userDataAccess;

    public RegisterUser(UserDataAccess userDataAccess)
    {
        _userDataAccess = userDataAccess;
    }

    public async Task<RegisterResult> Register(User user, string password)
    {
        user.AddRole(RoleType.User);

        UserCreationResult userCreationResult = await _userDataAccess.CreateUser(user, password);

        return userCreationResult.Succeeded
            ? RegisterResult.Success()
            : RegisterResult.Failed(userCreationResult.Errors);
    }
}