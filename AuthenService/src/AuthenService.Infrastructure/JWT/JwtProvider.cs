using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenService.Domain.Models.Auth;
using AuthenService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthenService.Domain.Interfaces;

namespace AuthenService.Infrastructure.JWT;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _config;

    public JwtProvider(IConfiguration config)
    {
        _config = config;
    }

    public AuthResponse GenerateToken(User user)
    {
        string role = "User";
        
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthResponse 
        {
            Token = tokenHandler.WriteToken(token),
            RefreshToken = Guid.NewGuid().ToString(),
            Expiration = tokenDescriptor.Expires!.Value
        };
    }
}