﻿namespace TodoApi.Dtos;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;
    public string Role { get; set; } = string.Empty;
    public DateTime Created { get; set; }
}