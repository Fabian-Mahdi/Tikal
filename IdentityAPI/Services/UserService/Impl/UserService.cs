using IdentityAPI.Database;
using IdentityAPI.Database.Repositories.UserRepository.Exceptions;
using IdentityAPI.Models;
using IdentityAPI.Services.UserService.Exceptions;

namespace IdentityAPI.Services.UserService.Impl;

public class UserService : IUserService
{
    private readonly UnitOfWork unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<User> GetUser(string username, string password)
    {
        try
        {
            return await unitOfWork.UserRepository.GetUser(username, password);
        }
        catch (UserNotFoundException)
        {
            throw new InvalidCredentialsException();
        }
    }
}