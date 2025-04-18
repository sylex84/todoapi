﻿namespace TodoApi.Dtos;

public class TodoItemDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public CategoryDto? Category { get; set; }
}