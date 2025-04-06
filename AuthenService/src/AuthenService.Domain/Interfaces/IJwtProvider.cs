using AuthenService.Domain.Models.Auth;
using AuthenService.Domain.Entities;

namespace AuthenService.Domain.Interfaces
{
    public interface IJwtProvider
    {
        AuthResponse GenerateToken(User user);
    }
}
