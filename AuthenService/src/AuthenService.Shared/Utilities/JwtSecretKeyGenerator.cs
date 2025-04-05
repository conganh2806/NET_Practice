using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthenService.Shared.Utilities;

public static class JwtSecretKeyGenerator
{
    private const int KeySize = 32;

    public static byte[] GenerateSecretKey()
    {
        byte[] key = new byte[KeySize];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    public static SymmetricSecurityKey GetSecurityKey(string secretKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }
}
