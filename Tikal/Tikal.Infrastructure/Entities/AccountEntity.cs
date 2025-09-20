using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Tikal.Infrastructure.Entities;

/// <summary>
///     Contains all data related to an account of a player
/// </summary>
[Index(nameof(Id), IsUnique = true)]
public class AccountEntity
{
    /// <summary>
    ///     The unique identifier
    /// </summary>
    [MaxLength(120)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The display name shown to other players
    /// </summary>
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;
}