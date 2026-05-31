using System.ComponentModel.DataAnnotations;

namespace Wordition.Application.DTO.Auth;

public class LoginRequest
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public required string Login { get; set; }
    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
}