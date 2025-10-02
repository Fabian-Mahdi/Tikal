using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Identity.Mappers;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace Identity.Infrastructure.Identity;

public class UserDatabase : UserRepository
{
    private readonly UserMapper userMapper;

    private readonly UserManager<UserEntity> userManager;

    private readonly SignInManager<UserEntity> signInManager;

    public UserDatabase(
        UserMapper userMapper,
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager
    )
    {
        this.userMapper = userMapper;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<User?> FindByUsername(string username, CancellationToken cancellationToken)
    {
        UserEntity? user = await userManager.FindByNameAsync(username);

        return user == null ? null : userMapper.ToUser(user);
    }

    public async Task<OneOf<User, DuplicateUsername>> CreateUser(
        User user,
        string password,
        CancellationToken cancellationToken
    )
    {
        UserEntity userEntity = userMapper.FromUser(user);

        IdentityResult result = await userManager.CreateAsync(userEntity, password);

        if (!result.Succeeded)
        {
            return new DuplicateUsername(user.Username);
        }

        return userMapper.ToUser(userEntity);
    }

    public async Task<bool> ValidatePassword(User user, string password, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await userManager.FindByNameAsync(user.Username);

        if (userEntity is null)
        {
            return false;
        }

        SignInResult result = await signInManager.CheckPasswordSignInAsync(userEntity, password, false);

        return result.Succeeded;
    }
}