using System.ComponentModel.DataAnnotations;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.DTO;

public class AuthRequest
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Login { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [EmailAddress]
    public Email? Email { get; set; }
}