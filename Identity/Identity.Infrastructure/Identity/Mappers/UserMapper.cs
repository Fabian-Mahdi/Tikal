using Identity.Domain.Identity;
using Identity.Infrastructure.Entities;

namespace Identity.Infrastructure.Identity.Mappers;

/// <summary>
///     Used to map <see cref="User" /> to <see cref="UserEntity" /> and vice versa
/// </summary>
public class UserMapper
{
    /// <summary>
    ///     Maps a given <see cref="User" /> to a <see cref="UserEntity" />
    /// </summary>
    /// <param name="user">The <see cref="User" /> to map</param>
    /// <returns>The resulting <see cref="UserEntity" /></returns>
    public UserEntity FromUser(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            UserName = user.Username
        };
    }

    /// <summary>
    ///     Maps a given <see cref="UserEntity" /> to a <see cref="User" />
    /// </summary>
    /// <param name="userEntity">The <see cref="UserEntity" /> to map</param>
    /// <returns>The resulting <see cref="User" /></returns>
    public User ToUser(UserEntity userEntity)
    {
        return new User(userEntity.Id, userEntity.UserName ?? string.Empty);
    }
}