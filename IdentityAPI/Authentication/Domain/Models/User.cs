namespace IdentityAPI.Authentication.Domain.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public List<Role> Roles { get; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Roles = [];
    }

    public void AddRole(RoleType roleType)
    {
        Role role = new(roleType);

        Roles.Add(role);
    }
}