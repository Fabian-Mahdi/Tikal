namespace IdentityAPI.Authentication.Domain.Models;

public enum RoleType
{
    User,
    Admin
}

public class Role
{
    public RoleType Type { get; private set; }

    public Role(RoleType type)
    {
        Type = type;
    }
}