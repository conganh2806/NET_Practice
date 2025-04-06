using AuthenService.Domain.Models.Auth;
using AuthenService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (result == null) return BadRequest("Email already exists.");
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (result == null) return Unauthorized("Invalid email or password.");
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        if (result == null) return Unauthorized("Invalid refresh token.");
        return Ok(result);
    }
}
