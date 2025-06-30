using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Dtos;

public record TokenDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}