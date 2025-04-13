﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Entities;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TodoContext context, IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserLoginDto>> Register(UserRegistrationDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user == null)
        {
            return BadRequest("User with the same email or username already exists.");
        }
        return Ok(user);
    }
    
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await authService.VerifyEmailTokenAsync(token);
        if (!result)
            return BadRequest("Invalid or expired token.");

        return Ok("Email verified successfully.");
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
    {
        var response = await authService.LoginAsync(request);
        if (response is null)
        {
            return BadRequest("Invalid email or password.");
        }
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        var response = await authService.RefreshTokensAsync(request);
        if (response is null || response.RefreshToken is null || response.AccessToken is null)
        {
            return Unauthorized("Invalid refresh token.");
        }
        return Ok(response);
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();
        
        bool result = await authService.LogoutAsync(Guid.Parse(userId));
        if (!result) return BadRequest("Logout failed.");
        
        return Ok("Logout successful.");
    }
}