using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }
}