using System.ComponentModel.DataAnnotations;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.DTO.Auth;

public class AuthRequest
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public required string Login { get; set; }
    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
    public required Email Email {get; set;}
}