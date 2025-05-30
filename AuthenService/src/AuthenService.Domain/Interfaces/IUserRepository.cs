using AuthenService.Domain.Entities;

namespace AuthenService.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task UpdateRefreshTokenAsync(Guid userId, string newRefreshToken, DateTime refreshTokenExpiryTime);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}