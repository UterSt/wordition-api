using Microsoft.AspNetCore.Identity;
using Wordition.Application.DTO;
using Wordition.Application.DTO.Auth;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;
using Wordition.Domain.Exceptions;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _passwordHasher;
    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<User>();
    }
    public async Task<AuthTokenDto> LoginAsync(string username, string password)
    {
        var user = await _unitOfWork.User.GetByLoginAsync(username);
        if (user == null) return new AuthTokenDto()
        {
            JwtToken = null,
            RefreshToken = null,
        };
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Success)
        {
            await _unitOfWork.User.RemoveRefreshTokenAsync(user.Id);
            var jwtToken = _tokenService.GenerateJwtToken(user);
            var generateResult = _tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = generateResult.token,
                UserId = user.Id,
                User = user
            };
            await _unitOfWork.User.AddRefreshTokenAsync(refreshToken);
            await _unitOfWork.SaveAsync();
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
        var user = await _unitOfWork.User.GetByLoginAsync(username);
        if (user != null)
            throw new NotUniqueLoginException(username);
        
        user = new User()
        {
            Id = Guid.NewGuid(),
            Login = username,
            Email = email
        };
        var passwordHash = _passwordHasher.HashPassword(user, password);
        user.PasswordHash = passwordHash;
        await _unitOfWork.User.AddUserAsync(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task<AuthTokenDto> RefreshAsync(string refreshTokenFromCoockie)
    {
        
        var refreshTokenResult = await _unitOfWork.User.GetRefreshTokenAsync(_tokenService.GetHashToken(refreshTokenFromCoockie));
        if (refreshTokenResult == null || refreshTokenResult.ExpiresAt < DateTime.UtcNow)
            return new AuthTokenDto()
            {
                JwtToken = null,
                RefreshToken = null,
            };

        var generateResult = _tokenService.GenerateRefreshToken();
        await _unitOfWork.User.RemoveRefreshTokenAsync(refreshTokenResult.Token);
        var newRefreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            Token = generateResult.token,
            UserId = refreshTokenResult.UserId,
            User = refreshTokenResult.User,
        };
        await _unitOfWork.User.AddRefreshTokenAsync(newRefreshToken);
        var newJwtToken = _tokenService.GenerateJwtToken(newRefreshToken.User);
        await _unitOfWork.SaveAsync();
        return new AuthTokenDto()
        {
            JwtToken = newJwtToken,
            RefreshToken = generateResult.tokenForCookie,
        };
    }
    public async Task LogoutAsync(string refreshToken)
    {
        await _unitOfWork.User.RemoveRefreshTokenAsync(_tokenService.GetHashToken(refreshToken));
        await _unitOfWork.SaveAsync();
    }
}