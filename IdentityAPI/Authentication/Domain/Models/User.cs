namespace IdentityAPI.Authentication.Domain.Models;

public class User
{
    public string Id { get; set; }

    public string Username { get; set; }

    public List<Role> Roles { get; }

    public User(string username)
    {
        Id = string.Empty;
        Username = username;
        Roles = [];
    }

    public User(string id, string username)
    {
        Id = id;
        Username = username;
        Roles = [];
    }

    public User(string id, string username, List<Role> roles)
    {
        Id = id;
        Username = username;
        Roles = roles;
    }

    public void AddRole(RoleType roleType)
    {
        Role role = new(roleType);

        Roles.Add(role);
    }
}