using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Tikal.Infrastructure.Entities;

[Index(nameof(Id), IsUnique = true)]
public class AccountEntity
{
    [MaxLength(120)] public string Id { get; set; } = string.Empty;

    [MaxLength(120)] public string Name { get; set; } = string.Empty;
}