using Microsoft.AspNetCore.Identity;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly PasswordHasher<User> _passwordHasher;
    public AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = new PasswordHasher<User>();
    }
    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByLoginAsync(username);
        if (user == null) return new AuthResponse()
        {
            Token = null,
        };
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Success)
        {
            var token = _jwtTokenService.GenerateToken(user);
            return new AuthResponse()
            {
                Token = token,
            };
        }
        return new AuthResponse()
        {
            Token = null,
        };
    }

    public async Task RegisterAsync(string username, string password, Email? email)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Login = username,
            Email = email
        };
        var passwordHash = _passwordHasher.HashPassword(user, password);
        user.PasswordHash = passwordHash;
        await _userRepository.AddUserAsync(user);
    }
}