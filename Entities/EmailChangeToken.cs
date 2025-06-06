﻿namespace Todo.Api.Entities;

public class EmailChangeToken
{
    public Guid TokenId { get; set; }
    public Guid? UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string NewEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } 
    public User? User { get; set; }
}