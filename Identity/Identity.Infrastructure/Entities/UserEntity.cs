using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Entities;

/// <summary>
///     Contains all data related to a registered user, makes use of the identity framework
/// </summary>
public class UserEntity : IdentityUser<int>
{
}