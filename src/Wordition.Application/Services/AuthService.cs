using Microsoft.AspNetCore.Identity;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher;
    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<User>();
    }
    public async Task<AuthTokenDto> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByLoginAsync(username);
        if (user == null) return new AuthTokenDto()
        {
            JwtToken = null,
            RefreshToken = null,
        };
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Success)
        {
            await _userRepository.RemoveRefreshTokenAsync(user.Id);
            var jwtToken = _tokenService.GenerateJwtToken(user);
            var generateResult = _tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = generateResult.token,
                UserId = user.Id,
                User = user
            };
            await _userRepository.AddRefreshTokenAsync(refreshToken);
            return new AuthTokenDto()
            {
                JwtToken = jwtToken,
                RefreshToken = generateResult.tokenForCookie,
            };
        }
        return new AuthTokenDto()
        {
            JwtToken = null,
            RefreshToken = null,
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

    public async Task<AuthTokenDto> RefreshAsync(string refreshTokenFromCoockie)
    {
        
        var refreshTokenResult = await _userRepository.GetRefreshTokenAsync(_tokenService.GetHashToken(refreshTokenFromCoockie));
        if (refreshTokenResult == null || refreshTokenResult.ExpiresAt < DateTime.UtcNow)
            return new AuthTokenDto()
            {
                JwtToken = null,
                RefreshToken = null,
            };

        var generateResult = _tokenService.GenerateRefreshToken();
        await _userRepository.RemoveRefreshTokenAsync(refreshTokenResult.Token);
        var newRefreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            Token = generateResult.token,
            UserId = refreshTokenResult.UserId,
            User = refreshTokenResult.User,
        };
        await _userRepository.AddRefreshTokenAsync(newRefreshToken);
        var newJwtToken = _tokenService.GenerateJwtToken(newRefreshToken.User);
        return new AuthTokenDto()
        {
            JwtToken = newJwtToken,
            RefreshToken = generateResult.tokenForCookie,
        };
    }
    public async Task LogoutAsync(string refreshToken)
    {
        await _userRepository.RemoveRefreshTokenAsync(_tokenService.GetHashToken(refreshToken));
    }
}