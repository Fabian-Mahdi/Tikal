using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Controllers.Login.Dtos;

public record TokenDto
{
    [Required] public string AccessToken { get; set; } = string.Empty;
}