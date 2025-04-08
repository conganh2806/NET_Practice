using AuthenService.Domain.Models.Auth;

namespace AuthenService.Application.Services.Authentication;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(AuthRequest request);
    Task<AuthResponse> LoginAsync(AuthRequest request);
    Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
}