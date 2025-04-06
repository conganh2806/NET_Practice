using AuthenService.Domain.Models.Auth;
using AuthenService.Domain.Entities;
using AuthenService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AuthenService.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IJwtProvider jwtProvider, IConfiguration config)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _config = config;
    }

    public async Task<AuthResponse?> RegisterAsync(AuthRequest request)
    {
        try 
        {
            if (await _userRepository.EmailExistsAsync(request.Email)) return null;

            string passwordHash = HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            await _userRepository.AddUserAsync(user);
            return GenerateTokenAsync(user);
        }
        catch(Exception ex)
        {
            // Log exception (ex) here
            return null;
        }
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest request)
    {
        try 
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !PasswordMatches(request.Password, user.PasswordHash))
                return null;

            var response = GenerateTokenAsync(user);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = response.Expiration.AddDays(7);
            await _userRepository.UpdateUserAsync(user);
            
            return response;
        }
        catch(Exception e)
        {
            return null;
        }
    }

    public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
    {
        try 
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return null;

            var newRefreshToken = Guid.NewGuid().ToString();
            var newRefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, newRefreshTokenExpiryTime);

            var accessToken = GenerateTokenAsync(user);
            
            return new AuthResponse
            {
                Token = accessToken.Token,
                RefreshToken = newRefreshToken,
                Expiration = newRefreshTokenExpiryTime
            };
        }
        catch(Exception e)
        {
            return null;
        }
    }

    private bool PasswordMatches(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private AuthResponse GenerateTokenAsync(User user)
    {
        return  _jwtProvider.GenerateToken(user);
    }
}
