using System.ComponentModel.DataAnnotations;

namespace Tikal.Infrastructure.Entities;

/// <summary>
///     Contains all data related to an account of a player
/// </summary>
public class AccountEntity
{
    /// <summary>
    ///     The unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The display name shown to other players
    /// </summary>
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;
}